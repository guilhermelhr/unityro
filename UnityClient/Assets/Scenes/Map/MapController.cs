using Assets.Scripts.Renderer.Map;
using ROIO;
using ROIO.Models.FileTypes;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityRO.Core.GameEntity;
using UnityRO.GameCamera;

public class MapController : MonoBehaviour {

    public static MapController Instance;

    public MapUiController UIController;

    private NetworkClient NetworkClient;
    private GameManager GameManager;
    private EntityManager EntityManager;
    private PathFinder PathFinding;

    private async void Awake() {
        if (Instance == null) {
            Instance = this;
        }

        UIController = FindObjectOfType<MapUiController>();
        NetworkClient = FindObjectOfType<NetworkClient>();
        GameManager = FindObjectOfType<GameManager>();
        EntityManager = FindObjectOfType<EntityManager>();

        UIController.GetComponent<CanvasGroup>().alpha = 1;

        var mapInfo = NetworkClient.State.MapLoginInfo;
        if (mapInfo == null) {
            throw new Exception("Map Login info cannot be null");
        }

        NetworkClient.HookPacket(ZC.NOTIFY_STANDENTRY11.HEADER, OnEntitySpawn);
        NetworkClient.HookPacket(ZC.NOTIFY_NEWENTRY11.HEADER, OnEntitySpawn);
        NetworkClient.HookPacket(ZC.NOTIFY_MOVEENTRY11.HEADER, OnEntitySpawn);
        NetworkClient.HookPacket(ZC.NOTIFY_VANISH.HEADER, OnEntityVanish);
        NetworkClient.HookPacket(ZC.NOTIFY_MOVE.HEADER, OnEntityMovement); //Others movement
        NetworkClient.HookPacket(ZC.NPCACK_MAPMOVE.HEADER, OnEntityMoved);
        NetworkClient.HookPacket(ZC.HP_INFO.HEADER, OnEntityHpChanged);
        NetworkClient.HookPacket(ZC.STOPMOVE.HEADER, OnEntityMovement);
        NetworkClient.HookPacket(ZC.NOTIFY_EFFECT2.HEADER, OnEffect);
        NetworkClient.HookPacket(ZC.RESURRECTION.HEADER, OnEntityResurrected);
        NetworkClient.HookPacket(ZC.SPRITE_CHANGE2.HEADER, OnSpriteChanged);
        NetworkClient.HookPacket(ZC.ACTION_FAILURE.HEADER, OnActionFailure);
        NetworkClient.HookPacket(ZC.NOTIFY_TIME.HEADER, delegate{ });

        GameManager.InitCamera();

        var gameMap = await GameManager.BeginMapLoading(mapInfo.mapname);

        PathFinding = gameMap.GetPathFinder();
        NetworkClient.StartHeatBeat();
        NetworkClient.ResumePacketHandling();

        InitEntity(mapInfo);
    }

    private void InitEntity(MapLoginInfo mapInfo) {

        var entity = Session.CurrentSession.Entity as Entity;
        entity.transform.position = new Vector3(mapInfo.PosX, PathFinding.GetCellHeight(mapInfo.PosX, mapInfo.PosY), mapInfo.PosY);

        /**
        * Hack
        */
        CharacterCamera charCam = FindObjectOfType<CharacterCamera>();
        charCam.SetTarget(entity.EntityViewer.transform);

        entity.SetReady(true);
    }

    private void OnActionFailure(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.ACTION_FAILURE ACTION_FAILURE) {
            (Session.CurrentSession.Entity as Entity).StopMoving();
            switch (ACTION_FAILURE.ErrorCode) {
                case 0: // Please equip the proper amnution first
                    UIController.ChatBox.DisplayMessage(242, 0);
                    break;

                case 1:  // You can't Attack or use Skills because your Weight Limit has been exceeded.
                    UIController.ChatBox.DisplayMessage(243, 0);
                    break;

                case 2: // You can't use Skills because Weight Limit has been exceeded.
                    UIController.ChatBox.DisplayMessage(244, 0);
                    break;

                case 3: // Ammunition has been equipped.
                        // TODO: check the class - assassin: 1040 | gunslinger: 1175 | default: 245
                    UIController.ChatBox.DisplayMessage(245, 0);
                    break;
            }
        }
    }

    private void OnSpriteChanged(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.SPRITE_CHANGE2 SPRITE_CHANGE) {
            var entity = EntityManager.GetEntity(SPRITE_CHANGE.GID);
            if (entity == null) return;
            entity.OnSpriteChange(SPRITE_CHANGE.type, SPRITE_CHANGE.value, SPRITE_CHANGE.value2);
        }
    }

    private void OnEntityResurrected(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.RESURRECTION RESURRECTION) {
            var entity = EntityManager.GetEntity(RESURRECTION.GID);
            entity.ChangeMotion(new MotionRequest { Motion = SpriteMotion.Idle });
            
            // If it's our main character update Escape ui
            if (entity.AID == Session.CurrentSession.AccountID) {
                UIController.EscapeWindow.Hide();
            }
        }
    }

    private void OnEffect(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.NOTIFY_EFFECT2 NOTIFY_EFFECT2) {

            switch (NOTIFY_EFFECT2.EffectId) {
                case 313:
                    var entity = EntityManager.GetEntity(NOTIFY_EFFECT2.GID);
                    if (entity == null) break;

                    //var str = FileManager.Load("data/texture/effect/magnificat.str") as STR;
                    //var renderer = new GameObject().AddComponent<StrEffectRenderer>();
                    //renderer.transform.SetParent(entity.transform, false);
                    //renderer.Initialize(str);
                    break;
                default:
                    break;
            }

        }
    }

    private void OnEntityHpChanged(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.HP_INFO) {
            var pkt = packet as ZC.HP_INFO;

            var entity = EntityManager.GetEntity(pkt.GID);
            entity.UpdateHitPoints(pkt.Hp, pkt.MaxHp);
        }
    }

    private async void OnEntityMoved(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.NPCACK_MAPMOVE) {
            var pkt = packet as ZC.NPCACK_MAPMOVE;

            await GameManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
            var entity = Session.CurrentSession.Entity as Entity;
            entity.StopMoving();

            if (Path.GetFileNameWithoutExtension(pkt.MapName) != Session.CurrentSession.CurrentMap) {
                var mapname = Path.GetFileNameWithoutExtension(pkt.MapName);

                GameMap map = await GameManager.BeginMapLoading(mapname);
                PathFinding = map.GetPathFinder();

                Session.CurrentSession.SetCurrentMap(pkt.MapName);
            }

            entity.transform.position = new Vector3(pkt.PosX, PathFinding.GetCellHeight(pkt.PosX, pkt.PosY), pkt.PosY);
            new CZ.NOTIFY_ACTORINIT().Send();
            await GameManager.UnloadScene("LoadingScene");
        }
    }

    private void OnEntityVanish(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.NOTIFY_VANISH) {
            var pkt = packet as ZC.NOTIFY_VANISH;
            EntityManager.VanishEntity(pkt.AID, (VanishType)pkt.Type);

            // Show escape menu
            if (pkt.AID == Session.CurrentSession.AccountID && (VanishType)pkt.Type == VanishType.DIED) {
                UIController.EscapeWindow.BuildButtons(true);
                UIController.EscapeWindow.Show();
            }
        }
    }

    private void OnEntitySpawn(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.NOTIFY_NEWENTRY11) {
            var pkt = packet as ZC.NOTIFY_NEWENTRY11;
            EntityManager.Spawn(pkt.entityData);
        } else if (packet is ZC.NOTIFY_STANDENTRY11) {
            var pkt = packet as ZC.NOTIFY_STANDENTRY11;
            EntityManager.Spawn(pkt.entityData);
        } else if (packet is ZC.NOTIFY_MOVEENTRY11) {
            var pkt = packet as ZC.NOTIFY_MOVEENTRY11;
            var entity = EntityManager.Spawn(pkt.entityData);

            entity.ChangeMotion(new MotionRequest { Motion = SpriteMotion.Walk });
            entity.StartMoving(pkt.entityData.PosDir[0], pkt.entityData.PosDir[1], pkt.entityData.PosDir[2], pkt.entityData.PosDir[3]);
        }
    }

    private void OnEntityMovement(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.NOTIFY_MOVE) {
            var pkt = packet as ZC.NOTIFY_MOVE;

            var entity = EntityManager.GetEntity(pkt.AID);
            if (entity == null) return;

            entity.ChangeMotion(new MotionRequest { Motion = SpriteMotion.Walk });
            entity.StartMoving(pkt.StartPosition[0], pkt.StartPosition[1], pkt.EndPosition[0], pkt.EndPosition[1]);
        } else if (packet is ZC.STOPMOVE) {
            var pkt = packet as ZC.STOPMOVE;
            var entity = EntityManager.GetEntity(pkt.AID);
            if (entity == null) return;

            entity.ChangeMotion(new MotionRequest { Motion = SpriteMotion.Walk });
            entity.StartMoving((int)entity.transform.position.x, (int)entity.transform.position.z, pkt.PosX, pkt.PosY);
        }
    }

    // Start is called before the first frame update
    void Start() {
        new CZ.NOTIFY_ACTORINIT().Send();
    }
}

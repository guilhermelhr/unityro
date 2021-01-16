using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

    public static MapController Instance;

    [SerializeField] private Light worldLight;

    public MapUiController UIController;

    private void Awake() {

        if (Instance == null) {
            Instance = this;
        }

        UIController = FindObjectOfType<MapUiController>();
        UIController.GetComponent<CanvasGroup>().alpha = 1;

        var mapInfo = Core.NetworkClient.State.MapLoginInfo;
        if (mapInfo == null) {
            throw new Exception("Map Login info cannot be null");
        }

        Core.NetworkClient.HookPacket(ZC.NOTIFY_STANDENTRY11.HEADER, OnEntitySpawn);
        Core.NetworkClient.HookPacket(ZC.NOTIFY_NEWENTRY11.HEADER, OnEntitySpawn);
        Core.NetworkClient.HookPacket(ZC.NOTIFY_MOVEENTRY11.HEADER, OnEntitySpawn);
        Core.NetworkClient.HookPacket(ZC.NOTIFY_VANISH.HEADER, OnEntityVanish);
        Core.NetworkClient.HookPacket(ZC.NOTIFY_MOVE.HEADER, OnEntityMovement); //Others movement
        Core.NetworkClient.HookPacket(ZC.NPCACK_MAPMOVE.HEADER, OnEntityMoved);
        Core.NetworkClient.HookPacket(ZC.HP_INFO.HEADER, OnEntityHpChanged);
        Core.NetworkClient.HookPacket(ZC.STOPMOVE.HEADER, OnEntityMovement);

        Core.Instance.InitManagers();
        Core.Instance.InitCamera();
        Core.Instance.SetWorldLight(worldLight);
        Core.Instance.BeginMapLoading(mapInfo.mapname);

        //var entity = Core.EntityManager.SpawnPlayer(Core.NetworkClient.State.SelectedCharacter);
        //Core.Session = new Session(entity, Core.NetworkClient.State.LoginInfo.AccountID);
        //Core.Session.SetCurrentMap(mapInfo.mapname);
        Core.Session.Entity.transform.position = new Vector3(mapInfo.PosX, Core.PathFinding.GetCellHeight(mapInfo.PosX, mapInfo.PosY), mapInfo.PosY);

        /**
        * Hack
        */
        Core.MainCamera.GetComponent<ROCamera>().SetTarget(Core.Session.Entity.EntityViewer.transform);
        Core.MainCamera.transform.SetParent(Core.Session.Entity.transform);

        Core.Session.Entity.SetReady(true);
    }

    private void OnEntityHpChanged(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.HP_INFO) {
            var pkt = packet as ZC.HP_INFO;

            var entity = Core.EntityManager.GetEntity(pkt.GID);
            entity.UpdateHitPoints(pkt.Hp, pkt.MaxHp);
        }
    }

    private void OnEntityMoved(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.NPCACK_MAPMOVE) {
            var pkt = packet as ZC.NPCACK_MAPMOVE;

            if (pkt.MapName != Core.Session.CurrentMap) {
                Core.Session.Entity.StopMoving();
                Core.Instance.BeginMapLoading(pkt.MapName.Split('.')[0]);
                Core.Session.SetCurrentMap(pkt.MapName);
                Core.Session.Entity.transform.position = new Vector3(pkt.PosX, Core.PathFinding.GetCellHeight(pkt.PosX, pkt.PosY), pkt.PosY);
                new CZ.NOTIFY_ACTORINIT().Send();
            }
        }
    }

    private void OnEntityVanish(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.NOTIFY_VANISH) {
            var pkt = packet as ZC.NOTIFY_VANISH;
            Core.EntityManager.VanishEntity(pkt.AID, pkt.Type);
        }
    }

    private void OnEntitySpawn(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.NOTIFY_NEWENTRY11) {
            var pkt = packet as ZC.NOTIFY_NEWENTRY11;
            Core.EntityManager.Spawn(pkt.entityData);
        } else if (packet is ZC.NOTIFY_STANDENTRY11) {
            var pkt = packet as ZC.NOTIFY_STANDENTRY11;
            Core.EntityManager.Spawn(pkt.entityData);
        } else if (packet is ZC.NOTIFY_MOVEENTRY11) {
            var pkt = packet as ZC.NOTIFY_MOVEENTRY11;
            var entity = Core.EntityManager.Spawn(pkt.entityData);

            entity.ChangeMotion(SpriteMotion.Walk);
            entity.StartMoving(pkt.entityData.PosDir[0], pkt.entityData.PosDir[1], pkt.entityData.PosDir[2], pkt.entityData.PosDir[3]);
        }
    }

    private void OnEntityMovement(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.NOTIFY_MOVE) {
            var pkt = packet as ZC.NOTIFY_MOVE;

            var entity = Core.EntityManager.GetEntity(pkt.GID);
            if (entity == null) return;

            entity.ChangeMotion(SpriteMotion.Walk);
            entity.StartMoving(pkt.StartPosition[0], pkt.StartPosition[1], pkt.EndPosition[0], pkt.EndPosition[1]);
        } else if (packet is ZC.STOPMOVE) {
            var pkt = packet as ZC.STOPMOVE;
            var entity = Core.EntityManager.GetEntity(pkt.AID);
            if (entity == null) return;

            entity.ChangeMotion(SpriteMotion.Walk);
            entity.StartMoving((int)entity.transform.position.x, (int)entity.transform.position.z, pkt.PosX, pkt.PosY);
        }
    }

    // Start is called before the first frame update
    void Start() {
        new CZ.NOTIFY_ACTORINIT().Send();
    }
}

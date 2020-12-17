using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Entity : MonoBehaviour {

    private EntityWalk _EntityWalk;

    // Picking Priority
    // TODO

    public EntityType Type = EntityType.UNKNOWN;
    public EntityViewer EntityViewer;
    public Direction Direction = 0;
    public float ShadowSize;
    public int Action = 0;
    public int HeadDir;

    public bool IsReady = false;
    public bool HasAuthority => GID == Core.Session.Entity.GID;

    public uint GID;
    public short Job { get; private set; }
    public byte Sex { get; private set; }
    public short Hair { get; private set; }
    public short AttackSpeed { get; private set; }
    public short AttackRange { get; private set; } = 0;
    public short WalkSpeed { get; private set; } = 150;

    public void SetReady(bool ready) {
        IsReady = ready;
        _EntityWalk = gameObject.AddComponent<EntityWalk>();
    }

    public void StartMoving(int startX, int startY, int endX, int endY) {
        _EntityWalk.StartMoving(startX, startY, endX, endY);
    }

    public void Init(EntityData data) {
        Job = data.job;
        Sex = data.sex;
        Hair = data.hairStyle;
        Type = data.type;

        gameObject.transform.position = new Vector3(data.PosDir[0], Core.PathFinding.GetCellHeight(data.PosDir[0], data.PosDir[1]), data.PosDir[1]);
    }

    public void ChangeMotion(SpriteMotion motion) {
        EntityViewer.ChangeMotion(motion);
        //EntityViewer.State = AnimationHelper.GetStateForMotion(motion);
    }

    public void Init(CharacterData data) {
        Job = data.Job;
        Sex = (byte)data.Sex;
        Hair = data.Hair;
        Type = EntityType.PC;
    }

    private void Update() {

    }

    public void StopMoving() {
        _EntityWalk.StopMoving();
    }


    private void Awake() {
        Core.NetworkClient.HookPacket(ZC.NOTIFY_ACT3.HEADER, OnEntityAction);
    }

    private void OnEntityAction(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.NOTIFY_ACT3) {
            var pkt = packet as ZC.NOTIFY_ACT3;
            var srcEntity = Core.EntityManager.GetEntity(pkt.GID);
            var dstEntity = Core.EntityManager.GetEntity(pkt.targetGID);

            if (pkt.GID == Core.Session.Entity.GID || pkt.GID == Core.Session.AccountID) {
                srcEntity = Core.Session.Entity;
            } else if (pkt.targetGID == Core.Session.Entity.GID || pkt.targetGID == Core.Session.AccountID) {
                dstEntity = Core.Session.Entity;
            }
            Entity target;

            // entity out of screen
            if (!srcEntity) {
                return;
            }

            switch (pkt.action) {
                // Damage
                case 0:
                case 4:
                case 8:
                case 9:
                case 10:
                    if (dstEntity) {
                        // only if damage and do not have endure
                        // and damage isn't absorbed (healing)
                        if (pkt.damage > 0 && pkt.action != 9 && pkt.action != 4) {
                            dstEntity.ChangeMotion(SpriteMotion.Hit);
                            dstEntity.ChangeMotion(SpriteMotion.Standby);
                        }

                        target = pkt.damage > 0 ? dstEntity : srcEntity;

                        if (target) {
                            switch (pkt.action) {
                                // regular damage (and endure)
                                case 9:
                                case 0:
                                    //Damage.add(pkt.damage, target, Renderer.tick + pkt.attackMT);
                                    break;

                                // double attack
                                case 8:
                                    // Display combo only if entity is mob and the attack don't miss
                                    //if (dstEntity.objecttype === Entity.TYPE_MOB && pkt.damage > 0) {
                                    //    Damage.add(pkt.damage / 2, dstEntity, Renderer.tick + pkt.attackMT * 1, Damage.TYPE.COMBO);
                                    //    Damage.add(pkt.damage, dstEntity, Renderer.tick + pkt.attackMT * 2, Damage.TYPE.COMBO | Damage.TYPE.COMBO_FINAL);
                                    //}

                                    //Damage.add(pkt.damage / 2, target, Renderer.tick + pkt.attackMT * 1);
                                    //Damage.add(pkt.damage / 2, target, Renderer.tick + pkt.attackMT * 2);
                                    break;

                                // TODO: critical damage
                                case 10:
                                    //Damage.add(pkt.damage, target, Renderer.tick + pkt.attackMT);
                                    break;

                                // TODO: lucky miss
                                case 11:
                                    //Damage.add(0, target, Renderer.tick + pkt.attackMT);
                                    break;
                            }
                        }
                    }

                    srcEntity.AttackSpeed = (short)pkt.attackMT;
                    srcEntity.ChangeMotion(SpriteMotion.Attack1);
                    //srcEntity.ChangeMotion(SpriteMotion.Standby);
                    break;

                // Pickup Item
                case 1:
                    break;

                // Sit
                case 2:
                    break;

                // Stand
                case 3:
                    break;
            }
        }
    }
}

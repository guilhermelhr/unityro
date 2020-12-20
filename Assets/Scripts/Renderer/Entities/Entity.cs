using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Entity : MonoBehaviour {

    private EntityWalk _EntityWalk;
    public OutPacket MoveAction;

    // Picking Priority
    // TODO

    public EntityType Type = EntityType.UNKNOWN;
    public EntityViewer EntityViewer;
    public Direction Direction = 0;
    public float ShadowSize;
    public int Action = 0;
    public int HeadDir;

    public bool IsReady = false;
    public bool HasAuthority => GID == Core.Session?.Entity?.GID;

    public uint GID;
    public short Job { get; private set; }
    public byte Sex { get; private set; }
    public short Hair { get; private set; }
    public short AttackSpeed { get; private set; }
    public short AttackRange { get; private set; } = 0;
    public short WalkSpeed { get; private set; } = 150;
    public int Weapon { get; private set; }
    public int Hp { get; private set; }
    public int MaxHp { get; private set; }

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
        WalkSpeed = data.speed;
        Weapon = data.weapon;
        Hp = data.hp;
        MaxHp = data.maxhp;

        gameObject.transform.position = new Vector3(data.PosDir[0], Core.PathFinding.GetCellHeight(data.PosDir[0], data.PosDir[1]), data.PosDir[1]);
    }

    public void Init(CharacterData data) {
        Job = data.Job;
        Sex = (byte)data.Sex;
        Hair = data.Hair;
        WalkSpeed = data.Speed;
        Type = EntityType.PC;
        Weapon = data.Weapon;
        Hp = data.HP;
        MaxHp = data.MaxHP;
    }

    public void ChangeMotion(SpriteMotion motion, SpriteMotion? nextMotion = null) {
        EntityViewer.ChangeMotion(motion, nextMotion);
        //EntityViewer.State = AnimationHelper.GetStateForMotion(motion);
    }

    public void UpdateHitPoints(int hp, int maxHp) {
        this.Hp = hp;
        this.MaxHp = maxHp;
    }

    private void Update() {

    }

    public void StopMoving() {
        _EntityWalk.StopMoving();
    }


    private void Awake() {
        Core.NetworkClient.HookPacket(ZC.NOTIFY_ACT3.HEADER, OnEntityAction);
        Core.NetworkClient.HookPacket(ZC.PAR_CHANGE.HEADER, OnParameterChange);
        Core.NetworkClient.HookPacket(ZC.COUPLESTATUS.HEADER, OnParameterChange);
    }

    private void OnParameterChange(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.PAR_CHANGE) {
            var pkt = packet as ZC.PAR_CHANGE;

        }
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
                            dstEntity.ChangeMotion(SpriteMotion.Hit, SpriteMotion.Standby);
                        }

                        target = pkt.damage > 0 ? dstEntity : srcEntity;

                        if (target) {
                            switch (pkt.action) {
                                // regular damage (and endure)
                                case 9:
                                case 0:
                                    target.Damage(pkt.damage, Core.CurrentTime + pkt.attackMT);
                                    break;

                                // double attack
                                case 8:
                                    // Display combo only if entity is mob and the attack don't miss
                                    if (dstEntity.Type == EntityType.MOB && pkt.damage > 0) {
                                        dstEntity.Damage(pkt.damage / 2, Core.CurrentTime + pkt.attackMT * 1, DamageType.COMBO);
                                        dstEntity.Damage(pkt.damage, Core.CurrentTime + pkt.attackMT * 2, DamageType.COMBO | DamageType.COMBO_FINAL);
                                    }

                                    target.Damage(pkt.damage / 2, Core.CurrentTime + pkt.attackMT * 1);
                                    target.Damage(pkt.damage / 2, Core.CurrentTime + pkt.attackMT * 2);
                                    break;

                                // TODO: critical damage
                                case 10:
                                    target.Damage(pkt.damage, Core.CurrentTime + pkt.attackMT);
                                    break;

                                // TODO: lucky miss
                                case 11:
                                    target.Damage(0, Core.CurrentTime + pkt.attackMT);
                                    break;
                            }
                        }

                        srcEntity.LookTo(dstEntity.transform.position);
                    }

                    srcEntity.AttackSpeed = (short)pkt.attackMT;
                    srcEntity.ChangeMotion(SpriteMotion.Attack1, SpriteMotion.Standby);
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

    private void LookTo(Vector3 position) {
        var offset = new Vector2Int((int)position.x, (int)position.z) - new Vector2Int((int)transform.position.x, (int)transform.position.z);
        Direction = PathFindingManager.GetDirectionForOffset(offset);
    }

    /**
     * This method renders the damage sprites
     * The packet to receive damage data and etc is ZC_PAR_CHANGE
     */
    public void Damage(float amount, float tick, DamageType? damageType = null) {
        var DamagePrefab = (GameObject)Resources.Load("Prefabs/Damage");
        if (!DamagePrefab)
            throw new Exception("Could not load damage prefab");

        var damageRenderer = Instantiate(DamagePrefab).GetComponent<DamageRenderer>();
        damageRenderer.Display(amount, tick, damageType, this);
    }
}
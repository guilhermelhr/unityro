using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Entity : MonoBehaviour {

    public Action OnParameterUpdated;

    private EntityWalk _EntityWalk;
    public OutPacket AfterMoveAction;

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

    [SerializeField] public uint GID;
    [SerializeField] public short Job;
    [SerializeField] public byte Sex;
    [SerializeField] public short Hair;
    [SerializeField] public ushort AttackSpeed;
    [SerializeField] public short AttackRange = 0;
    [SerializeField] public short WalkSpeed = 150;
    [SerializeField] public int Weapon;
    [SerializeField] public int Hp;
    [SerializeField] public int MaxHp;

    public EntityBaseStatus Status = new EntityBaseStatus();

    public Inventory Inventory = new Inventory();

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
        Direction = (Direction)data.PosDir[2];

        gameObject.transform.position = new Vector3(data.PosDir[0], Core.PathFinding.GetCellHeight(data.PosDir[0], data.PosDir[1]), data.PosDir[1]);
    }

    public void Vanish(int type) {
        switch (type) {
            case 0: // Moved out of sight
                // TODO start coroutine to fade-out entity
                Core.EntityManager.RemoveEntity(GID);
                break;
            case 1: // Died
                var isPC = Type == EntityType.PC;
                ChangeMotion(SpriteMotion.Dead);
                if (!isPC) {
                    StartCoroutine(DestroyAfterSeconds());
                }
                break;
            default:
                Core.EntityManager.RemoveEntity(GID);
                break;
        }
    }

    IEnumerator DestroyAfterSeconds() {
        yield return new WaitForSeconds(1f);
        Core.EntityManager.RemoveEntity(GID);
        yield return null;
    }

    public void Init(SPR spr, ACT act) {
        EntityViewer.Init(spr, act);
    }

    // TODO refactor to use only Status class
    public void Init(CharacterData data) {
        Job = data.Job;
        Sex = (byte)data.Sex;
        Hair = data.Hair;
        WalkSpeed = data.Speed;
        Type = EntityType.PC;
        Weapon = data.Weapon;

        Status.base_exp = (uint)data.Exp;
        Status.base_level = (uint)data.BaseLevel;
        Status.job_exp = (uint)data.JobExp;
        Status.job_level = (uint)data.JobLevel;
        Status.sp = data.SP;
        Status.max_sp = data.MaxSP;
        Status.class_ = data.Job;
        Status.sex = (byte)data.Sex;
        Status.hp = data.HP;
        Status.max_hp = data.MaxHP;
        Status.name = data.Name;
    }

    public void ChangeMotion(SpriteMotion motion, SpriteMotion? nextMotion = null, ushort speed = 0) {
        EntityViewer.ChangeMotion(motion, nextMotion, speed);
    }

    public void UpdateHitPoints(int hp, int maxHp) {
        this.Hp = hp;
        this.MaxHp = maxHp;
        Status.hp = hp;
        Status.max_hp = maxHp;
        Debug.Log($"{hp}/{maxHp}");
    }

    public void StopMoving() {
        _EntityWalk.StopMoving();
    }

    private void Awake() {
        Core.NetworkClient.HookPacket(ZC.NOTIFY_ACT3.HEADER, OnEntityAction);
        Core.NetworkClient.HookPacket(ZC.NOTIFY_ACT.HEADER, OnEntityAction);
        Core.NetworkClient.HookPacket(ZC.PAR_CHANGE.HEADER, OnParameterChange);
        Core.NetworkClient.HookPacket(ZC.LONGPAR_CHANGE.HEADER, OnParameterChange);
        Core.NetworkClient.HookPacket(ZC.LONGPAR_CHANGE2.HEADER, OnParameterChange);
        Core.NetworkClient.HookPacket(ZC.COUPLESTATUS.HEADER, OnParameterChange);
    }

    private void OnParameterChange(ushort cmd, int size, InPacket packet) {
        EntityStatus? status = null;
        int value = 0;
        int plusValue = 0;

        if (packet is ZC.PAR_CHANGE PAR_CHANGE) {
            status = PAR_CHANGE.varID;
            value = PAR_CHANGE.value;
        } else if (packet is ZC.LONGPAR_CHANGE LONGPAR_CHANGE) {
            status = LONGPAR_CHANGE.varID;
            value = LONGPAR_CHANGE.value;
        } else if (packet is ZC.LONGPAR_CHANGE2 LONGPAR_CHANGE2) {
            status = LONGPAR_CHANGE2.varID;
            value = LONGPAR_CHANGE2.value;
        } else if (packet is ZC.COUPLESTATUS COUPLESTATUS) {
            status = COUPLESTATUS.status;
            value = COUPLESTATUS.value;
            plusValue = COUPLESTATUS.plusValue;
        }

        if (status == null) {
            return;
        }

        switch (status) {
            case EntityStatus.SP_BASEEXP:
                Status.base_exp = (uint)value;
                break;
            case EntityStatus.SP_JOBEXP:
                Status.job_exp = (uint)value;
                break;
            case EntityStatus.SP_HP:
                Status.hp = value;
                break;
            case EntityStatus.SP_MAXHP:
                Status.max_hp = value;
                break;
            case EntityStatus.SP_SP:
                Status.sp = value;
                break;
            case EntityStatus.SP_MAXSP:
                Status.max_sp = value;
                break;
            case EntityStatus.SP_BASELEVEL:
                Status.base_level = (uint)value;
                break;
            case EntityStatus.SP_JOBLEVEL:
                Status.job_level = (uint)value;
                break;
            case EntityStatus.SP_NEXTBASEEXP:
                Status.next_base_exp = value;
                break;
            case EntityStatus.SP_NEXTJOBEXP:
                Status.next_job_exp = value;
                break;
            default:
                break;
        }

        OnParameterUpdated?.Invoke();
    }

    private void OnEntityAction(ushort cmd, int size, InPacket packet) {
        EntityActionRequest actionRequest;

        if (packet is ZC.NOTIFY_ACT3) {
            var p = packet as ZC.NOTIFY_ACT3;
            actionRequest = p.ActionRequest;
        } else if (packet is ZC.NOTIFY_ACT) {
            var p = packet as ZC.NOTIFY_ACT;
            actionRequest = p.ActionRequest;
        } else {
            return;
        }

        if (actionRequest == null) return;

        var srcEntity = Core.EntityManager.GetEntity(actionRequest.GID);
        var dstEntity = Core.EntityManager.GetEntity(actionRequest.targetGID);

        if (actionRequest.GID == Core.Session.Entity.GID || actionRequest.GID == Core.Session.AccountID) {
            srcEntity = Core.Session.Entity;
        } else if (actionRequest.targetGID == Core.Session.Entity.GID || actionRequest.targetGID == Core.Session.AccountID) {
            dstEntity = Core.Session.Entity;
        }

        // entity out of screen
        if (!srcEntity) {
            return;
        }

        switch (actionRequest.action) {
            // Damage
            case 0:
            case 4:
            case 8:
            case 9:
            case 10:
                OnEntityAttack(actionRequest, srcEntity, dstEntity);
                break;

            // Pickup Item
            case 1:
                OnEntityPickup(srcEntity, dstEntity);
                break;

            // Sit
            case 2:
                break;

            // Stand
            case 3:
                break;
        }
    }

    private static void OnEntityPickup(Entity srcEntity, Entity dstEntity) {
        srcEntity.ChangeMotion(SpriteMotion.PickUp, SpriteMotion.Idle);
        if (dstEntity) {
            srcEntity.LookTo(dstEntity.transform.position);
        }
    }

    private static void OnEntityAttack(EntityActionRequest pkt, Entity srcEntity, Entity dstEntity) {
        Entity target;
        if (dstEntity) {
            // only if damage and do not have endure
            // and damage isn't absorbed (healing)
            if (pkt.damage > 0 && pkt.action != 9 && pkt.action != 4) {
                dstEntity.ChangeMotion(SpriteMotion.Hit, SpriteMotion.Standby);
            }

            target = pkt.damage > 0 ? dstEntity : srcEntity;

            // Process damage
            if (target) {
                switch (pkt.action) {
                    // regular damage (and endure)
                    case 9:
                    case 0:
                        target.Damage(pkt.damage, Core.Tick + pkt.sourceSpeed);
                        break;

                    // double attack
                    case 8:
                        // Display combo only if entity is mob and the attack don't miss
                        if (dstEntity.Type == EntityType.MOB && pkt.damage > 0) {
                            dstEntity.Damage(pkt.damage / 2, Core.Tick + pkt.sourceSpeed * 1, DamageType.COMBO);
                            dstEntity.Damage(pkt.damage, Core.Tick + pkt.sourceSpeed * 2, DamageType.COMBO | DamageType.COMBO_FINAL);
                        }

                        target.Damage(pkt.damage / 2, Core.Tick + pkt.sourceSpeed * 1);
                        target.Damage(pkt.damage / 2, Core.Tick + pkt.sourceSpeed * 2);
                        break;

                    // TODO: critical damage
                    case 10:
                        target.Damage(pkt.damage, Core.Tick + pkt.sourceSpeed);
                        break;

                    // TODO: lucky miss
                    case 11:
                        target.Damage(0, Core.Tick + pkt.sourceSpeed);
                        break;
                }
            }

            srcEntity.LookTo(dstEntity.transform.position);
        }

        srcEntity.AttackSpeed = pkt.sourceSpeed;
        srcEntity.ChangeMotion(SpriteMotion.Attack1, SpriteMotion.Standby, srcEntity.AttackSpeed);
    }

    public void LookTo(Vector3 position) {
        var offset = new Vector2Int((int)position.x, (int)position.z) - new Vector2Int((int)transform.position.x, (int)transform.position.z);
        Direction = PathFindingManager.GetDirectionForOffset(offset);
    }

    /**
     * This method renders the damage sprites
     * The packet to receive damage data and etc is ZC_PAR_CHANGE
     */
    public void Damage(float amount, double tick, DamageType? damageType = null) {
        var DamagePrefab = (GameObject)Resources.Load("Prefabs/Damage");
        if (!DamagePrefab)
            throw new Exception("Could not load damage prefab");

        var damageRenderer = Instantiate(DamagePrefab).GetComponent<DamageRenderer>();
        damageRenderer.Display(amount, tick, damageType, this);
    }
}
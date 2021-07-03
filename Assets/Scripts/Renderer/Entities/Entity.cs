using ROIO.Models.FileTypes;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Entity : MonoBehaviour, INetworkEntity {

    public Action OnParameterUpdated;

    private EntityWalk EntityWalk;
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
    public bool HasAuthority => GID == Session.CurrentSession.Entity?.GetEntityGID();

    [SerializeField] public uint GID;
    [SerializeField] public uint AID;

    public EntityBaseStatus Status = new EntityBaseStatus();
    public EntityEquipInfo EquipInfo;

    public Inventory Inventory = new Inventory();
    public SkillTree SkillTree = new SkillTree();

    private void HookPackets() {
        Core.NetworkClient.HookPacket(ZC.NOTIFY_ACT3.HEADER, OnEntityAction);
        Core.NetworkClient.HookPacket(ZC.NOTIFY_ACT.HEADER, OnEntityAction);
        Core.NetworkClient.HookPacket(ZC.PAR_CHANGE.HEADER, OnParameterChange);
        Core.NetworkClient.HookPacket(ZC.LONGPAR_CHANGE.HEADER, OnParameterChange);
        Core.NetworkClient.HookPacket(ZC.LONGPAR_CHANGE2.HEADER, OnParameterChange);
        Core.NetworkClient.HookPacket(ZC.COUPLESTATUS.HEADER, OnParameterChange);
        Core.NetworkClient.HookPacket(ZC.STATUS.HEADER, OnStatsWindowData);
        Core.NetworkClient.HookPacket(ZC.NOTIFY_EXP2.HEADER, OnExpReceived);
        Core.NetworkClient.HookPacket(ZC.SKILLINFO_LIST.HEADER, OnSkillListReceived);
        Core.NetworkClient.HookPacket(ZC.ATTACK_RANGE.HEADER, OnAttackRangeReceived);
    }

    public void Init(SPR spr, ACT act) {
        EntityViewer.Init(spr, act);
    }

    public void Init(EntitySpawnData data, int rendererLayer) {
        Type = data.objecttype;
        Direction = ((NpcDirection)data.PosDir[2]).ToDirection();

        GID = data.GID;
        AID = data.AID;

        Status.name = data.name;
        Status.jobId = data.job;
        Status.sex = data.sex;
        Status.hair = (short)data.head;
        Status.walkSpeed = data.speed;
        Status.hp = data.HP;
        Status.max_hp = data.MaxHP;
        Status.char_id = GID;
        Status.account_id = AID;

        EquipInfo = new EntityEquipInfo {
            Weapon = (short)data.Weapon,
            Shield = (short)data.Shield,
            HeadTop = (short)data.Accessory2,
            HeadBottom = (short)data.Accessory,
            HeadMid = (short)data.Accessory3,
            Robe = (short)data.Robe
        };

        gameObject.transform.position = new Vector3(data.PosDir[0], Core.PathFinding.GetCellHeight(data.PosDir[0], data.PosDir[1]), data.PosDir[1]);

        SetupViewer(EquipInfo, rendererLayer);
    }

    public void Init(CharacterData data, int rendererLayer) {
        Type = EntityType.PC;
        GID = (uint)data.GID;

        Status.walkSpeed = data.Speed;
        Status.hair = data.Hair;
        Status.base_exp = (uint)data.Exp;
        Status.base_level = (uint)data.BaseLevel;
        Status.job_exp = (uint)data.JobExp;
        Status.job_level = (uint)data.JobLevel;
        Status.sp = data.SP;
        Status.max_sp = data.MaxSP;
        Status.jobId = data.Job;
        Status.sex = (byte)data.Sex;
        Status.hp = data.HP;
        Status.max_hp = data.MaxHP;
        Status.name = data.Name;

        EquipInfo = new EntityEquipInfo {
            Weapon = data.Weapon,
            Shield = data.Shield,
            HeadTop = data.Accessory2,
            HeadBottom = data.Accessory,
            HeadMid = data.Accessory3,
            Robe = (short)data.Robe
        };

        SetupViewer(EquipInfo, rendererLayer);

        HookPackets();
    }

    public void SetReady(bool ready) {
        IsReady = ready;
        EntityWalk = gameObject.AddComponent<EntityWalk>();
    }

    private void SetupViewer(EntityEquipInfo data, int rendererLayer) {
        var body = new GameObject("Body");
        body.layer = rendererLayer;
        body.transform.SetParent(gameObject.transform, false);
        body.transform.localPosition = new Vector3(0.5f, 0.4f, 0.5f);
        body.AddComponent<Billboard>();
        var sortingGroup = body.AddComponent<SortingGroup>();
        sortingGroup.sortingOrder = 2;

        var bodyViewer = body.AddComponent<EntityViewer>();
        bodyViewer.ViewerType = ViewerType.BODY;
        bodyViewer.Entity = this;
        bodyViewer.SpriteOffset = 0.5f;
        bodyViewer.HeadDirection = 0;
        bodyViewer.CurrentMotion = new EntityViewer.MotionRequest { Motion = SpriteMotion.Idle };

        EntityViewer = bodyViewer;
        ShadowSize = 1f;
        // Add more options such as sex etc

        // Any other than PC has Head etc
        if (Type != EntityType.PC) {
            return;
        }

        InitHead(rendererLayer, bodyViewer);
        MaybeInitLayer(rendererLayer, bodyViewer, data.Weapon, ViewerType.WEAPON);
        MaybeInitLayer(rendererLayer, bodyViewer, data.Shield, ViewerType.SHIELD, 1);
        MaybeInitLayer(rendererLayer, bodyViewer, data.HeadTop, ViewerType.HEAD_TOP);
        MaybeInitLayer(rendererLayer, bodyViewer, data.HeadMid, ViewerType.HEAD_MID);
        MaybeInitLayer(rendererLayer, bodyViewer, data.HeadBottom, ViewerType.HEAD_BOTTOM);
    }

    private void InitHead(int rendererLayer, EntityViewer bodyViewer) {
        var head = new GameObject("Head");
        head.layer = rendererLayer;
        head.transform.SetParent(bodyViewer.transform, false);
        head.transform.localPosition = Vector3.zero;
        var headViewer = head.AddComponent<EntityViewer>();
        var sortingGroup = head.AddComponent<SortingGroup>();
        sortingGroup.sortingOrder = 1;

        headViewer.Parent = bodyViewer;
        headViewer.Entity = this;
        headViewer.ViewerType = ViewerType.HEAD;
        bodyViewer.Children.Add(headViewer);
    }

    private void MaybeInitLayer(
        int rendererLayer,
        EntityViewer bodyViewer,
        int viewId,
        ViewerType viewerType,
        int spriteOrder = 2
    ) {
        var viewer = bodyViewer.Children.Find(it => it.ViewerType == viewerType);
        if (viewId != 0 && viewer == null) {
            var layerObject = new GameObject($"{viewerType}");
            layerObject.layer = rendererLayer;
            layerObject.transform.SetParent(bodyViewer.transform, false);
            layerObject.transform.localPosition = Vector3.zero;

            var layerViewer = layerObject.AddComponent<EntityViewer>();
            var sortingGroup = layerObject.AddComponent<SortingGroup>();
            sortingGroup.sortingOrder = spriteOrder;

            layerViewer.Parent = bodyViewer;
            layerViewer.Entity = this;
            layerViewer.ViewerType = viewerType;

            bodyViewer.Children.Add(layerViewer);
        } else if (viewId == 0) {
            viewer?.gameObject.SetActive(false);
        }
    }

    public void Vanish(int type) {
        var id = GID > 0 ? GID : AID;
        switch (type) {
            case 0: // Moved out of sight
                // TODO start coroutine to fade-out entity
                Core.EntityManager.RemoveEntity(id);
                break;
            case 1: // Died
                var isPC = Type == EntityType.PC;
                ChangeMotion(new EntityViewer.MotionRequest { Motion = SpriteMotion.Dead });
                if (!isPC) {
                    StartCoroutine(DestroyAfterSeconds(id));
                }
                break;
            default:
                Core.EntityManager.RemoveEntity(id);
                break;
        }
    }

    private IEnumerator DestroyAfterSeconds(uint id) {
        yield return new WaitForSeconds(1f);
        Core.EntityManager.RemoveEntity(id);
        yield return null;
    }

    internal void OnSpriteChange(int type, short value, short value2) {
        switch (type) {
            case 0:
                Status.jobId = value;
                // update info window
                break;
            default:
                break;
        }

        EntityViewer.Init(reloadSprites: true);
    }

    public void ChangeMotion(EntityViewer.MotionRequest motion, EntityViewer.MotionRequest? nextMotion = null) {
        EntityViewer.ChangeMotion(motion, nextMotion);
    }

    public void UpdateHitPoints(int hp, int maxHp) {
        Status.hp = hp;
        Status.max_hp = maxHp;
    }

    public void StopMoving() {
        EntityWalk.StopMoving();
    }

    private void OnAttackRangeReceived(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.ATTACK_RANGE ATTACK_RANGE && HasAuthority) {
            Status.attackRange = ATTACK_RANGE.Range;
        }
    }

    private void OnSkillListReceived(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.SKILLINFO_LIST SKILLINFO_LIST) {
            SkillTree.Init(Status.jobId, SKILLINFO_LIST.skills);
        }
    }

    private void OnExpReceived(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.NOTIFY_EXP2 NOTIFY_EXP2) {
            switch ((EntityStatus)NOTIFY_EXP2.expType) {
                case EntityStatus.SP_JOBEXP:
                    Status.job_exp += NOTIFY_EXP2.exp;
                    break;
                case EntityStatus.SP_BASEEXP:
                    Status.base_exp += NOTIFY_EXP2.exp;
                    break;
            }
        }

        OnParameterUpdated?.Invoke();
    }

    private void OnStatsWindowData(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.STATUS STATUS) {
            MapUiController.Instance.StatsWindow.UpdateData(STATUS);
        }
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

            case EntityStatus.SP_STR:
            case EntityStatus.SP_AGI:
            case EntityStatus.SP_VIT:
            case EntityStatus.SP_INT:
            case EntityStatus.SP_DEX:
            case EntityStatus.SP_LUK:
                MapUiController.Instance.StatsWindow.UpdateData($"{value} + {plusValue}", status);
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

        if (actionRequest.GID == Session.CurrentSession.Entity.GetEntityGID() || actionRequest.GID == Session.CurrentSession.AccountID) {
            srcEntity = Session.CurrentSession.Entity as Entity;
        } else if (actionRequest.targetGID == Session.CurrentSession.Entity.GetEntityGID() || actionRequest.targetGID == Session.CurrentSession.AccountID) {
            dstEntity = Session.CurrentSession.Entity as Entity;
        }

        // entity out of screen
        if (!srcEntity) {
            return;
        }

        switch (actionRequest.action) {
            // Damage
            case ActionRequestType.ATTACK:
            case ActionRequestType.ATTACK_NOMOTION:
            case ActionRequestType.ATTACK_MULTIPLE:
            case ActionRequestType.ATTACK_MULTIPLE_NOMOTION:
            case ActionRequestType.ATTACK_CRITICAL:
                OnEntityAttack(actionRequest, srcEntity, dstEntity);
                break;

            // Pickup Item
            case ActionRequestType.ITEMPICKUP:
                OnEntityPickup(srcEntity, dstEntity);
                break;

            // Sit
            case ActionRequestType.SIT:
                break;

            // Stand
            case ActionRequestType.STAND:
                break;

            default:
                break;
        }
    }

    private void OnEntityPickup(Entity srcEntity, Entity dstEntity) {
        srcEntity.ChangeMotion(new EntityViewer.MotionRequest { Motion = SpriteMotion.PickUp }, new EntityViewer.MotionRequest { Motion = SpriteMotion.Idle });
        if (dstEntity) {
            srcEntity.LookTo(dstEntity.transform.position);
        }
    }

    private void OnEntityAttack(EntityActionRequest pkt, Entity srcEntity, Entity dstEntity) {
        Entity target;
        if (dstEntity) {
            // only if damage and do not have endure
            // and damage isn't absorbed (healing)
            if (pkt.damage > 0 &&
                pkt.action != ActionRequestType.ATTACK_MULTIPLE_NOMOTION &&
                pkt.action != ActionRequestType.ATTACK_NOMOTION) {
                dstEntity.ChangeMotion(
                    new EntityViewer.MotionRequest { Motion = SpriteMotion.Hit, delay = Core.Tick + pkt.sourceSpeed },
                    new EntityViewer.MotionRequest { Motion = SpriteMotion.Standby, delay = Core.Tick + pkt.sourceSpeed * 2 }
                    );
            }

            target = pkt.damage > 0 ? dstEntity : srcEntity;

            // Process damage
            if (target) {
                switch (pkt.action) {
                    // regular damage (and endure)
                    case ActionRequestType.ATTACK_MULTIPLE_NOMOTION:
                    case ActionRequestType.ATTACK:
                        target.Damage(pkt.damage, Core.Tick + pkt.sourceSpeed);
                        break;

                    // double attack
                    case ActionRequestType.ATTACK_MULTIPLE:
                        // Display combo only if entity is mob and the attack don't miss
                        if (dstEntity.Type == EntityType.MOB && pkt.damage > 0) {
                            dstEntity.Damage(pkt.damage / 2, Core.Tick + pkt.sourceSpeed * 1, DamageType.COMBO);
                            dstEntity.Damage(pkt.damage, Core.Tick + pkt.sourceSpeed * 2, DamageType.COMBO | DamageType.COMBO_FINAL);
                        }

                        target.Damage(pkt.damage / 2, Core.Tick + pkt.sourceSpeed * 1);
                        target.Damage(pkt.damage / 2, Core.Tick + pkt.sourceSpeed * 2);
                        break;

                    // TODO: critical damage
                    case ActionRequestType.ATTACK_CRITICAL:
                        target.Damage(pkt.damage, Core.Tick + pkt.sourceSpeed);
                        break;

                    // TODO: lucky miss
                    case ActionRequestType.ATTACK_LUCKY:
                        target.Damage(0, Core.Tick + pkt.sourceSpeed);
                        break;
                }
            }

            srcEntity.LookTo(dstEntity.transform.position);
        }

        srcEntity.SetAttackSpeed(pkt.sourceSpeed);
        srcEntity.ChangeMotion(
            new EntityViewer.MotionRequest { Motion = SpriteMotion.Attack },
            new EntityViewer.MotionRequest { Motion = SpriteMotion.Standby, delay = Core.Tick + pkt.sourceSpeed }
        );
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

    internal void RequestMove(int x, int y, int dir) {
        EntityWalk.RequestMove(x, y, dir);
    }

    public void StartMoving(int startX, int startY, int endX, int endY) {
        EntityWalk.StartMoving(startX, startY, endX, endY);
    }

    public EntityType GetEntityType() {
        return Type;
    }

    public uint GetEntityGID() {
        return GID;
    }

    public void SetAttackSpeed(ushort speed) {
        Status.attackSpeed = speed;
    }

    public EntityBaseStatus GetBaseStatus() {
        return Status;
    }

    public void UpdateSprites() {
        MaybeInitLayer(gameObject.layer, EntityViewer, EquipInfo.Weapon, ViewerType.WEAPON);
        MaybeInitLayer(gameObject.layer, EntityViewer, EquipInfo.Shield, ViewerType.SHIELD);
        MaybeInitLayer(gameObject.layer, EntityViewer, EquipInfo.HeadTop, ViewerType.HEAD_TOP);
        MaybeInitLayer(gameObject.layer, EntityViewer, EquipInfo.HeadMid, ViewerType.HEAD_MID);
        MaybeInitLayer(gameObject.layer, EntityViewer, EquipInfo.HeadBottom, ViewerType.HEAD_BOTTOM);
        EntityViewer.Init(reloadSprites: true);
    }
}
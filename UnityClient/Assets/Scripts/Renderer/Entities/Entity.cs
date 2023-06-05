using Assets.Scripts.Effects;
using Assets.Scripts.Renderer.Sprite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using UnityRO.Core.GameEntity;
using UnityRO.Net;
using static ZC.NOTIFY_VANISH;

public class Entity : MonoBehaviour, INetworkEntity {

    private GameObject DamagePrefab;
    private EntityWalk EntityWalk;
    private AudioSource AudioSource;

    public FramePaceAlgorithm CurrentFramePaceAlgorithm = FramePaceAlgorithm.UnityRO;
    public EntityType Type = EntityType.UNKNOWN;
    public GameEntityViewerType EntityViewerType = GameEntityViewerType.SPRITE;

    public Action OnParameterUpdated;
    public Action AfterMoveAction;
    public GameEntityViewer EntityViewer;
    public Direction Direction = 0;
    public SortingGroup SortingGroup;
    public float ShadowSize;
    public int Action = 0;
    public int HeadDir;

    public bool IsReady = false;
    public bool HasAuthority => GID == SessionManager.CurrentSession.Entity?.GetEntityGID();

    public uint GID;
    public uint AID;
    public EntityBaseStatus Status = new EntityBaseStatus();
    public EntityEquipInfo EquipInfo;
    public Inventory Inventory = new Inventory();
    public SkillTree SkillTree = new SkillTree();

    private GameManager GameManager;
    private NetworkClient NetworkClient;
    private EntityManager EntityManager;
    private PathFinder PathFinder;
    private SessionManager SessionManager;
    
    private EntityCanvas Canvas;
    private Camera MainCamera;
    private LayerMask EntityMask;
    private List<DamageRenderer> DamageNumbers;

    private void Awake() {
        DamagePrefab = (GameObject) Resources.Load("Prefabs/Damage");
        NetworkClient = FindObjectOfType<NetworkClient>();
        EntityManager = FindObjectOfType<EntityManager>();
        PathFinder = FindObjectOfType<PathFinder>();
        GameManager = FindObjectOfType<GameManager>();
        SessionManager = FindObjectOfType<SessionManager>();
        
        EntityMask = LayerMask.GetMask("NPC", "Monsters", "Characters");
        DamageNumbers = new List<DamageRenderer>();

        if (AudioSource == null) {
            AudioSource = gameObject.AddComponent<AudioSource>();
            AudioSource.spatialBlend = 0.7f;
            AudioSource.priority = 60;
            AudioSource.maxDistance = 60;
            AudioSource.rolloffMode = AudioRolloffMode.Linear;
            AudioSource.volume = 1f;
            AudioSource.dopplerLevel = 0;
            AudioSource.outputAudioMixerGroup = GameManager.EffectsMixerGroup;
        }
    }

    internal void DisplayChatBubble(string message) {
        Canvas?.SetEntityMessage(message);
    }

    private void Update() {
        if (MainCamera == null) {
            MainCamera = Camera.main;
        }

        CheckForMouseOver();
    }

    private void CheckForMouseOver() {
        var ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        var didHitAnyEntity = Physics.Raycast(ray, out var entityHit, 150, EntityMask);

        if (entityHit.collider == null) {
            Canvas?.HideEntityName();
            return;
        }
        entityHit.collider.gameObject.TryGetComponent<SpriteEntityViewer>(out var target);

        if (didHitAnyEntity && target != null && target.Entity == this) {
            Canvas?.ShowEntityName();
        } else {
            Canvas?.HideEntityName();
        }
    }

    private void HookPackets() {
        NetworkClient.HookPacket<ZC.NOTIFY_ACT3>(ZC.NOTIFY_ACT3.HEADER, OnEntityAction);
        NetworkClient.HookPacket<ZC.NOTIFY_ACT>(ZC.NOTIFY_ACT.HEADER, OnEntityAction);
        NetworkClient.HookPacket<ZC.PAR_CHANGE>(ZC.PAR_CHANGE.HEADER, OnParameterChange);
        NetworkClient.HookPacket<ZC.LONGPAR_CHANGE>(ZC.LONGPAR_CHANGE.HEADER, OnParameterChange);
        NetworkClient.HookPacket<ZC.LONGPAR_CHANGE2>(ZC.LONGPAR_CHANGE2.HEADER, OnParameterChange);
        NetworkClient.HookPacket<ZC.COUPLESTATUS>(ZC.COUPLESTATUS.HEADER, OnParameterChange);
        NetworkClient.HookPacket<ZC.STATUS>(ZC.STATUS.HEADER, OnStatsWindowData);
        NetworkClient.HookPacket<ZC.NOTIFY_EXP2>(ZC.NOTIFY_EXP2.HEADER, OnExpReceived);
        NetworkClient.HookPacket<ZC.SKILLINFO_LIST>(ZC.SKILLINFO_LIST.HEADER, OnSkillsUpdated);
        NetworkClient.HookPacket<ZC.SKILLINFO_UPDATE>(ZC.SKILLINFO_UPDATE.HEADER, OnSkillsUpdated);
        NetworkClient.HookPacket<ZC.ATTACK_RANGE>(ZC.ATTACK_RANGE.HEADER, OnAttackRangeReceived);
        NetworkClient.HookPacket<ZC.ACK_TOUSESKILL>(ZC.ACK_TOUSESKILL.HEADER, OnUseSkillResult);
        NetworkClient.HookPacket<ZC.NOTIFY_SKILL2>(ZC.NOTIFY_SKILL2.HEADER, OnEntityUseSkillToAttack);
        NetworkClient.HookPacket<ZC.USESKILL_ACK2>(ZC.USESKILL_ACK2.HEADER, OnEntityCastSkill);
        NetworkClient.HookPacket<ZC.ATTACK_FAILURE_FOR_DISTANCE>(ZC.ATTACK_FAILURE_FOR_DISTANCE.HEADER, OnAttackFailureForDistance);
    }

    public void Init(SpriteData spriteData, Texture2D atlas) {
        EntityViewer.Init(spriteData, atlas);
    }

    public void Init(EntitySpawnData data, int rendererLayer, EntityCanvas canvas) {
        Canvas = canvas;
        Type = data.job == 45 ? EntityType.WARP : (EntityType)data.objecttype;
        Direction = ((NpcDirection) data.PosDir[2]).ToDirection();

        GID = data.GID;
        AID = data.AID;

        Status.name = data.name;
        Status.jobId = data.job;
        Status.sex = data.sex;
        Status.hair = (short) data.head;
        Status.walkSpeed = data.speed;
        Status.hp = data.HP;
        Status.max_hp = data.MaxHP;
        Status.char_id = GID;
        Status.account_id = AID;

        Status.hair_color = data.HairColor;
        Status.clothes_color = data.ClothesColor;

        EquipInfo = new EntityEquipInfo {
            Weapon = (short) data.Weapon,
            Shield = (short) data.Shield,
            HeadTop = (short) data.Accessory2,
            HeadBottom = (short) data.Accessory,
            HeadMid = (short) data.Accessory3,
            Robe = (short) data.Robe
        };

        gameObject.transform.position = new Vector3(data.PosDir[0], PathFinder?.GetCellHeight(data.PosDir[0], data.PosDir[1]) ?? 0f, data.PosDir[1]);

        SetupViewer(EquipInfo, rendererLayer);

        SetReady(true);

        switch (data.state) {
            case EntitySpawnData.EntitySpawnState.Stand:
                break;
            case EntitySpawnData.EntitySpawnState.Sit:
                ChangeMotion(new MotionRequest { Motion = SpriteMotion.Sit });
                break;
            case EntitySpawnData.EntitySpawnState.Dead:
                ChangeMotion(new MotionRequest { Motion = SpriteMotion.Dead });
                break;
            default:
                break;
        }
    }

    public void Init(CharacterData data, int rendererLayer, EntityCanvas canvas, bool isFromCharacterSelection = false) {
        Canvas = canvas;
        Type = EntityType.PC;

        GID = (uint) data.GID;
        Status.base_exp = (uint) data.Exp;
        Status.zeny = data.Money;
        Status.job_exp = (uint) data.JobExp;
        Status.job_level = (uint) data.JobLevel;
        //body state?
        //health state?
        //option?
        //karma?
        //manner
        Status.StatusPoints = (uint) data.JobPoint;

        Status.hp = data.HP;
        Status.max_hp = data.MaxHP;
        Status.sp = data.SP;
        Status.max_sp = data.MaxSP;
        Status.walkSpeed = data.Speed;
        Status.jobId = data.Job;
        Status.hair = data.Head;
        Status.base_level = (uint) data.Level;
        Status.SkillPoints = (uint) data.SPPoint;

        Status.hair_color = data.HeadPalette;
        Status.clothes_color = data.BodyPalette;

        Status.name = data.Name;
        Status.str = data.Str;
        Status.agi = data.Agi;
        Status.vit = data.Vit;
        Status.int_ = data.Int;
        Status.dex = data.Dex;
        Status.luk = data.Luk;

        Status.sex = (byte) data.Sex;

        EquipInfo = new EntityEquipInfo {
            Weapon = (short) data.Weapon,
            Shield = (short) data.Shield,
            HeadTop = (short) data.Accessory2,
            HeadBottom = (short) data.Accessory,
            HeadMid = (short) data.Accessory3,
            Robe = (short) data.chr_slot_changeCnt
        };

        SetupViewer(EquipInfo, rendererLayer, isFromCharacterSelection);

        if (isFromCharacterSelection) {
            ShadowSize = 0f;
            return;
        }

        HookPackets();
    }

    public void Clone(Entity entity, int rendererLayer, bool isFromUi = false) {
        Status = entity.Status;
        Type = entity.Type;
        EquipInfo = entity.EquipInfo;

        SetupViewer(EquipInfo, rendererLayer, isFromUi);

        if (isFromUi) {
            ShadowSize = 0f;
            return;
        }
    }

    public void SetReady(bool ready, bool isFromCharacterSelection = false) {
        IsReady = ready;

        if (isFromCharacterSelection)
            return;

        EntityWalk = gameObject.AddComponent<EntityWalk>();
        SetupCanvas();
    }

    private void SetupViewer(EntityEquipInfo data, int rendererLayer, bool isFromUi = false) {
        if (EntityViewer != null) {
            Destroy(EntityViewer.gameObject);
        }

        if (EntityViewerType == GameEntityViewerType.SPRITE) {
            InitSpriteViewer(data, rendererLayer, isFromUi);
        } else if (EntityViewerType == GameEntityViewerType.MESH) {
            InitMeshViewer(data, rendererLayer, isFromUi);
        }
    }

    private void InitMeshViewer(EntityEquipInfo data, int rendererLayer, bool isFromUi = false) {
        var body = new GameObject("Body");
        body.layer = rendererLayer;
        body.transform.SetParent(gameObject.transform, false);
        body.transform.localPosition = Vector3.zero;

        var bodyViewer = body.AddComponent<MeshEntityViewer>();
        bodyViewer.ViewerType = ViewerType.MESH;
        bodyViewer.Entity = this;

        EntityViewer = bodyViewer;
    }

    private void InitSpriteViewer(EntityEquipInfo data, int rendererLayer, bool isFromUi = false) {
        var body = new GameObject("Body");
        body.layer = rendererLayer;
        body.transform.SetParent(gameObject.transform, false);
        body.transform.localPosition = new Vector3(0f, 0.4f, 0f);
        SortingGroup = body.AddComponent<SortingGroup>();
        SortingGroup.sortingOrder = 2;

        var bodyViewer = body.AddComponent<SpriteEntityViewer>();
        bodyViewer.ViewerType = ViewerType.BODY;
        bodyViewer.Entity = this;
        bodyViewer.HeadDirection = 0;

        EntityViewer = bodyViewer;
        ShadowSize = 1f;
        // Add more options such as sex etc

        if (Type == EntityType.WARP) {
            body.transform.localPosition = new Vector3(0.5f, 0f, 0.5f);
            var warp = body.AddComponent<MapWarpEffect>();
            warp.StartWarp(body);
            return;
        }

        if (!isFromUi) {
            body.AddComponent<Billboard>();
        }
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

    private void SetupCanvas() {
        Canvas.Init(this);
        Canvas.SetEntityName(Status.name);
        Canvas.SetEntityHP(Status.hp, Status.max_hp);
        Canvas.SetEntitySP(Status.sp, Status.max_sp);

        if (HasAuthority) {
            Canvas.ShowEntityHP();
            Canvas.ShowEntitySP();
        } else if (Type == EntityType.MOB && Status.hp != Status.max_hp) {
            Canvas.ShowEntityHP();
        }
    }

    private void InitHead(int rendererLayer, SpriteEntityViewer bodyViewer) {
        var head = new GameObject("Head");
        head.layer = rendererLayer;
        head.transform.SetParent(bodyViewer.transform, false);
        head.transform.localPosition = Vector3.zero;
        var headViewer = head.AddComponent<SpriteEntityViewer>();
        var sortingGroup = head.AddComponent<SortingGroup>();
        sortingGroup.sortingOrder = 1;

        headViewer.Parent = bodyViewer;
        headViewer.Entity = this;
        headViewer.ViewerType = ViewerType.HEAD;
        bodyViewer.Children.Add(headViewer);
    }

    private void MaybeInitLayer(
        int rendererLayer,
        SpriteEntityViewer bodyViewer,
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

            var layerViewer = layerObject.AddComponent<SpriteEntityViewer>();
            var sortingGroup = layerObject.AddComponent<SortingGroup>();
            sortingGroup.sortingOrder = spriteOrder;

            layerViewer.Parent = bodyViewer;
            layerViewer.Entity = this;
            layerViewer.ViewerType = viewerType;

            bodyViewer.Children.Add(layerViewer);
        } else if (viewer != null) {
            viewer.gameObject.SetActive(true);
        } else if (viewId == 0) {
            viewer?.gameObject.SetActive(false);
        }
    }

    /**
     * Investigate this because some of the packets to spawn
     * entities doesnt seem to send the name and since we're destroying them
     * everything gets borked...
     * Check roBrowser logic or rA guys
     */
    public void Vanish(VanishType type) {
        StopMoving();
        switch (type) {
            case VanishType.OUT_OF_SIGHT:
                StartCoroutine(DestroyWithFade());
                break;
            case VanishType.DIED:
                var isPC = Type == EntityType.PC;
                ChangeMotion(new MotionRequest { Motion = SpriteMotion.Dead });
                if (!isPC) {
                    StartCoroutine(DestroyAfterSecondsWithFade());
                }
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }

    private IEnumerator DestroyAfterSecondsWithFade() {
        yield return new WaitForSeconds(2.5f);
        IsReady = false;
        yield return EntityViewer.FadeOut();
        Destroy(gameObject);
        yield return null;
    }

    private IEnumerator DestroyWithFade() {
        IsReady = false;
        yield return EntityViewer.FadeOut();
        Destroy(gameObject);
    }

    internal void OnSpriteChange(ZC.SPRITE_CHANGE2.LookType type, short value, short value2) {
        switch (type) {
            case ZC.SPRITE_CHANGE2.LookType.LOOK_BASE:
                Status.jobId = value;
                // update info window
                break;
            case ZC.SPRITE_CHANGE2.LookType.LOOK_WEAPON:
                throw new NotImplementedException();
            default:
                break;
        }

        EntityViewer.Init(reloadSprites: true);
    }

    public void ChangeMotion(MotionRequest motion, MotionRequest? nextMotion = null) {
        EntityViewer.ChangeMotion(motion, nextMotion);
    }

    public void UpdateHitPoints(int hp, int maxHp) {
        Status.hp = hp;
        Status.max_hp = maxHp;

        if (Type == EntityType.MOB && Status.hp != Status.max_hp) {
            Canvas.ShowEntityHP();
            Canvas.SetEntityHP(Status.hp, Status.max_hp);
        }
    }

    public void StopMoving() {
        EntityWalk.StopMoving();
    }

    private void OnAttackFailureForDistance(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.ATTACK_FAILURE_FOR_DISTANCE ATTACK_FAILURE_FOR_DISTANCE) {
            RequestMove(ATTACK_FAILURE_FOR_DISTANCE.targetXPos, ATTACK_FAILURE_FOR_DISTANCE.targetYPos, 0);
        }
    }

    private void OnEntityCastSkill(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.USESKILL_ACK2 USESKILL_ACK2) {
            var srcEntity = EntityManager.GetEntity(USESKILL_ACK2.AID);
            var dstEntity = EntityManager.GetEntity(USESKILL_ACK2.targetID);

            if (!srcEntity) {
                return;
            }

            if (USESKILL_ACK2.delayTime > 0) {
                srcEntity.CastSkill(USESKILL_ACK2.delayTime / 1000f, USESKILL_ACK2.property);
            }
        }
    }

    public void CastSkill(float delayTime, uint property) {
        PlayAudio("data/wav/effect/ef_beginspell.wav");
        CastingEffect.StartCasting(delayTime, "data/texture/effect/ring_yellow.png", gameObject);
        ChangeMotion(new MotionRequest { Motion = SpriteMotion.Casting, delay = 0 });
    }

    public void PlayAudio(string path) {
        var clip = Addressables.LoadAssetAsync<AudioClip>(path.SanitizeForAddressables()).WaitForCompletion();

        if (clip != null && AudioSource != null) {
            AudioSource.clip = clip;
            AudioSource.Play();
        }
    }

    private void OnEntityUseSkillToAttack(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.NOTIFY_SKILL2 NOTIFY_SKILL2) {
            var srcEntity = EntityManager.GetEntity(NOTIFY_SKILL2.AID);
            var dstEntity = EntityManager.GetEntity(NOTIFY_SKILL2.targetID);

            if (NOTIFY_SKILL2.AID == SessionManager.CurrentSession.Entity.GetEntityGID() || NOTIFY_SKILL2.AID == SessionManager.CurrentSession.AccountID) {
                srcEntity = SessionManager.CurrentSession.Entity as Entity;
            } else if (NOTIFY_SKILL2.targetID == SessionManager.CurrentSession.Entity.GetEntityGID() || NOTIFY_SKILL2.targetID == SessionManager.CurrentSession.AccountID) {
                dstEntity = SessionManager.CurrentSession.Entity as Entity;
            }

            if (srcEntity != null) {
                NOTIFY_SKILL2.attackMT = Math.Min(450, NOTIFY_SKILL2.attackMT);
                NOTIFY_SKILL2.attackMT = Math.Max(1, NOTIFY_SKILL2.attackMT);
                srcEntity.SetAttackSpeed((ushort) NOTIFY_SKILL2.attackMT);

                if (srcEntity.Type != EntityType.MOB) {
                    // SET DIALOG BOX
                    // srcEntity.dialog.set( ( (SkillInfo[pkt.SKID] && SkillInfo[pkt.SKID].SkillName ) || 'Unknown Skill' ) + ' !!' );
                }

                srcEntity.ChangeMotion(
                    new MotionRequest {
                        Motion = SpriteMotion.Attack2,
                        delay = 0
                    },
                    new MotionRequest {
                        Motion = SpriteMotion.Idle,
                        delay = 0
                    }
                );
                srcEntity.SetAttackSpeed((ushort) NOTIFY_SKILL2.attackMT);
            }

            if (dstEntity != null) {
                Entity target = NOTIFY_SKILL2.damage > 0 ? dstEntity : srcEntity;
                int i;

                if (NOTIFY_SKILL2.damage > 0 && target) {
                    for (i = 0; i < NOTIFY_SKILL2.count; i++) {
                        StartCoroutine(AddDamageFromSkill(dstEntity, target, NOTIFY_SKILL2, i, NOTIFY_SKILL2.attackMT + (200 * i)));
                    }
                }
            }

            if (srcEntity && dstEntity) {
                //EffectManager.spamSkill(pkt.SKID, dstEntity.GID, null, Renderer.tick + pkt.attackMT);
            }
        }
    }

    private IEnumerator AddDamageFromSkill(Entity dstEntity, Entity target, ZC.NOTIFY_SKILL2 NOTIFY_SKILL2, int k, float delay) {
        yield return new WaitForSeconds(delay / 1000);

        var isAlive = dstEntity.Action != AnimationHelper.GetMotionIdForSprite(Type, SpriteMotion.Dead);
        var isCombo = target.Type != EntityType.PC && NOTIFY_SKILL2.count > 1;

        //EffectManager.spamSkillHit( pkt.SKID, dstEntity.GID, Renderer.tick);
        dstEntity.Damage(NOTIFY_SKILL2.damage / NOTIFY_SKILL2.count, GameManager.Tick);

        // Only display combo if the target is not entity and
        // there are multiple attacks
        if (isCombo) {
            dstEntity.Damage(
                NOTIFY_SKILL2.damage * (k + 1),
                GameManager.Tick,
                DamageType.COMBO | (k + 1 == NOTIFY_SKILL2.count ? DamageType.COMBO_FINAL : 0)
            );
        }

        if (isAlive) {
            dstEntity.ChangeMotion(
                new MotionRequest { Motion = SpriteMotion.Hit, delay = 0 },
                new MotionRequest { Motion = SpriteMotion.Standby, delay = 0 }
            );
        }
    }

    private void OnAttackRangeReceived(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.ATTACK_RANGE ATTACK_RANGE && HasAuthority) {
            Status.attackRange = ATTACK_RANGE.Range;
        }
    }

    private void OnSkillsUpdated(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.SKILLINFO_LIST SKILLINFO_LIST) {
            SkillTree.Init(Status.jobId, SKILLINFO_LIST.skills);
        } else if (packet is ZC.SKILLINFO_UPDATE SKILLINFO_UPDATE) {
            SkillTree.UpdateSkill(SKILLINFO_UPDATE.SkillInfo);
        }
    }

    private void OnExpReceived(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.NOTIFY_EXP2 NOTIFY_EXP2) {
            switch ((EntityStatus) NOTIFY_EXP2.expType) {
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
                Status.base_exp = (uint) value;
                break;
            case EntityStatus.SP_JOBEXP:
                Status.job_exp = (uint) value;
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
                Status.base_level = (uint) value;
                break;
            case EntityStatus.SP_JOBLEVEL:
                Status.job_level = (uint) value;
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

            case EntityStatus.SP_SPEED:
                Status.walkSpeed = (short) value;
                break;

            case EntityStatus.SP_STATUSPOINT:
                Status.StatusPoints = (uint) value;
                break;

            case EntityStatus.SP_SKILLPOINT:
                Status.SkillPoints = (uint) value;
                MapUiController.Instance.SkillWindow.UpdateSkillPoints();
                break;

            case EntityStatus.SP_ZENY:
                Status.zeny = value;
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

        if (actionRequest == null)
            return;

        var srcEntity = EntityManager.GetEntity(actionRequest.AID);
        var dstEntity = actionRequest.action != ActionRequestType.STAND && actionRequest.action != ActionRequestType.SIT ? EntityManager.GetEntity(actionRequest.targetAID) : null;

        if (actionRequest.AID == SessionManager.CurrentSession.Entity.GetEntityGID() || actionRequest.AID == SessionManager.CurrentSession.AccountID) {
            srcEntity = SessionManager.CurrentSession.Entity as Entity;
        } else if (actionRequest.targetAID == SessionManager.CurrentSession.Entity.GetEntityGID() || actionRequest.targetAID == SessionManager.CurrentSession.AccountID) {
            dstEntity = SessionManager.CurrentSession.Entity as Entity;
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
                OnEntitySit(srcEntity);
                break;

            // Stand
            case ActionRequestType.STAND:
                OnEntityStand(srcEntity);
                break;

            default:
                break;
        }
    }

    private void OnEntityStand(Entity srcEntity) {
        srcEntity.ChangeMotion(new MotionRequest { Motion = SpriteMotion.Idle });
    }

    private void OnEntitySit(Entity srcEntity) {
        srcEntity.ChangeMotion(new MotionRequest { Motion = SpriteMotion.Sit });
    }

    private void OnEntityPickup(Entity srcEntity, Entity dstEntity) {
        srcEntity.ChangeMotion(new MotionRequest { Motion = SpriteMotion.PickUp }, new MotionRequest { Motion = SpriteMotion.Idle });
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
                    new MotionRequest { Motion = SpriteMotion.Hit, delay = GameManager.Tick + pkt.sourceSpeed },
                    new MotionRequest { Motion = SpriteMotion.Standby, delay = GameManager.Tick + pkt.sourceSpeed * 2 }
                    );
            }

            target = pkt.damage > 0 ? dstEntity : srcEntity;

            // Process damage
            if (target) {
                switch (pkt.action) {
                    // regular damage (and endure)
                    case ActionRequestType.ATTACK_MULTIPLE_NOMOTION:
                    case ActionRequestType.ATTACK:
                        target.Damage(pkt.damage, GameManager.Tick + pkt.sourceSpeed);
                        break;

                    // double attack
                    case ActionRequestType.ATTACK_MULTIPLE:
                        // Display combo only if entity is mob and the attack don't miss
                        if (dstEntity.Type == EntityType.MOB && pkt.damage > 0) {
                            dstEntity.Damage(pkt.damage / 2, GameManager.Tick + pkt.sourceSpeed * 1, DamageType.COMBO);
                            dstEntity.Damage(pkt.damage, GameManager.Tick + pkt.sourceSpeed * 2, DamageType.COMBO | DamageType.COMBO_FINAL);
                        }

                        target.Damage(pkt.damage / 2, GameManager.Tick + pkt.sourceSpeed * 1);
                        target.Damage(pkt.damage / 2, GameManager.Tick + pkt.sourceSpeed * 2);
                        break;

                    // TODO: critical damage
                    case ActionRequestType.ATTACK_CRITICAL:
                        target.Damage(pkt.damage, GameManager.Tick + pkt.sourceSpeed);
                        break;

                    // TODO: lucky miss
                    case ActionRequestType.ATTACK_LUCKY:
                        target.Damage(0, GameManager.Tick + pkt.sourceSpeed);
                        break;
                }
            }

            srcEntity.LookTo(dstEntity.transform.position);
        }

        srcEntity.SetAttackSpeed(pkt.sourceSpeed);
        srcEntity.ChangeMotion(
            new MotionRequest { Motion = SpriteMotion.Attack },
            new MotionRequest { Motion = SpriteMotion.Standby, delay = GameManager.Tick + pkt.sourceSpeed }
        );
        srcEntity.SetAttackSpeed(pkt.sourceSpeed);
    }

    private void OnUseSkillResult(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.ACK_TOUSESKILL TOUSESKILL) {
            // success
            if (TOUSESKILL.Flag > 0) {
                return;
            }
        }
    }

    public void LookTo(Vector3 position) {
        var offset = new Vector2Int((int) position.x, (int) position.z) - new Vector2Int((int) transform.position.x, (int) transform.position.z);
        Direction = PathFinder.GetDirectionForOffset(offset);
    }

    /**
     * This method renders the damage sprites
     * The packet to receive damage data and etc is ZC_PAR_CHANGE
     */
    public void Damage(float amount, double tick, DamageType? damageType = null) {
        var damageRenderer = Instantiate(DamagePrefab).GetComponent<DamageRenderer>();
        var delay = damageRenderer.Display(amount, tick, damageType, this);

        if ((damageType & DamageType.COMBO) > 0) {
            var combos = DamageNumbers.FindAll(it => (it.CurrentType & DamageType.COMBO) > 0);
            combos.ForEach(it => Destroy(it.gameObject));
            DamageNumbers.RemoveAll(it => (it.CurrentType & DamageType.COMBO) > 0);
        }

        DamageNumbers.Add(damageRenderer);
        StartCoroutine(DestroyDamageRendererAfterDelay(damageRenderer, delay));
    }

    private IEnumerator DestroyDamageRendererAfterDelay(DamageRenderer renderer, float delay) {
        yield return new WaitForSeconds(delay);
        if (DamageNumbers.Contains(renderer)) {
            Destroy(renderer.gameObject);
            DamageNumbers.Remove(renderer);
        }
        yield return null;
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

    int INetworkEntity.GetEntityGID() {
        throw new NotImplementedException();
    }

    public int GetEntityAID() {
        throw new NotImplementedException();
    }

    public string GetEntityName() {
        throw new NotImplementedException();
    }

    int INetworkEntity.GetEntityType() {
        throw new NotImplementedException();
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
        if (!IsReady) {
            return;
        }

        if (EntityViewer is SpriteEntityViewer SpriteViewer) {
            MaybeInitLayer(gameObject.layer, SpriteViewer, EquipInfo.Weapon, ViewerType.WEAPON);
            MaybeInitLayer(gameObject.layer, SpriteViewer, EquipInfo.Shield, ViewerType.SHIELD);
            MaybeInitLayer(gameObject.layer, SpriteViewer, EquipInfo.HeadTop, ViewerType.HEAD_TOP);
            MaybeInitLayer(gameObject.layer, SpriteViewer, EquipInfo.HeadMid, ViewerType.HEAD_MID);
            MaybeInitLayer(gameObject.layer, SpriteViewer, EquipInfo.HeadBottom, ViewerType.HEAD_BOTTOM);
        }
        EntityViewer.Init(reloadSprites: true);
    }

    public enum FramePaceAlgorithm {
        RoBrowser, UnityRO, DHXJ
    }
}
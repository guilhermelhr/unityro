using ROIO;
using ROIO.Models.FileTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityRO.GameCamera;

public class EntityViewer : MonoBehaviour {

    private const int AVERAGE_ATTACK_SPEED = 432;
    private const int MAX_ATTACK_SPEED = AVERAGE_ATTACKED_SPEED * 2;
    private const int AVERAGE_ATTACKED_SPEED = 288;

    public Entity Entity;
    public EntityViewer Parent;
    public ViewerType ViewerType;

    public MotionRequest CurrentMotion;
    public MotionRequest? NextMotion;

    public float SpriteOffset;
    public int HeadDirection;
    public SpriteState State = SpriteState.Idle;

    public List<EntityViewer> Children = new List<EntityViewer>();
    private Dictionary<int, SpriteRenderer> Layers = new Dictionary<int, SpriteRenderer>();
    private Dictionary<ACT.Frame, Mesh> MeshCache = new Dictionary<ACT.Frame, Mesh>();

    public Sprite[] sprites { get; private set; }
    public ACT currentACT { get; private set; }
    public SPR currentSPR { get; private set; }
    public ACT.Action currentAction { get; private set; }
    public int ActionId { get; private set; } = -1;

    private int currentActionIndex;
    private int currentViewID;
    private int currentFrame = 0;
    private long AnimationStart;
    private double previousFrame = 0;

    private double motionSpeed = 4;
    private bool isAnimationFinished;
    private double loopCountToAnimationFinish = 1;
    private float motionSpeedMultiplier = 1f;

    private MeshCollider meshCollider;
    private Material SpriteMaterial;

    public void Init(SPR spr, ACT act) {
        currentSPR = spr;
        currentACT = act;

        currentSPR.SwitchToRGBA();
        sprites = currentSPR.GetSprites();
    }

    public void Start() {
        SpriteMaterial = Resources.Load("Materials/Sprites/SpriteMaterial") as Material;
        Init();

        InitShadow();
    }

    public void Init(bool reloadSprites = false) {
        meshCollider = gameObject.GetOrAddComponent<MeshCollider>();

        if (Entity.Type == EntityType.WARP) {
            return;
        }

        if (currentSPR == null || reloadSprites) {
            string path = "";

            switch (ViewerType) {
                case ViewerType.BODY:
                    path = DBManager.GetBodyPath(Entity.Status.jobId, Entity.Status.sex);
                    break;
                case ViewerType.HEAD:
                    path = DBManager.GetHeadPath(Entity.Status.jobId, Entity.Status.hair, Entity.Status.sex);
                    break;
                case ViewerType.HAND_RIGHT:
                    currentViewID = Entity.EquipInfo.RightHand.ViewID;
                    path = DBManager.GetWeaponPath(currentViewID, Entity.Status.jobId, Entity.Status.sex);
                    break;
                case ViewerType.HAND_LEFT:
                    currentViewID = Entity.EquipInfo.LeftHand.ViewID;

                    if (EquipmentLocation.HAND_LEFT.HasFlag(Entity.EquipInfo.LeftHand.Location) && !EquipmentLocation.HAND_RIGHT.HasFlag(Entity.EquipInfo.RightHand.Location)) { //shield only
                        path = DBManager.GetShieldPath(currentViewID, Entity.Status.jobId, Entity.Status.sex);
                    } else {
                        path = DBManager.GetWeaponPath(currentViewID, Entity.Status.jobId, Entity.Status.sex);
                    }

                    break;
                case ViewerType.HEAD_TOP:
                    currentViewID = Entity.EquipInfo.HeadTop.ViewID;
                    path = DBManager.GetHatPath(currentViewID, Entity.Status.sex);
                    break;
                case ViewerType.HEAD_MID:
                    currentViewID = Entity.EquipInfo.HeadMid.ViewID;
                    path = DBManager.GetHatPath(currentViewID, Entity.Status.sex);
                    break;
                case ViewerType.HEAD_BOTTOM:
                    currentViewID = Entity.EquipInfo.HeadBottom.ViewID;
                    path = DBManager.GetHatPath(currentViewID, Entity.Status.sex);
                    break;
            }

            if (ViewerType != ViewerType.BODY && ViewerType != ViewerType.HEAD && currentViewID <= 0) {
                currentACT = null;
                currentSPR = null;
                Layers.Values.ToList().ForEach(Renderer => {
                    Destroy(Renderer.gameObject);
                });
                Layers.Clear();
                MeshCache.Clear();

                return;
            }

            try {
                currentSPR = FileManager.Load(path + ".spr") as SPR;
                currentACT = FileManager.Load(path + ".act") as ACT;

                currentSPR.SwitchToRGBA();
                sprites = currentSPR.GetSprites();
            } catch {
                Debug.LogError($"Could not load sprites for: {path}");
                currentACT = null;
                currentSPR = null;
            }
        }

        if (ActionId == -1) {
            ChangeMotion(new MotionRequest { Motion = SpriteMotion.Idle });
        }

        foreach (var child in Children) {
            child.Init(reloadSprites);
            child.Start();
        }
    }

    void FixedUpdate() {
        if (!Entity.IsReady || currentACT == null)
            return;

        if (ViewerType != ViewerType.BODY && ViewerType != ViewerType.HEAD) {
            var updatedViewID = FindCurrentViewID();
            if (updatedViewID != currentViewID) {
                Init(reloadSprites: true);

                return;
            }
        }

        if (isAnimationFinished && NextMotion.HasValue) {
            ChangeMotion(NextMotion.Value);
        }

        var cameraDirection = (int) (CharacterCamera.ROCamera?.Direction ?? 0);
        var entityDirection = (int) Entity.Direction + 8;
        currentActionIndex = (ActionId + (cameraDirection + entityDirection) % 8) % currentACT.actions.Length;
        currentAction = currentACT.actions[currentActionIndex];
        currentFrame = GetCurrentFrame(GameManager.Tick - AnimationStart);
        var frame = currentAction.frames[currentFrame];

        UpdateMesh(frame);
        RenderLayers(frame);
        PlaySound(frame);
        UpdateAnchorPoints();
    }

    private int FindCurrentViewID() {
        switch (ViewerType) {
            case ViewerType.HAND_RIGHT:
                return Entity.EquipInfo.RightHand.ViewID;
            case ViewerType.HAND_LEFT:
                return Entity.EquipInfo.LeftHand.ViewID;
            case ViewerType.HEAD_TOP:
                return Entity.EquipInfo.HeadTop.ViewID;
            case ViewerType.HEAD_MID:
                return Entity.EquipInfo.HeadMid.ViewID;
            case ViewerType.HEAD_BOTTOM:
                return Entity.EquipInfo.HeadBottom.ViewID;
            default:
                return -1;
        }
    }

    private void UpdateAnchorPoints() {
        if (Parent != null && ViewerType != ViewerType.HAND_RIGHT) {
            var parentAnchor = Parent.GetAnimationAnchor();
            var ourAnchor = GetAnimationAnchor();

            var diff = parentAnchor - ourAnchor;

            transform.localPosition = new Vector3(diff.x, -diff.y, 0f) / SPR.PIXELS_PER_UNIT;
        }
    }

    private void PlaySound(ACT.Frame frame) {
        if (frame.soundId > -1 && frame.soundId < currentACT.sounds.Length) {
            var clipName = currentACT.sounds[frame.soundId];
            if (clipName == "atk")
                return;

            Entity.PlayAudio($"data/wav/{clipName}");
        }
    }

    private void RenderLayers(ACT.Frame frame) {
        // If current frame doesn't have layers, cleanup layer cache
        Layers.Values.ToList().ForEach(Renderer => Renderer.sprite = null);

        for (int i = 0; i < frame.layers.Length; i++) {
            var layer = frame.layers[i];
            var sprite = sprites[layer.index];

            Layers.TryGetValue(i, out var spriteRenderer);

            if (spriteRenderer == null) {
                var go = new GameObject($"Layer{i}");
                spriteRenderer = go.AddComponent<SpriteRenderer>();
                spriteRenderer.transform.SetParent(gameObject.transform, false);
                spriteRenderer.material = SpriteMaterial;
            }

            CalculateSpritePositionScale(layer, sprite, out Vector3 scale, out Vector3 newPos, out Quaternion rotation);

            spriteRenderer.transform.localRotation = rotation;
            spriteRenderer.transform.localPosition = newPos;
            spriteRenderer.transform.localScale = scale;

            spriteRenderer.sprite = sprite;
            spriteRenderer.material.color = layer.color;

            if (!Layers.ContainsKey(i)) {
                Layers.Add(i, spriteRenderer);
            }
        }
    }

    private void UpdateMesh(ACT.Frame frame) {
        // We need this mesh collider in order to have the raycast to hit the sprite
        MeshCache.TryGetValue(frame, out Mesh mesh);
        if (mesh == null) {
            mesh = SpriteMeshBuilder.BuildColliderMesh(frame, sprites);
            MeshCache.Add(frame, mesh);
        }
        meshCollider.sharedMesh = mesh;
    }

    private int GetCurrentFrame(long tm) {
        var isIdle = CurrentMotion.Motion == SpriteMotion.Idle || CurrentMotion.Motion == SpriteMotion.Sit;
        double animCount = currentAction.frames.Length;

        if (isIdle) {
            return 0;
        }

        var stateCnt = tm / 24f;
        double currentMotion = 0;
        if (!AnimationHelper.IsLoopingMotion(CurrentMotion.Motion)) {
            var motionCount = animCount;
            currentMotion = (int) (stateCnt / motionSpeed % animCount);
            var loopCount = (stateCnt / motionSpeed) / motionCount;

            if (loopCount >= 1) {
                currentMotion--;
                if (loopCount >= loopCountToAnimationFinish) {
                    isAnimationFinished = true;
                }
            }
        } else {
            currentMotion = (int) (stateCnt / motionSpeed % animCount);
        }

        if (currentMotion <= 0) {
            currentMotion = 0;
        }

        return (int) currentMotion;
    }

    private float GetDelay() {
        if (currentAction == null) {
            return 4f;
        } else if (currentAction.delay >= 100f) {
            return 4f;
        } else {
            return currentAction.delay;
        }
    }

    internal void CalculateSpritePositionScale(ACT.Layer layer, Sprite sprite, out Vector3 scale, out Vector3 newPos, out Quaternion rotation) {
        rotation = Quaternion.Euler(0, 0, -layer.angle);
        scale = new Vector3(layer.scale.x * (layer.isMirror ? -1 : 1), layer.scale.y, 1);
        var offsetX = (Mathf.RoundToInt(sprite.rect.width) % 2 == 1) ? 0.5f : 0f;
        var offsetY = (Mathf.RoundToInt(sprite.rect.height) % 2 == 1) ? 0.5f : 0f;

        newPos = new Vector3(layer.pos.x - offsetX, -(layer.pos.y) + offsetY) / sprite.pixelsPerUnit;
    }

    public void ChangeMotion(MotionRequest motion, MotionRequest? nextMotion = null) {
        switch (motion.Motion) {
            case SpriteMotion.Dead:
                State = SpriteState.Dead;
                break;
            case SpriteMotion.Sit:
                State = SpriteState.Sit;
                break;
            case SpriteMotion.Idle:
                State = SpriteState.Idle;
                break;
            case SpriteMotion.Walk:
                State = SpriteState.Walking;
                break;
            default:
                State = SpriteState.Alive;
                break;
        }

        int newAction;
        if (motion.Motion == SpriteMotion.Attack) {
            var attackActions = new SpriteMotion[] { SpriteMotion.Attack1, SpriteMotion.Attack2, SpriteMotion.Attack3 };
            var action = DBManager.GetWeaponAction((Job) Entity.Status.jobId, Entity.Status.sex, Entity.EquipInfo.RightHand, Entity.EquipInfo.LeftHand);

            newAction = AnimationHelper.GetMotionIdForSprite(Entity.Type, attackActions[action]);

            /**
             * Seems like og client makes entity look diagonally up
             * when attacking from the sides
             */
            if (Entity.Direction == Direction.East) {
                Entity.Direction = Direction.NorthEast;
            } else if (Entity.Direction == Direction.West) {
                Entity.Direction = Direction.NorthWest;
            }
        } else {
            newAction = AnimationHelper.GetMotionIdForSprite(Entity.Type, motion.Motion);
        }

        CurrentMotion = motion;
        NextMotion = nextMotion;
        Entity.Action = newAction;
        ActionId = newAction;
        AnimationStart = GameManager.Tick;
        previousFrame = 0;
        motionSpeedMultiplier = 1;
        isAnimationFinished = false;

        motionSpeed = GetDelay();
        if (motionSpeed < 1) {
            motionSpeed = 1;
        }
        motionSpeed *= motionSpeedMultiplier;

        foreach (var child in Children) {
            child.ChangeMotion(motion, nextMotion);
        }
    }

    public void SetMotionSpeedMultipler(int attackMT) {
        //if (weapon is bow)
        if (attackMT > MAX_ATTACK_SPEED) {
            attackMT = MAX_ATTACK_SPEED;
        }
        //endif

        motionSpeedMultiplier = (float) attackMT / AVERAGE_ATTACK_SPEED;
        motionSpeed *= motionSpeedMultiplier;
    }

    private void InitShadow() {
        if (ViewerType != ViewerType.BODY)
            return;

        var shadow = new GameObject("Shadow");
        shadow.layer = LayerMask.NameToLayer("Characters");
        shadow.transform.SetParent(transform, false);
        shadow.transform.localPosition = Vector3.zero;
        shadow.transform.localScale = new Vector3(Entity.ShadowSize, Entity.ShadowSize, Entity.ShadowSize);
        var sortingGroup = shadow.AddComponent<SortingGroup>();
        sortingGroup.sortingOrder = -20001;

        SPR sprite = FileManager.Load("data/sprite/shadow.spr") as SPR;

        sprite.SwitchToRGBA();

        var spriteRenderer = shadow.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite.GetSprites()[0];
        spriteRenderer.sortingOrder = -1;
        spriteRenderer.material.color = new Color(1, 1, 1, 0.4f);
    }

    public Vector2 GetAnimationAnchor() {
        var frame = currentAction.frames[currentFrame];
        if (frame.pos.Length > 0)
            return frame.pos[0];
        if (ViewerType == ViewerType.HEAD && (State == SpriteState.Idle || State == SpriteState.Sit))
            return frame.pos[currentFrame];
        return Vector2.zero;
    }

    public struct MotionRequest {
        public SpriteMotion Motion;
        public double delay;
    }
}

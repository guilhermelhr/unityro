﻿using Assets.Scripts.Renderer.Sprite;
using ROIO.Models.FileTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using UnityRO.GameCamera;

public class EntityViewer : MonoBehaviour {

    private const int AVERAGE_ATTACK_SPEED = 432;
    private const int AVERAGE_ATTACKED_SPEED = 288;
    private const int MAX_ATTACK_SPEED = AVERAGE_ATTACKED_SPEED * 2;

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

    private PaletteData CurrentPaletteData;
    private Sprite[] sprites;
    private ACT currentACT;
    private ACT.Action currentAction;
    private int currentActionIndex;
    private int currentViewID;
    private int currentFrame = 0;
    private long AnimationStart;
    private int ActionId = -1;
    private double previousFrame = 0;

    private double motionSpeed = 4;
    private bool isAnimationFinished;
    private double loopCountToAnimationFinish = 1;
    private float motionSpeedMultiplier = 1f;

    private MeshCollider meshCollider;
    private Material SpriteMaterial;
    private Texture2D PaletteTexture;

    public void Start() {
        SpriteMaterial = Resources.Load("Materials/Sprites/SpriteMaterial") as Material;
        Init();

        InitShadow();
    }

    public void Init(SpriteData spriteData) {
        currentACT = spriteData.act;
        sprites = spriteData.sprites;
    }

    public async void Init(bool reloadSprites = false) {
        CurrentPaletteData = new PaletteData {
            hairColor = Entity.Status.hair_color,
            hair = Entity.Status.hair,
            clothesColor = Entity.Status.clothes_color
        };
        meshCollider = gameObject.GetOrAddComponent<MeshCollider>();

        if (Entity.Type == EntityType.WARP) {
            return;
        }

        if (sprites == null || reloadSprites) {
            string path = "";
            string palettePath = "";

            switch (ViewerType) {
                case ViewerType.BODY:
                    path = DBManager.GetBodyPath(Entity.Status.jobId, Entity.Status.sex);
                    if (Entity.Status.clothes_color > 0) {
                        palettePath = DBManager.GetBodyPalPath(Entity.Status.jobId, Entity.Status.clothes_color, Entity.Status.sex);
                    }
                    break;
                case ViewerType.HEAD:
                    path = DBManager.GetHeadPath(Entity.Status.jobId, Entity.Status.hair, Entity.Status.sex);
                    if (Entity.Status.hair_color > 0) {
                        palettePath = DBManager.GetHeadPalPath(Entity.Status.hair, Entity.Status.hair_color, Entity.Status.sex);
                    }
                    break;
                case ViewerType.WEAPON:
                    currentViewID = Entity.EquipInfo.Weapon;
                    path = DBManager.GetWeaponPath(currentViewID, Entity.Status.jobId, Entity.Status.sex);
                    break;
                case ViewerType.SHIELD:
                    currentViewID = Entity.EquipInfo.Shield;
                    path = DBManager.GetShieldPath(currentViewID, Entity.Status.jobId, Entity.Status.sex);
                    break;
                case ViewerType.HEAD_TOP:
                    currentViewID = Entity.EquipInfo.HeadTop;
                    path = DBManager.GetHatPath(currentViewID, Entity.Status.sex);
                    break;
                case ViewerType.HEAD_MID:
                    currentViewID = Entity.EquipInfo.HeadMid;
                    path = DBManager.GetHatPath(currentViewID, Entity.Status.sex);
                    break;
                case ViewerType.HEAD_BOTTOM:
                    currentViewID = Entity.EquipInfo.HeadBottom;
                    path = DBManager.GetHatPath(currentViewID, Entity.Status.sex);
                    break;
            }

            if (ViewerType != ViewerType.BODY && ViewerType != ViewerType.HEAD && currentViewID <= 0) {
                currentACT = null;
                sprites = null;
                Layers.Values.ToList().ForEach(Renderer => {
                    Destroy(Renderer.gameObject);
                });
                Layers.Clear();
                MeshCache.Clear();

                return;
            }

            try {
                var spriteData = await Addressables.LoadAssetAsync<SpriteData>(path + ".asset").Task;

                // Figure out a way of using palettes with shaders
                //if (palettePath.Length > 0) {
                //    if (FileManager.Load(palettePath) is byte[] currentPalette) {
                //        PaletteTexture = new Texture2D(256, 1, TextureFormat.RGBA32, false) {
                //            filterMode = FilterMode.Point,
                //            wrapMode = TextureWrapMode.Clamp
                //        };
                //        PaletteTexture.LoadRawTextureData(currentPalette);
                //        PaletteTexture.Apply();
                //    }
                //}

                sprites = spriteData.sprites;
                currentACT = spriteData.act;
            } catch {
                Debug.LogError($"Could not load sprites for: {path}");
                currentACT = null;
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
        if (currentACT == null || !Entity.IsReady)
            return;

        if (ViewerType != ViewerType.BODY && ViewerType != ViewerType.HEAD) {
            var updatedViewID = FindCurrentViewID();
            if (updatedViewID != currentViewID) {
                Init(reloadSprites: true);

                return;
            }
        }

        if (CheckForEntityViewsUpdates()) {
            Init(reloadSprites: true);
            return;
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

        if (currentAction == null) {
            return;
        }

        UpdateMesh(frame);
        RenderLayers(frame);
        PlaySound(frame);
        UpdateAnchorPoints();
    }

    private bool CheckForEntityViewsUpdates() {
        if (ViewerType != ViewerType.BODY && ViewerType != ViewerType.HEAD) {
            return FindCurrentViewID() != currentViewID;
        }

        if (Entity.Status.hair != CurrentPaletteData.hair ||
            Entity.Status.hair_color != CurrentPaletteData.hairColor ||
            Entity.Status.clothes_color != CurrentPaletteData.clothesColor) {
            return true;
        }

        return false;
    }

    private int FindCurrentViewID() {
        switch (ViewerType) {
            case ViewerType.WEAPON:
                return Entity.EquipInfo.Weapon;
            case ViewerType.SHIELD:
                return Entity.EquipInfo.Shield;
            case ViewerType.HEAD_TOP:
                return Entity.EquipInfo.HeadTop;
            case ViewerType.HEAD_MID:
                return Entity.EquipInfo.HeadMid;
            case ViewerType.HEAD_BOTTOM:
                return Entity.EquipInfo.HeadBottom;
            default:
                return -1;
        }
    }

    private void UpdateAnchorPoints() {
        if (Parent != null && ViewerType != ViewerType.WEAPON) {
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
                SpriteMaterial.SetTexture("_PaletteTex", PaletteTexture);

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

    /**
     * Hammer time
     */
    private int GetCurrentFrame(long tm) {
        var isIdle = CurrentMotion.Motion == SpriteMotion.Idle || CurrentMotion.Motion == SpriteMotion.Sit;
        double animCount = currentAction.frames.Length;

        switch (CurrentMotion.Motion) {
            case SpriteMotion.Attack:
            case SpriteMotion.Attack1:
            case SpriteMotion.Attack2:
            case SpriteMotion.Attack3:
                return GetAttackFrame(tm, isIdle, animCount);
            default:
                return GetEverythingElseFrame(tm, isIdle, animCount);

        }
    }

    private int GetEverythingElseFrame(long tm, bool isIdle, double animCount) {
        long delay = (long) GetDelay();
        if (delay <= 0) {
            delay = (int) currentAction.delay;
        }
        var headDir = 0;
        double frame;

        if (ViewerType == ViewerType.BODY && Entity.Type == EntityType.PC && isIdle) {
            return Entity.HeadDir;
        }

        if ((ViewerType == ViewerType.HEAD ||
            ViewerType == ViewerType.HEAD_TOP ||
            ViewerType == ViewerType.HEAD_MID ||
            ViewerType == ViewerType.HEAD_BOTTOM) && isIdle) {
            animCount = Math.Floor(animCount / 3);
            headDir = Entity.HeadDir;
        }

        if (AnimationHelper.IsLoopingMotion(CurrentMotion.Motion)) {
            frame = Math.Floor((double) (tm / delay));
            frame %= animCount;
            frame += animCount * headDir;
            frame += previousFrame;
            frame %= animCount;

            return (int) frame;
        }

        frame = Math.Min(tm / delay | 0, animCount);
        frame += (animCount * headDir);
        frame += previousFrame;

        if (ViewerType == ViewerType.BODY && frame >= animCount - 1) {
            previousFrame = frame = animCount - 1;

            if (CurrentMotion.delay > 0 && GameManager.Tick < CurrentMotion.delay) {
                if (NextMotion.HasValue) {
                    StartCoroutine(ChangeMotionAfter(NextMotion.Value, (float) (CurrentMotion.delay - GameManager.Tick) / 1000f));
                }
            } else {
                if (NextMotion.HasValue) {
                    ChangeMotion(NextMotion.Value);
                }
            }
        }

        return (int) Math.Min(frame, animCount - 1);
    }

    private int GetAttackFrame(long tm, bool isIdle, double animCount) {
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

    private IEnumerator ChangeMotionAfter(MotionRequest motion, float time) {
        yield return new WaitForSeconds(time);

        ChangeMotion(motion);
    }

    private float GetDelay() {
        if (currentACT == null || currentAction == null) {
            return 4f;
        }

        if (ViewerType == ViewerType.BODY && CurrentMotion.Motion == SpriteMotion.Walk) {
            return (int) (currentAction.delay / 150 * Entity.Status.walkSpeed);
        }

        if (CurrentMotion.Motion == SpriteMotion.Attack ||
            CurrentMotion.Motion == SpriteMotion.Attack1 ||
            CurrentMotion.Motion == SpriteMotion.Attack2 ||
            CurrentMotion.Motion == SpriteMotion.Attack3) {

            if (currentAction == null) {
                return 4f;
            } else if (currentAction.delay >= 100f) {
                return 4f;
            } else {
                return currentAction.delay;
            }
        }

        return (int) currentAction.delay;
    }

    private void CalculateSpritePositionScale(ACT.Layer layer, Sprite sprite, out Vector3 scale, out Vector3 newPos, out Quaternion rotation) {
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
            var action = DBManager.GetWeaponAction((Job) Entity.Status.jobId, Entity.Status.sex, Entity.EquipInfo.Weapon, Entity.EquipInfo.Shield);
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

    private async void InitShadow() {
        if (ViewerType != ViewerType.BODY)
            return;

        var shadow = new GameObject("Shadow");
        shadow.layer = LayerMask.NameToLayer("Characters");
        shadow.transform.SetParent(transform, false);
        shadow.transform.localPosition = Vector3.zero;
        shadow.transform.localScale = new Vector3(Entity.ShadowSize, Entity.ShadowSize, Entity.ShadowSize);
        var sortingGroup = shadow.AddComponent<SortingGroup>();
        sortingGroup.sortingOrder = -20001;

        var spriteData = await Addressables.LoadAssetAsync<SpriteData>("data/sprite/shadow.asset").Task;

        var spriteRenderer = shadow.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteData.sprites[0];
        spriteRenderer.sortingOrder = -1;
        spriteRenderer.material.color = new Color(1, 1, 1, 0.4f);
    }

    public Vector2 GetAnimationAnchor() {
        if (currentAction == null) {
            return Vector2.zero;
        }

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

    public struct PaletteData {
        public int hair;
        public int hairColor;
        public int clothesColor;
    }

    internal void SetMotionSpeedMultipler(ushort attackMT) {
        //if (weapon is bow)
        if (attackMT > MAX_ATTACK_SPEED) {
            attackMT = MAX_ATTACK_SPEED;
        }
        //endif

        motionSpeedMultiplier = (float) attackMT / AVERAGE_ATTACK_SPEED;
        motionSpeed *= motionSpeedMultiplier;
    }
}

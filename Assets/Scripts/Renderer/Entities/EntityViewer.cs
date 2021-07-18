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
    private AudioSource AudioSource;

    private Sprite[] sprites;
    private ACT currentACT;
    private SPR currentSPR;
    private ACT.Action currentAction;
    private int currentActionIndex;

    [SerializeField]
    private int currentViewID;
    private int currentFrame = 0;
    private long AnimationStart;
    private int ActionId = -1;
    private double previousFrame = 0;

    private MeshCollider meshCollider;

    public void Init(SPR spr, ACT act) {
        currentSPR = spr;
        currentACT = act;

        currentSPR.SwitchToRGBA();
        sprites = currentSPR.GetSprites();
    }

    public void Start() {
        Init();

        InitShadow();

        if (AudioSource == null && Parent == null) {
            AudioSource = gameObject.AddComponent<AudioSource>();
            AudioSource.spatialBlend = 0.7f;
            AudioSource.priority = 60;
            AudioSource.maxDistance = 40;
            AudioSource.rolloffMode = AudioRolloffMode.Linear;
            AudioSource.volume = 1f;
            AudioSource.dopplerLevel = 0;
            AudioSource.outputAudioMixerGroup = MapRenderer.SoundsMixerGroup;
        }
    }

    public void Init(bool reloadSprites = false) {
        if (currentSPR == null || reloadSprites) {
            string path = "";

            switch (ViewerType) {
                case ViewerType.BODY:
                    path = DBManager.GetBodyPath((Job)Entity.Status.jobId, Entity.Status.sex);
                    break;
                case ViewerType.HEAD:
                    path = DBManager.GetHeadPath(Entity.Status.hair, Entity.Status.sex);
                    break;
                case ViewerType.WEAPON:
                    currentViewID = Entity.EquipInfo.Weapon;
                    path = DBManager.GetWeaponPath(currentViewID, Entity.Status.jobId, Entity.Status.sex);
                    break;
                case ViewerType.SHIELD:
                    currentViewID = Entity.EquipInfo.Weapon;
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

        meshCollider = gameObject.GetOrAddComponent<MeshCollider>();

        if (currentAction == null) {
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

        currentActionIndex =
            (ActionId + // action
            ((int) CharacterCamera.ROCamera.Direction + (int)Entity.Direction + 8) % 8 // direction
            ) % currentACT.actions.Length; // avoid overflow
        currentAction = currentACT.actions[currentActionIndex];
        currentFrame = GetCurrentFrame(Core.Tick - AnimationStart);
        var frame = currentAction.frames[currentFrame];

        UpdateMesh(frame);
        RenderLayers(frame);
        PlaySound(frame);
        UpdateAnchorPoints();
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
            if (clipName == "atk") return;

            var clip = FileManager.Load($"data/wav/{clipName}") as AudioClip;

            if (clip != null && AudioSource != null) {
                AudioSource.clip = clip;
                AudioSource.Play();
            }
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
        long delay = GetDelay();
        if (delay <= 0) {
            delay = (int)currentAction.delay;
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
            frame = Math.Floor((double)(tm / delay));
            frame %= animCount;
            frame += animCount * headDir;
            frame += previousFrame;
            frame %= animCount;

            return (int)frame;
        }

        frame = Math.Min(tm / delay | 0, animCount);
        frame += (animCount * headDir);
        frame += previousFrame;

        if (ViewerType == ViewerType.BODY && frame >= animCount - 1) {
            previousFrame = frame = animCount - 1;

            if (CurrentMotion.delay > 0 && Core.Tick < CurrentMotion.delay) {
                if (NextMotion.HasValue) {
                    StartCoroutine(ChangeMotionAfter(NextMotion.Value, (float)(CurrentMotion.delay - Core.Tick) / 1000f));
                }
            } else {
                if (NextMotion.HasValue) {
                    ChangeMotion(NextMotion.Value);
                }
            }
        }

        return (int)Math.Min(frame, animCount - 1);
    }

    private IEnumerator ChangeMotionAfter(MotionRequest motion, float time) {
        yield return new WaitForSeconds(time);

        ChangeMotion(motion);
    }

    private int GetDelay() {
        if (ViewerType == ViewerType.BODY && CurrentMotion.Motion == SpriteMotion.Walk) {
            return (int)(currentAction.delay / 150 * Entity.Status.walkSpeed);
        }

        if (CurrentMotion.Motion == SpriteMotion.Attack ||
            CurrentMotion.Motion == SpriteMotion.Attack1 ||
            CurrentMotion.Motion == SpriteMotion.Attack2 ||
            CurrentMotion.Motion == SpriteMotion.Attack3) {
            var delay = (int)(currentAction.delay * (Entity.Status.attackSpeed / AVERAGE_ATTACK_SPEED));

            return (delay > 0) ? delay : Entity.Status.attackSpeed / currentAction.FrameCount;
        }

        return (int)currentAction.delay;
    }

    private void CalculateSpritePositionScale(ACT.Layer layer, Sprite sprite, out Vector3 scale, out Vector3 newPos, out Quaternion rotation) {
        rotation = Quaternion.Euler(0, 0, -layer.angle);
        scale = new Vector3(layer.scale.x * (layer.isMirror ? -1 : 1), -(layer.scale.y), 1);
        var offsetX = (Mathf.RoundToInt(sprite.rect.width) % 2 == 1) ? 0.5f : 0f;
        var offsetY = (Mathf.RoundToInt(sprite.rect.height) % 2 == 1) ? 0.5f : 0f;

        newPos = new Vector3(layer.pos.x - offsetX, -(layer.pos.y) + offsetY) / sprite.pixelsPerUnit;
    }

    public void ChangeMotion(MotionRequest motion, MotionRequest? nextMotion = null) {
        State = SpriteState.Alive;

        int newAction;
        if (motion.Motion == SpriteMotion.Attack) {
            var attackActions = new SpriteMotion[] { SpriteMotion.Attack1, SpriteMotion.Attack2, SpriteMotion.Attack3 };
            var action = DBManager.GetWeaponAction((Job)Entity.Status.jobId, Entity.Status.sex, Entity.EquipInfo.Weapon);
            newAction = AnimationHelper.GetMotionIdForSprite(Entity.Type, attackActions[action]);
        } else {
            newAction = AnimationHelper.GetMotionIdForSprite(Entity.Type, motion.Motion);
        }

        CurrentMotion = motion;
        NextMotion = nextMotion;
        Entity.Action = newAction;
        ActionId = newAction;
        AnimationStart = Core.Tick;
        previousFrame = 0;

        foreach (var child in Children) {
            child.ChangeMotion(motion, nextMotion);
        }
    }

    private void InitShadow() {
        if (ViewerType != ViewerType.BODY) return;

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

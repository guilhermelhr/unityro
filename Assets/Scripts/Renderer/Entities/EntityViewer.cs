using ROIO;
using ROIO.Models.FileTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class EntityViewer : MonoBehaviour {

    public Entity Entity;
    public EntityType Type;
    public EntityViewer Parent;
    public ViewerType ViewerType;

    public SpriteMotion CurrentMotion;
    public SpriteMotion? NextMotion;

    public float SpriteOffset;
    public int HeadDirection;
    public int SpriteOrder;
    public SpriteState State = SpriteState.Idle;

    public List<EntityViewer> Children = new();
    private Dictionary<int, SpriteRenderer> Layers = new();
    private Dictionary<ACT.Frame, Mesh> MeshCache = new();
    private AudioSource AudioSource;

    private Sprite[] sprites;
    private ACT currentACT;
    private SPR currentSPR;
    private ACT.Action currentAction;
    private int currentActionIndex;

    private int currentFrame = 0;
    private long AnimationStart;
    private int ActionId = -1;

    private MeshCollider meshCollider;

    public void Init(SPR spr, ACT act) {
        currentSPR = spr;
        currentACT = act;
    }

    public void Start() {
        if (currentSPR == null) {
            string path = "";

            switch (ViewerType) {
                case ViewerType.BODY:
                    path = DBManager.GetBodyPath((Job)Entity.Status.jobId, Entity.Status.sex);
                    break;
                case ViewerType.HEAD:
                    path = DBManager.GetHeadPath(Entity.Status.hair, Entity.Status.sex);
                    break;
                case ViewerType.WEAPON:
                    path = DBManager.GetWeaponPath(Entity.Status.weapon, Entity.Status.jobId, Entity.Status.sex);
                    break;
            }

            currentSPR = FileManager.Load(path + ".spr") as SPR;
            currentACT = FileManager.Load(path + ".act") as ACT;
        }

        currentSPR.SwitchToRGBA();
        sprites = currentSPR.GetSprites();
        meshCollider = gameObject.GetOrAddComponent<MeshCollider>();

        if (currentAction == null) {
            ChangeMotion(SpriteMotion.Idle);
        }

        foreach (var child in Children) {
            child.Start();
        }

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

    void FixedUpdate() {
        if (!Entity.IsReady || currentACT == null)
            return;

        currentActionIndex = ActionId + GetFacingDirection() % currentACT.actions.Length;
        currentAction = currentACT.actions[currentActionIndex];
        currentFrame = GetCurrentFrame(Core.Tick - AnimationStart);
        var frame = currentAction.frames[currentFrame];

        UpdateMesh(frame);
        RenderLayers(frame);
        PlaySound(frame);
        UpdateAnchorPoints();
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
                spriteRenderer.sortingOrder = SpriteOrder;
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
        var isIdle = CurrentMotion == SpriteMotion.Idle || CurrentMotion == SpriteMotion.Sit;
        double animCount = currentAction.frames.Length;
        long delay = GetDelay();
        var headDir = 0;
        double frame;

        if (ViewerType == ViewerType.BODY && Type == EntityType.PC && isIdle) {
            return Entity.HeadDir;
        }

        if (ViewerType == ViewerType.HEAD && isIdle) {
            animCount = Math.Floor(animCount / 3);
            headDir = Entity.HeadDir;
        }

        if (AnimationHelper.IsLoopingMotion(CurrentMotion)) {
            frame = Math.Floor((double)(tm / delay));

            frame %= animCount;

            frame += animCount * headDir;

            frame %= animCount;

            return (int)frame;
        }

        frame = Math.Min(tm / delay | 0, animCount) + (animCount * headDir) + 0;
        if (ViewerType == ViewerType.BODY && frame >= animCount - 1) {
            frame = animCount - 1;
            if (NextMotion.HasValue) {
                ChangeMotion(NextMotion.Value);
            }
        }

        return (int)Math.Min(frame, animCount - 1);
    }

    private int GetDelay() {
        if (ViewerType == ViewerType.BODY && CurrentMotion == SpriteMotion.Walk) {
            return (int)(currentAction.delay / 150 * Entity.Status.walkSpeed);
        }

        if (CurrentMotion == SpriteMotion.Attack ||
            CurrentMotion == SpriteMotion.Attack1 ||
            CurrentMotion == SpriteMotion.Attack2 ||
            CurrentMotion == SpriteMotion.Attack3) {
            return Entity.Status.attackSpeed / currentAction.frames.Length;
        }

        return (int)currentAction.delay;
    }

    private int GetFacingDirection() {
        int angle;
        if (AnimationHelper.IsFourDirectionAnimation(Type, CurrentMotion)) {
            angle = AnimationHelper.GetFourDirectionSpriteIndexForAngle(Entity.Direction, 360 - ROCamera.Instance.Rotation);
        } else {
            angle = AnimationHelper.GetSpriteIndexForAngle(Entity.Direction, 360 - ROCamera.Instance.Rotation);
        }

        return angle < 0 ? 0 : angle;
    }

    private void CalculateSpritePositionScale(ACT.Layer layer, Sprite sprite, out Vector3 scale, out Vector3 newPos, out Quaternion rotation) {
        rotation = Quaternion.Euler(0, 0, -layer.angle);
        scale = new Vector3(layer.scale.x * (layer.isMirror ? -1 : 1), -(layer.scale.y), 1);
        var offsetX = (Mathf.RoundToInt(sprite.rect.width) % 2 == 1) ? 0.5f : 0f;
        var offsetY = (Mathf.RoundToInt(sprite.rect.height) % 2 == 1) ? 0.5f : 0f;

        newPos = new Vector3(layer.pos.x - offsetX, -(layer.pos.y) + offsetY) / sprite.pixelsPerUnit;
    }

    public void ChangeMotion(SpriteMotion motion, SpriteMotion? nextMotion = null, ushort speed = 0, ushort factor = 0) {
        State = SpriteState.Alive;
        var newAction = AnimationHelper.GetMotionIdForSprite(Entity.Type, motion);

        CurrentMotion = motion;
        NextMotion = nextMotion;

        Entity.Action = newAction;

        ActionId = newAction;
        AnimationStart = Core.Tick;

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
}

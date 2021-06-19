using ROIO;
using ROIO.Models.FileTypes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class EntityViewer : MonoBehaviour {
    public const float AVERAGE_ASPD = 432f;

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

    public List<EntityViewer> Children = new List<EntityViewer>();
    private Dictionary<int, SpriteRenderer> Layers = new Dictionary<int, SpriteRenderer>();
    private Dictionary<ACT.Frame, Mesh> MeshCache = new Dictionary<ACT.Frame, Mesh>();
    private AudioSource AudioSource;

    private Sprite[] sprites;
    private ACT currentACT;
    private SPR currentSPR;
    private ACT.Action currentAction;
    private int currentActionIndex;

    private int currentFrame = 0;

    private long _start;
    private ushort _time;
    private float _factor;
    private int _action = -1;
    private int _next = -1;

    private bool isPaused;

    private MeshCollider meshCollider;
    private Material material;

    public void Init(SPR spr, ACT act) {
        currentSPR = spr;
        currentACT = act;
    }

    public void Start() {
        if(currentSPR == null) {
            string path = "";

            switch(ViewerType) {
                case ViewerType.BODY:
                    path = DBManager.GetBodyPath((Job)Entity.GetBaseStatus().jobId, Entity.GetBaseStatus().sex);
                    break;
                case ViewerType.HEAD:
                    path = DBManager.GetHeadPath(Entity.GetBaseStatus().hair, Entity.GetBaseStatus().sex);
                    break;
                case ViewerType.WEAPON:
                    path = DBManager.GetWeaponPath(Entity.GetBaseStatus().weapon, Entity.GetBaseStatus().jobId, Entity.GetBaseStatus().sex);
                    break;
            }

            currentSPR = FileManager.Load(path + ".spr") as SPR;
            currentACT = FileManager.Load(path + ".act") as ACT;
        }

        //if (material == null) {
        //    material = Resources.Load("Materials/Unlit_CustomSpriteShader") as Material;
        //}

        currentSPR.SwitchToRGBA();
        sprites = currentSPR.GetSprites();
        meshCollider = gameObject.GetOrAddComponent<MeshCollider>();

        if(currentAction == null) {
            ChangeMotion(SpriteMotion.Idle);
        }

        foreach(var child in Children) {
            child.Start();
        }

        InitShadow();

        if(AudioSource == null && Parent == null) {
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

    void Update() {
        if(!Entity.IsReady || currentACT == null)
            return;
        if(State == SpriteState.Dead) {
            return;
        }

        long deltaTime = Core.Tick - _start;
        if(_time > 0 && deltaTime > _time) {
            if(NextMotion != null) {
                ChangeMotion(NextMotion.Value, null);
            } else {
                ChangeMotion(SpriteMotion.Idle, null);
            }
        }

        currentActionIndex = _action + GetFacingDirection() % currentACT.actions.Length;
        currentAction = currentACT.actions[currentActionIndex];

        var newFrame = GetCurrentFrame(deltaTime);

        // Are we looping or stopping?
        if(newFrame >= currentAction.frames.Length - 1) {
            currentFrame = currentAction.frames.Length - 1;
            if(NextMotion != null) {
                ChangeMotion(NextMotion.Value, null);
            } else if(CurrentMotion == SpriteMotion.Dead) {
                State = SpriteState.Dead;
            }
        } else {
            currentFrame = newFrame;
        }

        var frame = currentAction.frames[currentFrame];

        // We need this mesh collider in order to have the raycast to hit the sprite
        MeshCache.TryGetValue(frame, out Mesh mesh);
        if(mesh == null) {
            mesh = SpriteMeshBuilder.BuildColliderMesh(frame, sprites);
            MeshCache.Add(frame, mesh);
        }
        meshCollider.sharedMesh = mesh;

        // If current frame doesn't have layers, cleanup layer cache
        Layers.Values.ToList().ForEach(Renderer => Renderer.sprite = null);

        for(int i = 0; i < frame.layers.Length; i++) {
            var layer = frame.layers[i];
            var sprite = sprites[layer.index];

            Layers.TryGetValue(i, out var spriteRenderer);

            if(spriteRenderer == null) {
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

            if(!Layers.ContainsKey(i)) {
                Layers.Add(i, spriteRenderer);
            }
        }

        if(frame.soundId > -1 && frame.soundId < currentACT.sounds.Length) {
            var clipName = currentACT.sounds[frame.soundId];
            if(clipName == "atk") return;

            var clip = FileManager.Load($"data/wav/{clipName}") as AudioClip;

            if(clip != null && AudioSource != null) {
                AudioSource.clip = clip;
                AudioSource.Play();
            }
        }

        if(Parent != null && ViewerType != ViewerType.WEAPON) {
            var parentAnchor = Parent.GetAnimationAnchor();
            var ourAnchor = GetAnimationAnchor();

            var diff = parentAnchor - ourAnchor;

            transform.localPosition = new Vector3(diff.x, -diff.y, 0f) / SPR.PIXELS_PER_UNIT;
        }
    }

    private int GetCurrentFrame(long deltaTime) {
        if(Entity.Type == EntityType.PC) {
            return (Entity.HeadDir < 0 || _action > 0 ? GetFrameIndex(deltaTime) : Entity.HeadDir) % currentAction.frames.Length;
        } else {
            return (Entity.HeadDir < 0 || _action >= 0 ? GetFrameIndex(deltaTime) : Entity.HeadDir) % currentAction.frames.Length;
        }
    }

    private int GetFacingDirection() => ((int)ROCamera.Instance.Direction + (int)Entity.Direction + 8) % 8;

    /**
     * TODO: the correct formula needs to be found
     * Aegis formula doesn't seem to work well here
     */
    private int GetFrameIndex(long tm) {
        return (int)(tm / currentAction.delay);
        //if(_time > 0) {
        //    var motionspeed = (currentAction.delay < 0 ? 4f : currentAction.delay) * _factor;
        //    return (int)(GetStateCnt() / motionspeed) % currentAction.frames.Length;

        //    var motion_speed = (currentAction.delay < 0 ? 4f : currentAction.delay) * (_time / AVERAGE_ASPD);
        //    return (int)((tm * 0.37f * 4.0f) / motion_speed) % currentAction.frames.Length;
        //} else if(_factor > 0) {
        //    return (int)(tm / (currentAction.delay * _factor / 100));
        //} else {
        //    return (int)(tm / currentAction.delay);
        //}
    }

    private float GetStateCnt() { return Core.Tick - _start; }

    private void CalculateSpritePositionScale(ACT.Layer layer, Sprite sprite, out Vector3 scale, out Vector3 newPos, out Quaternion rotation) {
        rotation = Quaternion.Euler(0, 0, -layer.angle);
        scale = new Vector3(layer.scale.x * (layer.isMirror ? -1 : 1), -(layer.scale.y), 1);
        var offsetX = (Mathf.RoundToInt(sprite.rect.width) % 2 == 1) ? 0.5f : 0f;
        var offsetY = (Mathf.RoundToInt(sprite.rect.height) % 2 == 1) ? 0.5f : 0f;

        newPos = new Vector3(layer.pos.x - offsetX, -(layer.pos.y) + offsetY) / sprite.pixelsPerUnit;
    }

    public void ChangeMotion(SpriteMotion motion, SpriteMotion? nextMotion = null, ushort speed = 0, float factor = 0) {
        State = SpriteState.Alive;
        var newAction = AnimationHelper.GetMotionIdForSprite(Entity.Type, motion);
        var nextAction = 0;
        if(nextMotion != null) {
            nextAction = AnimationHelper.GetMotionIdForSprite(Entity.Type, nextMotion.Value);
        }

        if(newAction != _action) {
            CurrentMotion = motion;
            NextMotion = nextMotion;

            Entity.Action = newAction;

            _action = newAction;
            _start = Core.Tick;
        }

        _next = nextAction;
        _time = speed;
        _factor = factor;

        foreach(var child in Children) {
            child.ChangeMotion(motion, nextMotion);
        }
    }

    private void InitShadow() {
        if(ViewerType != ViewerType.BODY) return;

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
        if(frame.pos.Length > 0)
            return frame.pos[0];
        if(ViewerType == ViewerType.HEAD && (State == SpriteState.Idle || State == SpriteState.Sit))
            return frame.pos[currentFrame];
        return Vector2.zero;
    }
}

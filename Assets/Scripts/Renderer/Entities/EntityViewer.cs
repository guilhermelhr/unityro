using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class EntityViewer : MonoBehaviour {

    public Entity Entity;
    public EntityType Type;
    public EntityViewer Parent;
    public ViewerType _ViewerType;
    public SpriteMotion CurrentMotion;
    public float SpriteOffset;
    public int HeadDirection;
    public int SpriteOrder;
    public float AnimSpeed = 1f;

    public SpriteState State = SpriteState.Idle;

    public List<EntityViewer> Children = new List<EntityViewer>();
    private Dictionary<int, SpriteRenderer> Layers = new Dictionary<int, SpriteRenderer>();
    private Dictionary<ACT.Frame, Mesh> MeshCache = new Dictionary<ACT.Frame, Mesh>();

    private Sprite[] sprites;
    private ACT currentACT;
    private SPR currentSPR;
    private ACT.Action currentAction;
    private int currentActionIndex;
    private int currentAngleIndex;

    private int currentFrame = 0;
    private float currentFrameTime = 0;
    private int maxFrame = 0;

    private bool isLooping;
    private bool isPaused;
    private bool isDirty;

    private MeshCollider meshCollider;

    public void Start() {
        var path = _ViewerType == ViewerType.BODY ? DBManager.GetBodyPath((Job)Entity.Job, Entity.Sex) : DBManager.GetHeadPath(Entity.Hair, Entity.Sex);
        currentSPR = FileManager.Load(path + ".spr") as SPR;
        currentACT = FileManager.Load(path + ".act") as ACT;
        sprites = currentSPR.GetSprites();
        meshCollider = gameObject.GetOrAddComponent<MeshCollider>();

        if (currentAction == null)
            ChangeAction(0);

        InitShadow();

        foreach (var child in Children) {
            child.Start();
        }
    }

    void Update() {
        if (!Entity.IsReady || currentACT == null)
            return;

        if (currentAction == null)
            ChangeAction(0);

        bool is4dir = AnimationHelper.IsFourDirectionAnimation(Type, CurrentMotion);
        int angleIndex;
        if (is4dir) {
            angleIndex = AnimationHelper.GetFourDirectionSpriteIndexForAngle(Entity.Direction, 360 - ROCamera.Instance.Rotation);
        } else {
            angleIndex = AnimationHelper.GetSpriteIndexForAngle(Entity.Direction, 360 - ROCamera.Instance.Rotation);
        }

        if (currentAngleIndex != angleIndex) {
            ChangeAngle(angleIndex);
        }

        if (currentActionIndex != Entity.Action) {
            ChangeAction(Entity.Action);
        }

        if (Parent == null)
            currentFrameTime -= Time.deltaTime;

        if (currentFrameTime < 0 || currentFrame > maxFrame) {
            AdvanceFrame();
        }

        if (isDirty) {
            UpdateSpriteFrame();

            foreach (var child in Children) {
                child.ChildSetFrameData(currentActionIndex, currentAngleIndex, currentFrame);
            }

            isDirty = false;
        }
    }

    private void AdvanceFrame() {
        if (!isPaused)
            currentFrame++;
        if (currentFrame > maxFrame) {
            var nextMotion = AnimationHelper.GetMotionForState(State);
            if (nextMotion != CurrentMotion)
                ChangeMotion(nextMotion);
            else {
                if (State != SpriteState.Dead)
                    currentFrame = 0;
                else {
                    currentFrame = maxFrame;
                    isPaused = true;
                }
            }
        }

        if (currentFrameTime < 0)
            currentFrameTime += currentAction.delay / 1000f * AnimSpeed;

        if (!isPaused)
            isDirty = true;
    }

    private void ChangeAngle(int newAngleIndex) {
        if (currentACT == null && Entity.isActiveAndEnabled) return;
        currentAngleIndex = newAngleIndex;
        //if (!isInitialized) return;
        currentAction = currentACT.actions[currentActionIndex + currentAngleIndex];
        maxFrame = currentAction.frames.Length - 1;
        isDirty = true;
    }

    private void ChangeAction(int newActionIndex) {
        if (currentACT == null || !Entity.gameObject.activeSelf) return;
        Entity.Action = newActionIndex;
        currentActionIndex = newActionIndex;
        //if (!isInitialized) return;
        currentAction = currentACT.actions[currentActionIndex + currentAngleIndex];
        maxFrame = currentAction.frames.Length - 1;
        currentFrameTime = currentAction.delay / 1000f * AnimSpeed; //reset current frame time
        isDirty = true;

        RenderLayers();
    }

    /** 
     * TODO 
     * Figure out a way of cleaning the layers not used by the
     * current animation.
     */
    private void RenderLayers() {
        /**
         * Some animations have more than one layer (think of npcs)
         * so this is needed to render each layer of the animation
         * since we cannot have more than one SpriteRenderer attached
         * to a single game object
         */
        foreach (var frame in currentAction.frames) {
            for (int i = 0; i < frame.layers.Length; i++) {
                var layer = frame.layers[i];
                var sprite = sprites[layer.index];

                Layers.TryGetValue(i, out var spriteRenderer);

                if (spriteRenderer == null) {
                    var go = new GameObject($"Layer{i}");
                    spriteRenderer = go.AddComponent<SpriteRenderer>();
                    //spriteRenderer.flipY = true;
                    spriteRenderer.sortingOrder = SpriteOrder;

                    spriteRenderer.transform.SetParent(gameObject.transform, false);
                }

                CalculateSpritePositionScale(layer, sprite, out Vector3 scale, out Vector3 newPos, out Quaternion rotation);

                spriteRenderer.transform.localRotation = rotation;
                spriteRenderer.transform.localPosition = newPos;
                spriteRenderer.transform.localScale = scale;

                if (!Layers.ContainsKey(i)) {
                    Layers.Add(i, spriteRenderer);
                }
            }
        }
    }

    public void UpdateSpriteFrame() {
        if (currentFrame >= currentAction.frames.Length) {
            if (AnimationHelper.IsLoopingMotion(CurrentMotion)) {
                currentFrame = 0;
                currentFrameTime = currentAction.delay / 1000f * AnimSpeed;
            } else {
                return;
            }
        }
        var frame = currentAction.frames[currentFrame];

        if (frame.soundId > -1 && frame.soundId < currentACT.sounds.Length && !isPaused) {
            // TODO sounds
        }

        // We need this mesh collider in order to have the raycast to hit the sprite
        MeshCache.TryGetValue(frame, out Mesh mesh);
        if (mesh == null) {
            mesh = SpriteMeshBuilder.BuildColliderMesh(frame, sprites);
            MeshCache.Add(frame, mesh);
        }
        meshCollider.sharedMesh = mesh;

        // Iterate each frame layer and do positioning magic
        for (int i = 0; i < frame.layers.Length; i++) {
            Layers.TryGetValue(i, out var spriteRenderer);

            if (spriteRenderer) {
                var layer = frame.layers[i];
                var sprite = sprites[layer.index];

                spriteRenderer.sprite = sprite;

                CalculateSpritePositionScale(layer, sprite, out Vector3 scale, out Vector3 newPos, out Quaternion rotation);
                spriteRenderer.transform.localRotation = rotation;
                spriteRenderer.transform.localPosition = newPos;
                spriteRenderer.transform.localScale = scale;
            }
        }
    }

    private void CalculateSpritePositionScale(ACT.Layer layer, Sprite sprite, out Vector3 scale, out Vector3 newPos, out Quaternion rotation) {
        rotation = Quaternion.Euler(0, 0, -layer.angle);
        scale = new Vector3(layer.scale.x * (layer.isMirror ? -1 : 1), -(layer.scale.y), 1);
        var offsetX = (Mathf.RoundToInt(sprite.rect.width) % 2 == 1) ? 0.5f : 0f;
        var offsetY = (Mathf.RoundToInt(sprite.rect.height) % 2 == 1) ? 0.5f : 0f;

        newPos = new Vector3(layer.pos.x - offsetX, -(layer.pos.y) + offsetY) / sprite.pixelsPerUnit;
    }

    public void ChangeMotion(SpriteMotion nextMotion, bool forceUpdate = false) {
        if (CurrentMotion == nextMotion && !forceUpdate)
            return;

        CurrentMotion = nextMotion;
        State = AnimationHelper.GetStateForMotion(CurrentMotion);
        currentFrame = 0;

        if (!Entity.IsReady)
            return;

        foreach (var child in Children) {
            child.ChangeMotion(nextMotion);
        }

        var action = AnimationHelper.GetMotionIdForSprite(Type, nextMotion);
        if (action < 0 || action > currentACT.actions.Length) {
            action = 0;
        }

        ChangeAction(action);
        isPaused = false;
        isDirty = true;

        if (Type == EntityType.PC) {
            if (CurrentMotion == SpriteMotion.Idle || CurrentMotion == SpriteMotion.Sit) {
                //currentFrame = (int) HeadF
                UpdateSpriteFrame();
                isPaused = true;
            } else {
                //HeadFacing 
            }
        }
    }

    public void ChildSetFrameData(int actionIndex, int angleIndex, int newCurrentFrame) {
        currentActionIndex = actionIndex;
        currentAngleIndex = angleIndex;

        //if(!isInitialized)
        //    return;

        currentAction = currentACT.actions[currentActionIndex + currentAngleIndex];
        currentFrame = newCurrentFrame;
        UpdateSpriteFrame();
        ChildUpdate();
    }

    public void ChildUpdate() {
        var parentAnchor = Parent.GetAnimationAnchor();
        var ourAnchor = GetAnimationAnchor();

        var diff = parentAnchor - ourAnchor;

        transform.localPosition = new Vector3(diff.x, -diff.y, 0f) / SPR.PIXELS_PER_UNIT;
    }

    private void InitShadow() {
        if (_ViewerType != ViewerType.BODY) return;

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

        var shader = Shader.Find("Unlit/CustomSpriteShader");
        var mat = new Material(shader);
        mat.color = new Color(1f, 1f, 1f, 0.5f);
        mat.mainTexture = spriteRenderer.sprite.texture;

        spriteRenderer.material = mat;
        spriteRenderer.sortingOrder = -1;
    }

    public Vector2 GetAnimationAnchor() {
        var frame = currentAction.frames[currentFrame];
        if (frame.pos.Length > 0)
            return frame.pos[0];
        if (_ViewerType == ViewerType.HEAD && (State == SpriteState.Idle || State == SpriteState.Sit))
            return frame.pos[currentFrame];
        return Vector2.zero;
    }
}

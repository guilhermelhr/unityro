using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class EntityViewer : MonoBehaviour {

    public Entity Entity;
    public EntityType Type;
    public EntityViewer Parent;
    public ViewerType _ViewerType;
    public SpriteMotion CurrentMotion;
    private int currentFrame;
    public Dictionary<int, SpriteRenderer> Children = new Dictionary<int, SpriteRenderer>();
    public float SpriteOffset;
    public int HeadDirection;
    public int SpriteOrder;

    //private int CurrentMotion;
    private int xSize;
    private int ySize;

    private SpriteRenderer renderer;
    private SpriteAction ActionTable;

    private ACT currentACT;
    private SPR currentSPR;
    private ACT.Action currentAction;
    private int currentActionIndex;
    private int currentAngleIndex;
    private int maxFrame;
    private float currentFrameTime;
    private float AnimSpeed = 1;

    void Start() {
        //renderer = gameObject.AddComponent<SPRRenderer>();
        renderer = gameObject.AddComponent<SpriteRenderer>();
        renderer.flipY = true;
        var path = _ViewerType == ViewerType.BODY ? DBManager.GetBodyPath((Job)Entity.Job, Entity.Sex) : DBManager.GetHeadPath(Entity.Hair, Entity.Sex);
        currentSPR = FileManager.Load(path + ".spr") as SPR;
        currentACT = FileManager.Load(path + ".act") as ACT;

        ChangeAngle(0);
        ChangeAction(0);
        //renderer.setSPR(currentSPR, 0, 0);
    }

    void Update() {
        if(!Entity.IsReady ||
            currentACT == null)
            return;
        if(ActionTable == null)
            ActionTable = Entity.ActionTable;
        if(currentAction == null)
            ChangeAction(0);

        bool is4dir = AnimationHelper.IsFourDirectionAnimation(Type, CurrentMotion);
        int angleIndex;
        if(is4dir) {
            angleIndex = AnimationHelper.GetFourDirectionSpriteIndexForAngle(Entity.Direction, 360 - ROCamera.Instance.Rotation);
        } else {
            angleIndex = AnimationHelper.GetSpriteIndexForAngle(Entity.Direction, 360 - ROCamera.Instance.Rotation);
        }

        if(currentAngleIndex != angleIndex) {
            ChangeAngle(angleIndex);
        }

        //var animationId = CalcAnimation(Entity, currentAction, "body", 0);
        //var animation = currentAction.animations[animationId];

        //foreach(var layer in animation.layers) {
        //    var index = layer.index < 0 ? 0 : layer.index;
        //    renderer.sprite = currentSPR.GetSprites()[index];
        //}
    }

    private void ChangeAngle(int newAngleIndex) {
        if(currentACT == null) return;
        currentAngleIndex = newAngleIndex;
        //if (!isInitialized) return;
        currentAction = currentACT.actions[currentActionIndex + currentAngleIndex];
        maxFrame = currentAction.animations.Length - 1;

        //renderer.setFrameLimits(currentAction.animations[0].layers[0].index, currentAction.animations[maxFrame].layers[0].index);
    }

    private void ChangeAction(int newActionIndex) {
        if(currentACT == null) return;
        currentActionIndex = newActionIndex;
        //if (!isInitialized) return;
        currentAction = currentACT.actions[currentActionIndex + currentAngleIndex];
        maxFrame = currentAction.animations.Length - 1;
        currentFrameTime = currentAction.delay / 1000f * AnimSpeed; //reset current frame time
        
        var animationId = CalcAnimation(Entity, currentAction, "body", 0);
        var animation = currentAction.animations[animationId];

        /**
         * Some animations have more than one layer (think of npcs)
         * so this is needed to render each layer of the animation
         * since we cannot have more than one SpriteRenderer attached
         * to a single game object
         */
        for(int i = 0; i < animation.layers.Length; i++) {
            Children.TryGetValue(i, out var layerGameObject);
            if (layerGameObject == null) {
                var go = new GameObject($"Layer{i}");
                layerGameObject = go.AddComponent<SpriteRenderer>();
                layerGameObject.flipY = true;
                go.transform.SetParent(gameObject.transform, false);
                Children.Add(i, layerGameObject);
            }
            var layer = animation.layers[i];
            layerGameObject.sprite = currentSPR.GetSprites()[layer.index < 0 ? 0 : layer.index];
        }
    }

    private void RenderLayer() {

    }

    public void ChangeMotion(SpriteMotion nextMotion, bool forceUpdate = false) {
        if(CurrentMotion == nextMotion && !forceUpdate)
            return;

        CurrentMotion = nextMotion;
        currentFrame = 0;

        if(!Entity.IsReady)
            return;

        var action = AnimationHelper.GetMotionIdForSprite(Type, nextMotion);
        if(action < 0 || action > currentACT.actions.Length) {
            action = 0;
        }

        ChangeAction(action);
    }

    private int CalcAnimation(Entity entity, ACT.Action act, string entityType, float tick) {
        if(entityType == "shadow") {
            return 0;
        }

        var animation = entity.Animation;
        var animCount = act.animations.Length;
        var animSize = animCount;
        var isIdle = (CurrentMotion == SpriteMotion.Idle || CurrentMotion == SpriteMotion.Sit);
        var delay = getAnimationDelay(entityType, entity, act);
        var headDir = 0;
        var anim = 0;

        // GeEntit doridori
        if(entityType == "head" && Entity.Type == EntityType.PC && isIdle) {
            return headDir;
        }

        // Don't play, so stop at current frame
        if(!animation.play) {
            return Math.Min(animation.frame, animSize - 1);
        }

        // If hat/hair, divide to 3 since there is doridori include
        // TODO: fixed, just on IDLE and SIT ?
        if(entityType == "head" && isIdle) {
            animCount = (int)Mathf.Floor(animCount / 3);
            headDir = entity.HeadDir;
        }

        if(animation.repeat) {
            anim = (int)Mathf.Floor(tick / delay);

            // TODO free sound
            // entity.sound.freeOnAnimationEnd(anim, animCount);

            anim %= animCount;
            anim += animCount * headDir; // get rid of doridori
            anim += animation.frame;     // don't forget the previous frame
            anim %= animSize;            // avoid overflow

            return anim;
        }


        // No repeat
        // Math.min(tick / delay | 0, animCount || animCount -1)
        anim = (
            (int)Mathf.Min(tick / delay, animCount - 1)  // Avoid an error if animation = 0, search for -1 :(
            + animCount * headDir // get rid of doridori
            + animation.frame     // previous frame
        );

        if(entityType == "body" && anim >= animSize - 1) {
            animation.frame = anim = animSize - 1;
            animation.play = false;
            if(animation.next != null) {
                //SetAction(animation.next);
            }
        }

        return Math.Min(anim, animCount - 1);
    }

    private float getAnimationDelay(string entityType, Entity entity, ACT.Action act) {
        return 100;
    }

    private void InitShadow() {
        if(_ViewerType != ViewerType.BODY) return;

        var shadow = new GameObject("Shadow");
        shadow.layer = LayerMask.NameToLayer("Characters");
        shadow.transform.SetParent(transform, false);
        shadow.transform.localPosition = Vector3.zero;
        shadow.transform.localScale = new Vector3(Entity.ShadowSize, Entity.ShadowSize, Entity.ShadowSize);

        SPR sprite = FileManager.Load("data/sprite/shadow.spr") as SPR;
        ACT act = FileManager.Load("data/sprite/shadow.act") as ACT;

        sprite.SwitchToRGBA();

        //if(Mathf.Approximately(0, ShadowSize))
        //    ShadowSize = 0.5f;

        var spriteRenderer = shadow.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite.GetSprites()[0];
        //shadowSprite = sprite;

        //var shader = Shader.Find("Unlit/TestSpriteShader");
        //var mat = new Material(shader);
        //mat.SetFloat("_Offset", 0.4f);
        //mat.color = new Color(1f, 1f, 1f, 0.5f);
        //sprite.material = mat;

        spriteRenderer.sortingOrder = -1;

        //SpriteAnimator.Shadow = go;
        //SpriteAnimator.ShadowSortingGroup = go.AddComponent<SortingGroup>();
        //SpriteAnimator.ShadowSortingGroup.sortingOrder = -20001;
        //if(SpriteAnimator.State == SpriteState.Sit)
        //    go.SetActive(false);
    }

    public enum ViewerType {
        BODY, HEAD, SHADOW, LAYER
    }
}

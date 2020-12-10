using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class EntityViewer : MonoBehaviour {

    public Entity Entity;
    public EntityViewer Parent;
    public ViewerType _ViewerType;
    public EntityState State;
    public List<EntityViewer> Children = new List<EntityViewer>();
    public float SpriteOffset;
    public int HeadDirection;
    public int SpriteOrder;

    private int CurrentAction;
    private int xSize;
    private int ySize;

    private EntityBody body;
    private SPRRenderer renderer;
    private Animation _Animation;
    private SpriteAction ActionTable;

    void Start() {
        renderer = gameObject.AddComponent<SPRRenderer>();
    }

    void Update() {
        if (ActionTable == null) {
            ActionTable = Entity.ActionTable;
        }

        // Animation change ! Get it now
        //if(anim.save != null && anim.delay < Time.time) {
        //    SetAction(anim.save);
        //}

        // Avoid look up, render as IDLE all not supported frames
        //var action = actionId < 0 ? actionIds.IDLE : actionId;
        //TODO RO camera
        //var direction = ((int)ROCamera.direction + Entity.Direction + 8) % 8;
        //var behind = direction > 1 && direction < 6;
        var currentSPR = FileManager.Load(DBManager.GetBodyPath(0, 0) + ".spr") as SPR;
        var currentACT = FileManager.Load(DBManager.GetBodyPath(0, 0) + ".act") as ACT;
        var action = currentACT.actions[(
                (Entity.Action * 8) + // Entity Action (IDLE, WALK, ETC)
                ((int)ROCamera.direction + Entity.Direction + 8) % 8 // Direction
                ) % currentACT.actions.Length]; // Avoid overflow
        int animationID = CalcAnimation(Entity, action, "body", Time.time);
        var animation = action.animations[animationID];

        renderer.setSPR(currentSPR, animation.layers[0].index, animation.layers[animation.layers.Length-1].index);
    }

    private int CalcAnimation(Entity entity, ACT.Action act, string entityType, float tick) {
        if (entityType == "shadow") {
            return 0;
        }

        var ACTION = entity.ActionTable;
        var action = entity.Action;
        var animation = entity.Animation;
        var animCount = act.animations.Length;
        var animSize = animCount;
        var isIdle = (action == ACTION.IDLE || action == ACTION.SIT);
        var delay = getAnimationDelay(entityType, entity, act);
        var headDir = 0;
        var anim = 0;

        // GeEntit doridori
        if (entityType == "head" && Entity.Type == EntityType.PC && isIdle) {
            return headDir;
        }

        // Don't play, so stop at current frame
        if (!animation.play) {
            return Math.Min(animation.frame, animSize - 1);
        }

        // If hat/hair, divide to 3 since there is doridori include
        // TODO: fixed, just on IDLE and SIT ?
        if (entityType == "head" && isIdle) {
            animCount = (int)Mathf.Floor(animCount / 3);
            headDir = entity.HeadDir;
        }

        if (animation.repeat) {
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

        if (entityType == "body" && anim >= animSize - 1) {
            animation.frame = anim = animSize - 1;
            animation.play = false;
            if (animation.next != null) {
                SetAction(animation.next);
            }
        }

        return Math.Min(anim, animCount - 1);
    }

    //TODO implement
    private int getAnimationDelay(string entityType, Entity entity, ACT.Action act) {
        return 0;
    }

    private void InitShadow() {
        if (_ViewerType != ViewerType.BODY) return;

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

    public void SetAction(Animation option) {
        if (option.delay != 0) {
            _Animation.delay = option.delay;
            option.delay = 0;
            _Animation.save = option;
        } else {
            var objecttype = Entity.Type;

            // Know attack frame based on weapon type
            if (option.action == ActionTable.ATTACK) {
                if (objecttype == EntityType.PC) {
                    //int attack = DBManager.getWeaponAction(entity.weapon, entity._job, entity._sex);
                    //option.action = new int[] { actionIds.ATTACK1, actionIds.ATTACK2, actionIds.ATTACK3 }[attack];
                }

                // No action loaded yet
                if (option.action == -2) {
                    option.action = ActionTable.ATTACK1;
                }
            }

            CurrentAction = option.action == -1 ? ActionTable.IDLE : option.action;
            _Animation.tick = Time.time;
            _Animation.delay = 0;
            _Animation.frame = option.frame;
            _Animation.repeat = option.repeat;
            _Animation.play = option.play;
            _Animation.next = option.next;
            _Animation.save = null;

            // Reset sounds
            // TODO
        }
    }

    public void UpdateBody(Job job, int sex) {
        var path = DBManager.GetBodyPath(job, sex);
        ACT act = FileManager.Load(path + ".act") as ACT;
        SPR spr = FileManager.Load(path + ".spr") as SPR;
        //body = new EntityBody(act, spr);

        //renderer = gameObject.GetOrAddComponent<SPRRenderer>();
        //renderer.setSPR(spr, 0, 0);
        //Children.Where(t => t.ViewerType == Type.HEAD).First().UpdateHead(job, sex);
    }

    public void UpdateHead(Job job, int sex) {
        var path = DBManager.GetHeadPath((int)job, sex);
        ACT act = FileManager.Load(path + ".act") as ACT;
        SPR spr = FileManager.Load(path + ".spr") as SPR;
        //body = new EntityBody(act, spr);

        //transform.position = new Vector3(0.23f, -0.08f, 0);
        //gameObject.AddComponent<SPRRenderer>().setSPR(spr, 0, 0);
    }

    public class EntityBody {
        public EntityBody(ACT act, SPR spr) {
            this.act = act;
            this.spr = spr;
        }

        public ACT act { get; }
        public SPR spr { get; }
    }

    public enum ViewerType {
        BODY, HEAD, SHADOW
    }
}

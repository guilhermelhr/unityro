using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityViewer : MonoBehaviour {

    public Entity Entity;
    public EntityViewer Parent;
    public EntityState State;
    public EntityViewer.Type ViewerType;
    public List<EntityViewer> Children = new List<EntityViewer>();
    public float SpriteOffset;
    public int HeadDirection;
    public int SpriteOrder;

    private int CurrentAction;
    private int xSize;
    private int ySize;

    private Animation _Animation;
    private SpriteAction ActionTable;


    void Start() {

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
        var currentACT = FileManager.Load(DBManager.GetBodyPath(0, 0)) as ACT;
        var action = currentACT.actions[(
                (Entity.Action * 8) + // Entity Action (IDLE, WALK, ETC)
                ((int)ROCamera.direction + Entity.Direction + 8) % 8 // Direction
                ) % currentACT.actions.Length]; // Avoid overflow
        int animationID = CalcAnimation(Entity, action, "body");
    }

    private int CalcAnimation(Entity entity, ACT.Action action, string v) {


        return 0;
    }

    private void InitShadow() {
        if (ViewerType != Type.BODY) return;

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

        gameObject.AddComponent<SPRRenderer>().setSPR(spr, 0, 0);
        Children.Where(t => t.ViewerType == Type.HEAD).First().UpdateHead(job, sex);
    }

    public void UpdateHead(Job job, int sex) {
        var path = DBManager.GetHeadPath((int)job, sex);
        ACT act = FileManager.Load(path + ".act") as ACT;
        SPR spr = FileManager.Load(path + ".spr") as SPR;
        //body = new EntityBody(act, spr);

        transform.position = new Vector3(0.23f, -0.08f, 0);
        gameObject.AddComponent<SPRRenderer>().setSPR(spr, 0, 0);
    }

    public class Animation {
        public int action = -1;
        public float tick = 0;
        public int frame = 0;
        public bool repeat = true;
        public bool play = true;
        public float delay = 0;
        public Animation save = null;
        public Animation next = null;
    }

    public class EntityBody {
        public EntityBody(ACT act, SPR spr) {
            this.act = act;
            this.spr = spr;
        }

        public ACT act { get; }
        public SPR spr { get; }
    }

    public enum Type {
        BODY, HEAD, SHADOW
    }
}

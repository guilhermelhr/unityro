using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class EntityViewer : MonoBehaviour {

    public Entity Entity;
    public EntityState State;
    public EntityViewer.Type ViewerType;
    public List<EntityViewer> Children = new List<EntityViewer>();
    public float SpriteOffset;
    public int HeadDirection;
    public EntityViewer Parent;
    public int SpriteOrder;

    private Action actionIds;
    private int actionId;
    private Animation anim;
    private int xSize;
    private int ySize;

    private EntityBody body;

    void Start() {
        //anim = new Animation();
        //InitActionIds();
        //InitShadow();

        var MeshFilter = gameObject.AddComponent<MeshFilter>();
        var MeshRenderer = gameObject.AddComponent<MeshRenderer>();
        var MeshCollider = gameObject.AddComponent<MeshCollider>();
        var SortingGroup = gameObject.GetOrAddComponent<SortingGroup>();

        MeshRenderer.sortingOrder = SpriteOrder;
        MeshRenderer.receiveShadows = false;
        MeshRenderer.lightProbeUsage = LightProbeUsage.Off;
        MeshRenderer.shadowCastingMode = ShadowCastingMode.Off;

        var shader = Shader.Find("Unlit/CustomSpriteShader");
        var material = new Material(shader);
        //TODO
        //material.mainTexture = 

        //if(Mathf.Approximately(0, SpriteOffset))
        //    material.SetFloat("_Offset", SpriteData.Size / 125f);
        //else
        //    material.SetFloat("_Offset", SpriteOffset);

        //MeshRenderer.material = material;
    }

    void Update() {
        // Animation change ! Get it now
        //if(anim.save != null && anim.delay < Time.time) {
        //    SetAction(anim.save);
        //}

        // Avoid look up, render as IDLE all not supported frames
        //var action = actionId < 0 ? actionIds.IDLE : actionId;
        //TODO RO camera
        //var direction = ((int)ROCamera.direction + Entity.Direction + 8) % 8;
        //var behind = direction > 1 && direction < 6;
    }

    private void CalcAnimation() {

    }

    private void InitShadow() {
        if(ViewerType != Type.BODY) return;

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

    private void InitActionIds() {
        if(ViewerType != Type.BODY) return;
        actionIds = new Action();
        switch(Entity.Type) {
            // Define action, base on type
            case EntityType.PC:
                actionIds.IDLE = 0;
                actionIds.WALK = 1;
                actionIds.SIT = 2;
                actionIds.PICKUP = 3;
                actionIds.READYFIGHT = 4;
                actionIds.ATTACK1 = 5;
                actionIds.HURT = 6;
                actionIds.FREEZE = 7;
                actionIds.DIE = 8;
                actionIds.FREEZE2 = 9;
                actionIds.ATTACK2 = 10;
                actionIds.ATTACK3 = 11;
                actionIds.SKILL = 12;
                break;

            // Mob action
            case EntityType.MOB:
                actionIds.IDLE = 0;
                actionIds.WALK = 1;
                actionIds.ATTACK = 2;
                actionIds.HURT = 3;
                actionIds.DIE = 4;
                break;

            case EntityType.PET:
                actionIds.IDLE = 0;
                actionIds.WALK = 1;
                actionIds.ATTACK = 2;
                actionIds.HURT = 3;
                actionIds.DIE = 4;
                actionIds.SPECIAL = 5;
                actionIds.PERF1 = 6;
                actionIds.PERF2 = 7;
                actionIds.PERF3 = 8;
                break;

            // NPC action
            case EntityType.NPC:
                actionIds.IDLE = 0;
                // For those NPC that move with unitwalk scriptcommand
                actionIds.WALK = 1;
                break;

            // When you see a warp with /effect, it's 3 times bigger.
            // TODO: put it somewhere else
            case EntityType.WARP:
                xSize = 20;
                ySize = 20;
                break;

            // Homunculus
            case EntityType.HOM:
                actionIds.IDLE = 0;
                actionIds.WALK = 1;
                actionIds.ATTACK = 2;
                actionIds.HURT = 3;
                actionIds.DIE = 4;
                actionIds.ATTACK2 = 5;
                actionIds.ATTACK3 = 6;
                actionIds.ACTION = 7;
                break;

            //TODO: define others Entities ACTION
            case EntityType.ELEM:
                break;
        }
    }

    public void SetAction(Animation option) {
        if(option.delay != 0) {
            anim.delay = option.delay;
            option.delay = 0;
            anim.save = option;
        } else {
            var objecttype = Entity.Type;

            // Know attack frame based on weapon type
            if(option.action == actionIds.ATTACK) {
                if(objecttype == EntityType.PC) {
                    //int attack = DBManager.getWeaponAction(entity.weapon, entity._job, entity._sex);
                    //option.action = new int[] { actionIds.ATTACK1, actionIds.ATTACK2, actionIds.ATTACK3 }[attack];
                }

                // No action loaded yet
                if(option.action == -2) {
                    option.action = actionIds.ATTACK1;
                }
            }

            actionId = option.action == -1 ? actionIds.IDLE : option.action;
            anim.tick = Time.time;
            anim.delay = 0;
            anim.frame = option.frame;
            anim.repeat = option.repeat;
            anim.play = option.play;
            anim.next = option.next;
            anim.save = null;

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

    public class Action {
        public int IDLE = 0,
            ATTACK = -2,
            WALK = -1,
            SIT = -1,
            PICKUP = -1,
            READYFIGHT = -1,
            FREEZE = -1,
            HURT = -1,
            DIE = -1,
            FREEZE2 = -1,
            ATTACK1 = -1,
            ATTACK2 = -1,
            ATTACK3 = -1,
            SKILL = -1,
            ACTION = -1,
            SPECIAL = -1,
            PERF1 = -1,
            PERF2 = -1,
            PERF3 = -1;
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

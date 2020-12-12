using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class EntityViewer : MonoBehaviour {

    public Entity Entity;
    public EntityType Type;
    public EntityViewer Parent;
    public ViewerType _ViewerType;
    public SpriteMotion  CurrentMotion;
    public List<EntityViewer> Children = new List<EntityViewer>();
    public float SpriteOffset;
    public int HeadDirection;
    public int SpriteOrder;

    //private int CurrentMotion;
    private int xSize;
    private int ySize;

    private SPRRenderer renderer;
    private Animation _Animation;
    private SpriteAction ActionTable;

    private ACT currentACT;
    private SPR currentSPR;
    private ACT.Action currentAction;
    private int currentActionIndex;
    private int currentAngleIndex;
    private Direction Direction;
    private int maxFrame;
    private float currentFrameTime;
    private float AnimSpeed = 1;

    void Start() {
        renderer = gameObject.AddComponent<SPRRenderer>();
        currentSPR = FileManager.Load(DBManager.GetBodyPath(0, 0) + ".spr") as SPR;
        currentACT = FileManager.Load(DBManager.GetBodyPath(0, 0) + ".act") as ACT;

        ChangeAngle(0);
        renderer.setSPR(currentSPR, 0, 0);
    }

    void Update() {
        if (ActionTable == null) {
            ActionTable = Entity.ActionTable;
        }

        if(currentAction == null)
            ChangeAction(0);

        bool is4dir = AnimationHelper.IsFourDirectionAnimation(Type, CurrentMotion);
        int angleIndex;
        if (is4dir) {
            angleIndex = AnimationHelper.GetFourDirectionSpriteIndexForAngle(Direction, 360 - ROCamera.Instance.Rotation);
        } else {
            angleIndex = AnimationHelper.GetSpriteIndexForAngle(Direction, 360 - ROCamera.Instance.Rotation);
        }

        if (currentAngleIndex != angleIndex) {
            ChangeAngle(angleIndex);
        }
    }

    private void ChangeAngle(int newAngleIndex) {
        currentAngleIndex = newAngleIndex;
        //if (!isInitialized) return;
        currentAction = currentACT.actions[currentActionIndex + currentAngleIndex];
        maxFrame = currentAction.animations.Length - 1;

        renderer.setFrameLimits(currentAction.animations[0].layers[0].index, currentAction.animations[maxFrame].layers[0].index);
    }

    private void ChangeAction(int newActionIndex) {
        currentActionIndex = newActionIndex;
        //if (!isInitialized) return;
        currentAction = currentACT.actions[currentActionIndex + currentAngleIndex];
        maxFrame = currentAction.animations.Length - 1;
        currentFrameTime = currentAction.delay / 1000f * AnimSpeed; //reset current frame time

        renderer.setFrameLimits(currentAction.animations[0].layers[0].index, currentAction.animations[maxFrame].layers[0].index);
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

    public enum ViewerType {
        BODY, HEAD, SHADOW
    }
}

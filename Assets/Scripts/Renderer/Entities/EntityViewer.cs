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
    private int currentFrame;
    public float SpriteOffset;
    public int HeadDirection;
    public int SpriteOrder;
    private SpriteState State = SpriteState.Idle;

    private MeshFilter MeshFilter;
    private MeshCollider MeshCollider;
    private MeshRenderer MeshRenderer;
    private SortingGroup SortingGroup;
    private Shader shader;
    private Material material;
    private Dictionary<int, Mesh> meshCache;
    private Dictionary<int, Mesh> colliderCache;

    //private SpriteRenderer renderer;

    private Coroutine AnimationCoroutine;
    private Sprite[] sprites;
    private ACT currentACT;
    private SPR currentSPR;
    private ACT.Action currentAction;
    private int currentActionIndex;
    private int currentAngleIndex;
    private int maxFrame;
    private float currentFrameTime;
    private float AnimSpeed = 1;
    private bool isPaused;

    void Start() {
        var path = _ViewerType == ViewerType.BODY ? DBManager.GetBodyPath((Job)Entity.Job, Entity.Sex) : DBManager.GetHeadPath(Entity.Hair, Entity.Sex);
        currentSPR = FileManager.Load(path + ".spr") as SPR;
        currentACT = FileManager.Load(path + ".act") as ACT;
        sprites = currentSPR.GetSprites();

        MeshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer = gameObject.AddComponent<MeshRenderer>();

        MeshRenderer.sortingOrder = SpriteOrder;
        MeshCollider = gameObject.AddComponent<MeshCollider>();

        SortingGroup = gameObject.GetOrAddComponent<SortingGroup>();

        MeshRenderer.receiveShadows = false;
        MeshRenderer.lightProbeUsage = LightProbeUsage.Off;
        MeshRenderer.shadowCastingMode = ShadowCastingMode.Off;

        material = new Material(Shader.Find("Unlit/CustomSpriteShader"));

        meshCache = SpriteMeshCache.GetMeshCacheForSprite(currentSPR.filename);
        colliderCache = SpriteMeshCache.GetColliderCacheForSprite(currentSPR.filename);
    }

    void Update() {
        if(!Entity.IsReady ||
            currentACT == null)
            return;

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

        if(currentActionIndex != Entity.Action) {
            ChangeAction(Entity.Action);
        }

        UpdateSpriteFrame();
    }

    private void ChangeAngle(int newAngleIndex) {
        if(currentACT == null) return;
        currentAngleIndex = newAngleIndex;
        //if (!isInitialized) return;
        currentAction = currentACT.actions[currentActionIndex + currentAngleIndex];
        maxFrame = currentAction.motions.Length - 1;

        //if(AnimationCoroutine != null)
        //    StopCoroutine(AnimationCoroutine);
        //AnimationCoroutine = StartCoroutine(AnimateMotion());
    }

    private void ChangeAction(int newActionIndex) {
        if(currentACT == null) return;
        Entity.Action = newActionIndex;
        currentActionIndex = newActionIndex;
        //if (!isInitialized) return;
        currentAction = currentACT.actions[currentActionIndex + currentAngleIndex];
        maxFrame = currentAction.motions.Length - 1;
        currentFrameTime = currentAction.delay / 1000f * AnimSpeed; //reset current frame time

        /**
         * Some animations have more than one layer (think of npcs)
         * so this is needed to render each layer of the animation
         * since we cannot have more than one SpriteRenderer attached
         * to a single game object
         */
        foreach(var motion in currentAction.motions) {
            for(int i = 0; i < motion.layers.Length; i++) {
                var mesh = SpriteMeshBuilder.BuildSpriteMesh(motion.layers[i], sprites);
                var collider = SpriteMeshBuilder.BuildColliderMesh(motion.layers[i], sprites);

                //Children.TryGetValue(i, out var spriteRenderer);

                //var layer = motion.layers[i];

                //if(layer.index < 0) continue;

                //if(spriteRenderer == null) {
                //    var go = new GameObject($"Layer{i}");
                //    spriteRenderer = go.AddComponent<SpriteRenderer>();
                //    spriteRenderer.flipY = true;
                //    //spriteRenderer.material = new Material(Shader.Find("Unlit/CustomSpriteShader"));

                //    go.transform.SetParent(gameObject.transform, false);

                //    Children.Add(i, spriteRenderer);
                //}

                //spriteRenderer.flipX = layer.isMirror != 0;
            }
        }

        //if(AnimationCoroutine != null)
        //    StopCoroutine(AnimationCoroutine);
        //AnimationCoroutine = StartCoroutine(AnimateMotion());
    }

    //private IEnumerator AnimateMotion() {
    //    foreach(var motion in currentAction.motions) {
    //        //var sprite = sprites[layer.index];
    //        //spriteRenderer.material.mainTexture = sprite.texture;
    //        ////spriteRenderer.material.SetFloat("_Offset", sprite.rect.width / 125f);
    //        //spriteRenderer.sprite = sprite;
    //        //spriteRenderer.flipX = layer.isMirror != 0;
    //        yield return new WaitForSeconds(currentAction.delay / 1000);
    //    }

    //    if(AnimationHelper.IsLoopingMotion(CurrentMotion)) {
    //        yield return AnimateMotion();
    //    } else {
    //        yield return null;
    //    }
    //}

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

    private Mesh GetColliderForFrame() {
        var id = ((currentActionIndex + currentAngleIndex) << 8) + currentFrame;

        if(colliderCache.TryGetValue(id, out var mesh))
            return mesh;


        var newMesh = SpriteMeshBuilder.BuildColliderMesh(currentAction.motions[id], sprites);

        colliderCache.Add(id, newMesh);

        return newMesh;
    }

    private Mesh GetMeshForFrame() {
        var id = ((currentActionIndex + currentAngleIndex) << 8) + currentFrame;

        if(meshCache.TryGetValue(id, out var mesh))
            return mesh;

        //Debug.Log("Building new mesh for " + name);

        var newMesh = SpriteMeshBuilder.BuildSpriteMesh(currentAction.motions[id], sprites);

        meshCache.Add(id, newMesh);

        return newMesh;
    }

    public void AdvanceFrame() {
        if(!isPaused)
            currentFrame++;
        if(currentFrame > maxFrame) {
            var nextMotion = AnimationHelper.GetMotionForState(State);
            if(nextMotion != CurrentMotion)
                ChangeMotion(nextMotion);
            else {
                if(State != SpriteState.Dead)
                    currentFrame = 0;
                else {
                    currentFrame = maxFrame;
                    isPaused = true;
                }
            }
        }

        if(currentFrameTime < 0)
            currentFrameTime += (float)currentAction.delay / 1000f * AnimSpeed;

        //if(!isPaused)
        //    isDirty = true;
    }

    private void UpdateSpriteFrame() {
        if(currentFrame >= currentAction.motions.Length) {
            Debug.LogWarning($"Current frame is {currentFrame}, max frame is {maxFrame}, but actual frame max is {currentAction.motions.Length}");
            return;
        }
        var frame = currentAction.motions[currentFrame];
        var layer = frame.layers.Where(t => t.index >= 0).FirstOrDefault();

        if(layer.index < 0)
            return;
        //if(frame.Sound > -1 && frame.Sound < SpriteData.Sounds.Length && !isPaused) {
        //    var sound = SpriteData.Sounds[frame.Sound];
        //    if(sound != null && AudioSource != null) {
        //        AudioSource.clip = sound;
        //        AudioSource.Play();
        //    }
        //}

        var mesh = GetMeshForFrame();
        var cMesh = GetColliderForFrame();

        MeshFilter.sharedMesh = null;
        MeshFilter.sharedMesh = mesh;
        MeshCollider.sharedMesh = null;
        MeshCollider.sharedMesh = cMesh;

        material.mainTexture = sprites[layer.index].texture;
        MeshRenderer.material = null;
        MeshRenderer.material = material;
    }
}

using Assets.Scripts.Renderer.Sprite;
using ROIO.Models.FileTypes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public enum CursorAction {
    DEFAULT = 0,
    TALK = 1,
    CLICK = 2,
    LOCK = 3,
    ROTATE = 4,
    ATTACK = 5,
    WARP = 7,
    INVALID = 8,
    PICK = 9,
    TARGET = 10
}

public class CursorRenderer : MonoBehaviour {

    private GameManager GameManager;
    private Camera CursorCamera => GameManager.CursorCamera;

    private ACT CurrentAct;
    private Sprite[] Sprites;

    private MeshCollider MeshCollider;
    private MeshFilter MeshFilter;
    private MeshRenderer MeshRenderer;
    private Material SpriteMaterial;
    private SortingGroup SortingGroup;

    private Dictionary<ACT.Frame, Mesh> MeshCache = new Dictionary<ACT.Frame, Mesh>();

    private CursorAction type;

    [SerializeField] public int CurrentActionIndex;
    [SerializeField] private int CurrentFrame;
    [SerializeField] private float CameraPlaneOffset = 0.02f;
    [SerializeField] private bool isReady = false;
    private long AnimationStart;
    private ACT.Action CurrentAction;

    private void Awake() {
        DontDestroyOnLoad(this);
        GameManager = FindObjectOfType<GameManager>();
    }

    void Start() {
        gameObject.layer = LayerMask.NameToLayer("Cursor");
        var spriteData = Addressables.LoadAssetAsync<SpriteData>("data/sprite/cursors.asset").WaitForCompletion();
        var atlas = Addressables.LoadAssetAsync<Texture2D>("data/sprite/cursors.png").WaitForCompletion();
        SpriteMaterial = Resources.Load("Materials/Sprites/SpriteMaterial") as Material;

        Sprites = spriteData.GetSprites(atlas);
        CurrentAct = spriteData.act;

        InitRenderers(atlas);

        SetAction(CursorAction.DEFAULT);

        isReady = false;
    }

    void Update() {
        if (!isReady)
            return;

        if (CursorCamera != null) {
            Cursor.visible = false;
        } else {
            Cursor.visible = true;
            return;
        }

        if (Input.GetKey(KeyCode.Mouse1)) {
            SetAction(CursorAction.ROTATE);
        }

        var frame = GetCurrentFrame();
        UpdateMesh(frame);

        Vector3 mousePosition = CursorCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = CursorCamera.transform.position.z + CursorCamera.nearClipPlane + CameraPlaneOffset;
        transform.position = mousePosition;
    }

    private void UpdateMesh(ACT.Frame frame) {
        MeshCache.TryGetValue(frame, out Mesh rendererMesh);
        if (rendererMesh == null) {
            rendererMesh = SpriteMeshBuilder.BuildSpriteMesh(frame, Sprites);
            MeshCache.Add(frame, rendererMesh);
        }

        MeshFilter.sharedMesh = null;
        MeshFilter.sharedMesh = rendererMesh;
    }

    private ACT.Frame GetCurrentFrame() {
        CurrentAction = CurrentAct.actions[CurrentActionIndex];
        var maxFrame = CurrentAction.frames.Length - 1;
        long deltaSinceAnimationStart = GameManager.Tick - AnimationStart;

        if (deltaSinceAnimationStart >= CurrentAction.delay * 1.15) {
            AnimationStart = GameManager.Tick;

            if (CurrentFrame < maxFrame) {
                CurrentFrame++;
            }
        }

        if (CurrentFrame >= maxFrame) {
            if (IsActionLoop()) {
                CurrentFrame = 0;
            } else {
                CurrentFrame = maxFrame;
            }
        }

        return CurrentAction.frames[CurrentFrame];
    }

    public bool IsActionLoop() {
        return type switch {
            CursorAction.DEFAULT => true,
            CursorAction.TALK => false,
            CursorAction.CLICK => false,
            CursorAction.LOCK => false,
            CursorAction.ROTATE => false,
            CursorAction.ATTACK => false,
            CursorAction.WARP => false,
            CursorAction.INVALID => false,
            CursorAction.PICK => false,
            CursorAction.TARGET => true,
            _ => true,
        };
    }

    public void SetAction(CursorAction type, bool repeat = false, int? animation = null) {
        if (type == this.type) {
            return;
        }

        this.type = type;
        CurrentActionIndex = animation ?? (int) type;
        CurrentFrame = 0;
        AnimationStart = GameManager.Tick;
    }

    private void InitRenderers(Texture2D atlas) {
        MeshFilter = gameObject.GetOrAddComponent<MeshFilter>();
        MeshRenderer = gameObject.GetOrAddComponent<MeshRenderer>();
        SortingGroup = gameObject.GetOrAddComponent<SortingGroup>();

        MeshRenderer.receiveShadows = false;
        MeshRenderer.lightProbeUsage = LightProbeUsage.Off;
        MeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
        MeshRenderer.material = SpriteMaterial;
        MeshRenderer.material.mainTexture = atlas;
    }
}

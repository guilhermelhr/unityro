using Assets.Scripts.Renderer.Sprite;
using ROIO;
using ROIO.Models.FileTypes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

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

    private ACT act;
    private List<Sprite> sprites = new List<Sprite>();

    private CursorAction type;
    private double tick;
    private bool repeat;

    public int currentAction;
    private int currentFrame;
    private double currentFrameTime = 0;

    private bool isReady = false;

    private void Awake() {
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    async void Start() {
        var spriteData = await Addressables.LoadAssetAsync<SpriteData>("data/sprite/cursors.asset").Task;

        sprites = spriteData.sprites.ToList();
        act = spriteData.act;

        tick = Time.deltaTime;

        //FlipTextures();
        SetAction(CursorAction.DEFAULT, true);

        // Leaving this off for now
        //isReady = true;
    }

    void Update() {
        if (!isReady)
            return;
        if (Input.GetKey(KeyCode.Mouse1)) {
            SetAction(CursorAction.ROTATE, false);
        }
    }

    // Update is called once per frame
    void LateUpdate() {
        if (!isReady)
            return;

        var action = act.actions[currentAction];
        currentFrameTime -= Time.deltaTime;

        if (currentFrameTime < 0 || currentFrame > action.frames.Length - 1) {
            AdvanceFrame();
        }

        var index = action.frames[currentFrame].layers[0].index;
        Cursor.SetCursor(sprites[index].texture, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void AdvanceFrame() {
        var action = act.actions[currentAction];
        currentFrame++;
        if (currentFrame > action.frames.Length - 1) {
            if (repeat) {
                currentFrame = 0;
            } else {
                currentFrame = action.frames.Length - 1;
            }
        }

        if (currentFrameTime < 0)
            currentFrameTime += action.delay / 1000f;
    }

    public void SetAction(CursorAction type, bool repeat, int? animation = null) {
        if (type == this.type) {
            return;
        }

        this.type = type;
        this.tick = GameManager.Tick;
        this.repeat = repeat;

        this.currentAction = animation ?? (int) type;
        this.currentFrame = 0;

        var action = act.actions[currentAction];

        this.currentFrameTime = action.delay / 1000f;
    }

    class CursorActionInfo {
        public int drawX, drawY, startX, startY;
        public float delayMult;
    }
}

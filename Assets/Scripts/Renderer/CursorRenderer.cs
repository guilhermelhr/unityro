using ROIO;
using ROIO.Models.FileTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CursorAction {
    DEFAULT = 0,
    TALK = 1,
    CLICK = 2,
    LOCK = 3,
    ROTATE = 4,
    ATTACK = 5,
    WARP = 7,
    PICK = 9,
    TARGET = 10
}

public class CursorRenderer : MonoBehaviour {

    private ACT act;
    private SPR spr;
    private List<Texture2D> textures = new List<Texture2D>();
    private Dictionary<CursorAction, CursorActionInfo> ActionInformations =
        new Dictionary<CursorAction, CursorActionInfo>() {
            { CursorAction.DEFAULT, new CursorActionInfo() { drawX = 1,  drawY= 19, startX = 0, startY = 0, delayMult = 2.0f } },
            { CursorAction.TALK,    new CursorActionInfo() { drawX = 20, drawY= 40, startX = 20, startY = 20, delayMult = 1.0f } },
            { CursorAction.WARP,    new CursorActionInfo() { drawX = 10, drawY= 32, startX = 0, startY = 0, delayMult = 1.0f } },
            { CursorAction.ROTATE,  new CursorActionInfo() { drawX = 18, drawY= 26, startX = 10, startY = 0, delayMult = 1.0f } },
            { CursorAction.PICK,    new CursorActionInfo() { drawX = 20, drawY= 40, startX = 15, startY = 15, delayMult = 1.0f } },
            { CursorAction.TARGET,  new CursorActionInfo() { drawX = 20, drawY= 50, startX = 20, startY = 28, delayMult = 0.5f } },
        };

    private CursorAction type;
    private double tick;
    private bool repeat;

    public int currentAction;
    private int currentFrame;
    private double currentFrameTime = 0;

    // Use this for initialization
    void Start() {
        spr = FileManager.Load("data/sprite/cursors.spr") as SPR;
        act = FileManager.Load("data/sprite/cursors.act") as ACT;

        tick = Time.deltaTime;

        FlipTextures();
        SetAction(CursorAction.DEFAULT, true);
    }

    void Update() {
        if (Input.GetKey(KeyCode.Mouse1)) {
            SetAction(CursorAction.ROTATE, false);
        }
    }

    // Update is called once per frame
    void LateUpdate() {
        var action = act.actions[currentAction];
        currentFrameTime -= Time.deltaTime;

        if (currentFrameTime < 0 || currentFrame > action.frames.Length - 1) {
            AdvanceFrame();
        }

        var index = action.frames[currentFrame].layers[0].index;
        Cursor.SetCursor(textures[index], Vector2.zero, CursorMode.ForceSoftware);
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
        this.tick = Core.Tick;
        this.repeat = repeat;

        this.currentAction = animation ?? (int) type;
        this.currentFrame = 0;

        var action = act.actions[currentAction];

        this.currentFrameTime = action.delay / 1000f;
    }

    private void FlipTextures() {
        foreach (var sprite in spr.GetSprites()) {
            var t = sprite.texture;
            var flipped = new Texture2D(t.width, t.height, TextureFormat.RGBA32, false, false);

#if UNITY_EDITOR
            flipped.alphaIsTransparency = true;
#endif

            int width = t.width;
            int height = t.height;

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    flipped.SetPixel(x, height - y - 1, t.GetPixel(x, y));
                }
            }
            flipped.Apply();

            textures.Add(flipped);
        }
    }

    private IEnumerator Animate() {
        //var action = act.actions[currentAction];
        //for (int i = 0; i < action.frames.Length; i++) {
        //    var spriteIndex = action.frames[i].layers[0].index;

        //    Cursor.SetCursor(textures[spriteIndex], Vector2.zero, CursorMode.Auto);
        //    yield return new WaitForSeconds(action.delay / 1000f);
        //}

        yield return Animate();
    }


    class CursorActionInfo {
        public int drawX, drawY, startX, startY;
        public float delayMult;
    }
}

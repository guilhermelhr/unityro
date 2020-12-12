using UnityEngine;

public partial class SPRRenderer : MonoBehaviour {

    public float secondsPerFrame = 1 / 10f;

    private Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private SPR spr;

    private int currentFrame = -1;
    private float nextChange = 0;

    public int lowerFrame = 0;
    public int upperFrame = 0;

    public void setSPR(SPR spr, int lowerFrame, int upperFrame) {
        setSPR(spr);
        setFrameLimits(lowerFrame, upperFrame);
    }

    public void setSPR(SPR spr) {
        this.spr = spr;
        sprites = spr.GetSprites();
        upperFrame = sprites.Length;
    }

    public void setFrameLimits(int lowerFrame, int upperFrame) {
        this.lowerFrame = lowerFrame;
        this.upperFrame = upperFrame;
        currentFrame = lowerFrame - 1;
        nextChange = 0;
    }

    void Start() {
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
    }

    void Update() {
        var now = Time.realtimeSinceStartup;

        if (now >= nextChange) {
            currentFrame++;
            if (currentFrame > upperFrame) {
                currentFrame = lowerFrame;
            }
            if (currentFrame < 0) {
                currentFrame = 0;
            }
            if (sprites == null) return;

            spriteRenderer.sprite = sprites[currentFrame];
            spriteRenderer.flipY = true;
            nextChange = now + secondsPerFrame;
        }
    }
}

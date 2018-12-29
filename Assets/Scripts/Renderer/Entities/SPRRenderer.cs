using UnityEngine;

public class SPRRenderer : MonoBehaviour
{
    public enum Animation {
        IDLE, WALK, ATTACK, DAMAGE, DIE
    }

    public float secondsPerFrame = 1/10f;

    private Sprite[] sprites;
    private SpriteRenderer spriteRenderer;

    private int currentFrame = -1;
    private float nextChange = 0;

    public int lowerFrame = 0;
    public int upperFrame = 0;

    public void setSPR(SPR spr, int lowerFrame, int upperFrame) {
        setSPR(spr);
        setFrameLimits(lowerFrame, upperFrame);
    }

    public void setSPR(SPR spr) {
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

        if(now >= nextChange) {
            currentFrame++;
            if(currentFrame > upperFrame) {
                currentFrame = lowerFrame;
            }

            spriteRenderer.sprite = sprites[currentFrame];
            nextChange = now + secondsPerFrame;
        }
    }
}

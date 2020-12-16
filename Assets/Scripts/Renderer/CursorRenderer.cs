using UnityEngine;
using System.Collections;


public class CursorRenderer : MonoBehaviour {

    private Texture2D cursorTexture;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;

    private ACT act;
    private SPR spr;
    private Sprite[] sprites;

    public int currentAction = 0;

    // Use this for initialization
    void Start() {
        spr = FileManager.Load("data/sprite/cursors.spr") as SPR;
        act = FileManager.Load("data/sprite/cursors.act") as ACT;
        sprites = spr.GetSprites();

        StartCoroutine(Animate());
    }

    // Update is called once per frame
    void Update() {

    }

    IEnumerator Animate() {
        var action = act.actions[currentAction];
        for (int i = 0; i < action.frames.Length; i++) {
            var spriteIndex = action.frames[i].layers[0].index;

            Cursor.SetCursor(sprites[spriteIndex].texture, hotSpot, cursorMode);
            yield return new WaitForSeconds(action.delay / 1000f);
        }

        yield return Animate();
    }
}

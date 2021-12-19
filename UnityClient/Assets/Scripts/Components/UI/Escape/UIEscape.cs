using UnityEngine;

public class UIEscape : MonoBehaviour {

    private Canvas Canvas;

    private void Awake() {
        Canvas = Canvas.FindMainCanvas();
    }
}

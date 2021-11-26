using UnityEngine;
using UnityEngine.EventSystems;

public class UIEscape : MonoBehaviour {

    private Canvas Canvas;
    private RectTransform ItemDragImageTransform;

    private IEscapeWindowController escapeWindowController;

    private void Awake() {
        Canvas = Canvas.FindMainCanvas();
    }

    internal void SetWindowController(IEscapeWindowController escapeWindowController) {
        this.escapeWindowController = escapeWindowController;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            // @todo
        }
    }
}

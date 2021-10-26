using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UsableContainer : MonoBehaviour, IDropHandler, IPointerClickHandler {

    [SerializeField] private RawImage UsableImage;

    private IUsable Usable;

    private void Awake() {
        UsableImage.texture = null;
    }

    public void OnDrop(PointerEventData eventData) {
        var dropped = eventData.pointerDrag?.GetComponent<IUsable>();
        if (dropped != null) {

            Usable = dropped;
            UsableImage.texture = dropped.GetTexture();
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) {
            Usable.OnRightClick();
        }

        if (eventData.clickCount == 2) {
            Usable.OnUse();
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUIWindow : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

    private CanvasGroup CanvasGroup;
    private Canvas MainCanvas;

    public void OnBeginDrag(PointerEventData eventData) {
        if (CanvasGroup == null) {
            CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
        }

        if (MainCanvas == null) {
            MainCanvas = MainCanvas.FindMainCanvas();
        }

        CanvasGroup.alpha = 0.8f;
    }

    public void OnDrag(PointerEventData eventData) {
        (transform as RectTransform).anchoredPosition += eventData.delta / MainCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData) {
        CanvasGroup.alpha = 1f;
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryGridItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField]
    private RawImage itemImage;

    [SerializeField]
    private TextMeshProUGUI amountLabel;

    private Item item;

    private void Start() {
    }

    public void SetItem(Item item) {
        this.item = item;
        if(item != null) {
            itemImage.enabled = true;
            itemImage.texture = item.texture;
            amountLabel.enabled = item.info.amount > 0;
            amountLabel.text = "" + item.info.amount;
        } else {
            itemImage.enabled = false;
            itemImage.texture = null;
            amountLabel.enabled = false;
            amountLabel.text = "";
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if(item == null) return;
        MapController.Instance.UIController.DisplayTooltip(
            $"{(item.info.IsIdentified ? item.identifiedDisplayName : item.unidentifiedDisplayName)} - {(item.info.amount > 0 ? item.info.amount : 1)} ea.",
            transform.position + new Vector3((transform as RectTransform).sizeDelta.x, (transform as RectTransform).sizeDelta.y));
    }

    public void OnPointerExit(PointerEventData eventData) {
        if(item == null) return;
        MapController.Instance.UIController.HideTooltip();
    }
}

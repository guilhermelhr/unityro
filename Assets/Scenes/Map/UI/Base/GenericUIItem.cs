using UnityEngine;
using UnityEngine.EventSystems;

public class GenericUIItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {

    private bool isDragging;
    private Vector3 InitialPosition = Vector3.zero;

    [SerializeField]
    protected ItemInfo itemInfo;

    private void Update() {
        if (isDragging) {
            transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2);
        } else if (InitialPosition != Vector3.zero) {
            var dir = InitialPosition - transform.position;
            transform.position += dir * Time.deltaTime * 10;
        }

        if (transform.position == InitialPosition) {
            InitialPosition = Vector3.zero;
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.clickCount == 2) {
            MapController.Instance.UIController.HideTooltip();
            switch ((ItemType)itemInfo.itemType) {
                // Usable item
                case ItemType.HEALING:
                case ItemType.USABLE:
                case ItemType.USABLE_UNK:
                    Core.Session.Entity.Inventory.OnUseItem(itemInfo.index);
                    break;

                // Use card
                case ItemType.CARD:
                    //Inventory.onUseCard(item.index);
                    break;

                case ItemType.USABLE_SKILL:
                    break;

                // Equip item
                case ItemType.WEAPON:
                case ItemType.EQUIP:
                case ItemType.PETEQUIP:
                case ItemType.AMMO:
                    if (itemInfo.IsIdentified && !itemInfo.IsDamaged) {
                        if (itemInfo.wearState <= 0) {//wear
                            Core.Session.Entity.Inventory.OnEquipItem(itemInfo.index, itemInfo.location);
                        } else {//takeoff
                            Core.Session.Entity.Inventory.OnTakeOffItem(itemInfo.index);
                        }
                    }
                    break;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (itemInfo == null || isDragging) return;
        var rectTransform = (transform as RectTransform);
        var itemName = itemInfo.IsIdentified ? itemInfo.item.identifiedDisplayName : itemInfo.item.unidentifiedDisplayName;
        var extra = "";
        if (itemInfo.tab == InventoryType.EQUIP) {
            extra = $"[{itemInfo.item.slotCount}]";
        } else if (itemInfo.amount > 0) {
            extra = $"- {itemInfo.amount} ea";
        }

        var label = $"{itemName} {extra}";
        var position = rectTransform.position + new Vector3(rectTransform.rect.x, rectTransform.rect.y + rectTransform.rect.height);
        MapController.Instance.UIController.DisplayTooltip(label, position);
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (itemInfo == null || isDragging) return;

        MapController.Instance.UIController.HideTooltip();
    }

    public void OnPointerUp(PointerEventData eventData) {
        isDragging = false;
    }

    public void OnPointerDown(PointerEventData eventData) {
        isDragging = true;
        InitialPosition = transform.position;
    }
}
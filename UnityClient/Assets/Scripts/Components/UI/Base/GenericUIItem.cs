using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GenericUIItem : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler,
    IUsable,
    IBeginDragHandler,
    IEndDragHandler,
    IDragHandler {

    [SerializeField] protected ItemInfo itemInfo;

    private Canvas Canvas;
    private RectTransform ItemDragImageTransform;

    private void Awake() {
        Canvas = Canvas.FindMainCanvas();
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) {
            DisplayItemDetails(eventData.position);
        }

        if (eventData.clickCount == 2) {
            MapController.Instance.UIController.HideTooltip();
            UseItem();
        }
    }

    private void DisplayItemDetails(Vector2 position) {
        MapUiController.Instance.DisplayItemDetails(itemInfo, position);
    }

    private void UseItem() {
        switch ((ItemType) itemInfo.itemType) {
            // Usable item
            case ItemType.HEALING:
            case ItemType.USABLE:
            case ItemType.USABLE_UNK:
                (Session.CurrentSession.Entity as Entity).Inventory.OnUseItem(itemInfo.index);
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
                    if (itemInfo.wearState <= 0 || itemInfo.itemType == (int) ItemType.AMMO) {//wear
                        (Session.CurrentSession.Entity as Entity).Inventory.OnEquipItem(itemInfo.index, itemInfo.location);
                    } else {//takeoff
                        (Session.CurrentSession.Entity as Entity).Inventory.OnTakeOffItem(itemInfo.index);
                    }
                }
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (itemInfo == null)
            return;
        var rectTransform = (transform as RectTransform);
        string label = GetItemName();
        var position = rectTransform.position + new Vector3(rectTransform.rect.x, rectTransform.rect.y + rectTransform.rect.height);
        MapController.Instance.UIController.DisplayTooltip(label, position);
    }

    private string GetItemName() {
        var itemName = itemInfo.IsIdentified ? itemInfo.item.identifiedDisplayName : itemInfo.item.unidentifiedDisplayName;
        var extra = "";
        if (itemInfo.tab == InventoryType.EQUIP) {
            extra = $"[{itemInfo.item.slotCount}]";
        }

        var label = $"{itemName} {extra}";
        return label;
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (itemInfo == null)
            return;

        MapController.Instance.UIController.HideTooltip();
    }

    public void OnUse() {
        UseItem();
    }

    public string GetDisplayName() {
        return GetItemName();
    }

    public int GetDisplayNumber() {
        return itemInfo.amount;
    }

    public Texture2D GetTexture() {
        return itemInfo.res;
    }

    public void OnRightClick() {
        DisplayItemDetails(Vector2.zero);
    }

    public void OnBeginDrag(PointerEventData eventData) {
        var ItemDragImage = new GameObject("ShopItemDrag");
        ItemDragImage.transform.SetParent(Canvas.transform, false);
        ItemDragImage.transform.SetAsLastSibling();

        var image = ItemDragImage.AddComponent<RawImage>();
        image.texture = itemInfo.res;
        image.SetNativeSize();

        CanvasGroup canvasGroup = ItemDragImage.gameObject.AddComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;

        ItemDragImageTransform = ItemDragImage.GetComponent<RectTransform>();
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(eventData.pointerEnter.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out var globalMousePos)
        ) {
            ItemDragImageTransform.position = globalMousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        Destroy(ItemDragImageTransform.gameObject);
    }

    public void OnDrag(PointerEventData eventData) {
        ItemDragImageTransform.anchoredPosition += eventData.delta / Canvas.scaleFactor;
    }
}
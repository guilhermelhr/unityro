using System;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler {

    [SerializeField]
    private RawImage ItemImage;

    [SerializeField]
    private TextMeshProUGUI ItemName;

    [SerializeField]
    private TextMeshProUGUI ItemPrice;

    [SerializeField]
    private TextMeshProUGUI ItemQuantity;

    public ItemNPCShopInfo ItemShopInfo { get; private set; }

    public int Quantity { get; private set; }
    public Item Item { get; private set; }

    private Canvas Canvas;
    private RectTransform ItemDragImageTransform;
    private INPCShopController ShopController;

    private void Awake() {
        Canvas = Canvas.FindMainCanvas();
    }

    public async void SetItemShopInfo(ItemNPCShopInfo itemShopInfo, NpcShopType shopType) {
        ItemShopInfo = itemShopInfo;

        if (shopType == NpcShopType.BUY) {
            Item = DBManager.GetItem(itemShopInfo.itemID);
            try {
                ItemImage.texture = await Addressables.LoadAssetAsync<Texture2D>(DBManager.GetItemResPath(itemShopInfo.itemID, true)).Task;
            } catch {
                Debug.LogError($"Could not load texture of item {itemShopInfo.itemID}");
                return;
            }
        } else if (shopType == NpcShopType.SELL) {
            var itemInfo = (Session.CurrentSession.Entity as Entity).Inventory.GetItem(itemShopInfo.inventoryIndex);

            if (itemInfo == null) {
                Debug.LogError($"Could not find item on inventory index {itemShopInfo.inventoryIndex}");
                return;
            }

            itemShopInfo.itemID = itemInfo.ItemID;
            itemShopInfo.type = itemInfo.itemType;
            try {
                ItemImage.texture = await Addressables.LoadAssetAsync<Texture2D>(DBManager.GetItemResPath(itemInfo.ItemID, true)).Task;
            } catch {
                Debug.LogError($"Could not load texture of item {itemInfo.ItemID}");
                return;
            }

            Item = itemInfo.item;
            Quantity = itemInfo.amount;
        }

        ItemName.text = Item.identifiedDisplayName;
        ItemPrice.text = $"{itemShopInfo.specialPrice}Z";
        ItemQuantity.text = shopType == NpcShopType.BUY ? null : $"{Quantity}";
    }

    public string GetItemName() {
        return Item.identifiedDisplayName;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        var ItemDragImage = new GameObject("ShopItemDrag");
        ItemDragImage.transform.SetParent(Canvas.transform, false);
        ItemDragImage.transform.SetAsLastSibling();

        var image = ItemDragImage.AddComponent<RawImage>();
        image.texture = ItemImage.texture;
        image.SetNativeSize();

        CanvasGroup canvasGroup = ItemDragImage.gameObject.AddComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;

        ItemDragImageTransform = ItemDragImage.GetComponent<RectTransform>();
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(eventData.pointerEnter.transform as RectTransform, eventData.position, eventData.pressEventCamera, out var globalMousePos)) {
            ItemDragImageTransform.position = globalMousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        Destroy(ItemDragImageTransform.gameObject);
    }

    public void OnDrag(PointerEventData eventData) {
        ItemDragImageTransform.anchoredPosition += eventData.delta / Canvas.scaleFactor;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.clickCount == 2) {
            ShopController.AddToCart(this);
        }
    }

    private void OnDestroy() {
        if (ItemDragImageTransform != null) {
            Destroy(ItemDragImageTransform.gameObject);
        }
    }

    internal void SetShopController(INPCShopController shopController) {
        ShopController = shopController;
    }
}

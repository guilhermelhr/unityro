using ROIO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static ZC.PC_PURCHASE_ITEMLIST;

public class ShopItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

    [SerializeField] private RawImage ItemImage;
    [SerializeField] private TextMeshProUGUI ItemName;
    [SerializeField] private TextMeshProUGUI ItemPrice;

    public ItemNPCShopInfo ItemShopInfo { get; private set; }
    
    private Item Item;
    private Canvas Canvas;
    private RectTransform ItemDragImageTransform;

    private void Awake() {
        Canvas = FindObjectOfType<Canvas>();
    }

    public void SetItemShopInfo(ItemNPCShopInfo itemShopInfo) {
        ItemShopInfo = itemShopInfo;

        Item = DBManager.GetItem(itemShopInfo.itemID);
        try {
            ItemImage.texture = FileManager.Load(DBManager.GetItemResPath(itemShopInfo.itemID, true)) as Texture2D;
        } catch {

        }
        ItemName.text = Item.identifiedDisplayName;
        ItemPrice.text = $"{itemShopInfo.discount}Z";
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
}

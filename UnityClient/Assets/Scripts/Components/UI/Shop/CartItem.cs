using ROIO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static ZC.PC_PURCHASE_ITEMLIST;

public class CartItem : MonoBehaviour {

    [SerializeField] private RawImage ItemImage;
    [SerializeField] private TextMeshProUGUI ItemName;
    [SerializeField] private TextMeshProUGUI ItemPrice;

    public ItemNPCShopInfo ItemShopInfo { get; private set; }
    public int Quantity { get; private set; }

    private Item Item;

    public void SetItemShopInfo(ItemNPCShopInfo itemShopInfo, int quantity) {
        ItemShopInfo = itemShopInfo;
        Quantity = quantity;

        Item = DBManager.GetItem(itemShopInfo.itemID);
        try {
            ItemImage.texture = FileManager.Load(DBManager.GetItemResPath(itemShopInfo.itemID, true)) as Texture2D;
        } catch {

        }
        ItemName.text = Item.identifiedDisplayName;
        ItemPrice.text = $"{itemShopInfo.discount}Z";
    }
}

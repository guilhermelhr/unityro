using ROIO;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ZC.PC_PURCHASE_ITEMLIST;

public class ShopItem : MonoBehaviour {

    [SerializeField] private RawImage ItemImage;
    [SerializeField] private TextMeshProUGUI ItemName;
    [SerializeField] private TextMeshProUGUI ItemPrice;

    private ItemNPCShopInfo ItemShopInfo;
    private Item Item;

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
}

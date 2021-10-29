using System.Collections.Generic;
using UnityEngine;

public class NpcShopController : DraggableUIWindow {

    [SerializeField] private GameObject CatalogScrollView;
    [SerializeField] private ShopItem ShopItemPrefab;
    [SerializeField] private ShopCart ShopCart;

    private List<ShopItem> CurrentShopItems;

    internal void DisplayShop(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.PC_PURCHASE_ITEMLIST PC_PURCHASE_ITEMLIST) {
            CurrentShopItems = new List<ShopItem>();
            foreach (var item in PC_PURCHASE_ITEMLIST.ItemList) {
                var shopItem = Instantiate(ShopItemPrefab, CatalogScrollView.transform);
                shopItem.SetItemShopInfo(item);
                CurrentShopItems.Add(shopItem);
            }
        }

        gameObject.SetActive(true);
    }

    public void Cancel() {
        CurrentShopItems.ForEach(it => Destroy(it.gameObject));
        CurrentShopItems.Clear();
        ShopCart.Clear();
        gameObject.SetActive(false);
    }
}

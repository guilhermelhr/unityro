using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcShopController : DraggableUIWindow, INPCShopController {

    [SerializeField]
    private GameObject CatalogScrollView;

    [SerializeField]
    private ShopItem ShopItemPrefab;

    [SerializeField]
    private ShopCart ShopCart;

    private List<ShopItem> CurrentShopItems;
    private NpcShopType ShopType;

    internal void DisplayShop(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.PC_PURCHASE_ITEMLIST PC_PURCHASE_ITEMLIST) {
            ShopType = NpcShopType.BUY;
            CurrentShopItems = new List<ShopItem>();
            ShopCart.SetShopType(ShopType);

            SetupShopItemList(PC_PURCHASE_ITEMLIST.ItemList);
        } else if (packet is ZC.PC_SELL_ITEMLIST PC_SELL_ITEMLIST) {
            ShopType = NpcShopType.SELL;
            CurrentShopItems = new List<ShopItem>();
            ShopCart.SetShopType(ShopType);

            SetupShopItemList(PC_SELL_ITEMLIST.ItemList);
        }

        gameObject.SetActive(true);
    }

    internal void OnPurchaseResult(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.PC_PURCHASE_RESULT PC_PURCHASE_RESULT) {
            ChatBoxController ChatBox = MapUiController.Instance.ChatBox;

            ClearAndClose();

            switch (PC_PURCHASE_RESULT.Result) {
                case 0:
                    ChatBox.DisplayMessage(54, ChatMessageType.BLUE);
                    break; // success
                case 1:
                    ChatBox.DisplayMessage(55, ChatMessageType.ERROR);
                    break; // zeny
                case 2:
                    ChatBox.DisplayMessage(56, ChatMessageType.ERROR);
                    break; // overweight
                case 4:
                    ChatBox.DisplayMessage(230, ChatMessageType.ERROR);
                    break; // out of stock
                case 5:
                    ChatBox.DisplayMessage(281, ChatMessageType.ERROR);
                    break; // trade
                           // case 6: 6 = Because the store information was incorrect the item was not purchased.
                case 7:
                    ChatBox.DisplayMessage(1797, ChatMessageType.ERROR);
                    break; // no sale information
                default:
                    ChatBox.DisplayMessage(57, ChatMessageType.ERROR);
                    break; // deal failed
            }
        }
    }

    internal void OnSellResult(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.PC_SELL_RESULT PC_SELL_RESULT) {
            ChatBoxController ChatBox = MapUiController.Instance.ChatBox;

            ClearAndClose();

            if (PC_SELL_RESULT.Result == 0) { // success
                ChatBox.DisplayMessage(54, ChatMessageType.BLUE);
            } else {
                ChatBox.DisplayMessage(57, ChatMessageType.ERROR);
            }
        }
    }

    public void Cancel() {
        ClearAndClose();

        new CZ.NPC_TRADE_QUIT().Send();
    }

    internal void RemoveItem(ShopItem droppedItem) {
        var itemShopInfoList = CurrentShopItems.Where(it => it.Item.id != droppedItem.Item.id).Select(it => it.ItemShopInfo).ToList();

        CurrentShopItems.ForEach(it => Destroy(it.gameObject));
        CurrentShopItems.Clear();

        SetupShopItemList(itemShopInfoList);
    }

    private void ClearAndClose() {
        CurrentShopItems.ForEach(it => Destroy(it.gameObject));
        CurrentShopItems.Clear();
        ShopCart.Clear();
        gameObject.SetActive(false);
    }

    internal void SubmitCart(List<CartItem> currentCartItems) {
        if (ShopType == NpcShopType.BUY) {
            var packetItems = currentCartItems
                .Select(it =>
                    new CZ.PC_PURCHASE_ITEMLIST.PC_PURCHASE_ITEMLIST_sub { Amount = (short) it.Quantity, ItemId = it.ItemShopInfo.itemID }
                ).ToList();

            new CZ.PC_PURCHASE_ITEMLIST {
                items = packetItems
            }.Send();
        } else if (ShopType == NpcShopType.SELL) {
            var packetItems = currentCartItems
                .Select(it =>
                    new CZ.PC_SELL_ITEMLIST.PC_SELL_ITEMLIST_sub { Amount = (short) it.Quantity, InventoryIndex = (short) it.ItemShopInfo.inventoryIndex }
                ).ToList();

            new CZ.PC_SELL_ITEMLIST {
                items = packetItems
            }.Send();
        }
    }

    public void AddToCart(ShopItem shopItem) {
        ShopCart.AddItem(shopItem);
    }

    private void SetupShopItemList(List<ItemNPCShopInfo> itemList) {
        foreach (var item in itemList) {
            var shopItem = Instantiate(ShopItemPrefab, CatalogScrollView.transform);
            shopItem.SetShopController(this);
            shopItem.SetItemShopInfo(item, ShopType);
            CurrentShopItems.Add(shopItem);
        }
    }
}

internal interface INPCShopController {
    void AddToCart(ShopItem shopItem);
}

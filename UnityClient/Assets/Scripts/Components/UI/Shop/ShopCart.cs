using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopCart : MonoBehaviour, IDropHandler {

    [SerializeField] private GameObject CartScrollView;
    [SerializeField] private CartItem ShopItemPrefab;
    [SerializeField] private TextMeshProUGUI TotalPriceLabel;

    private List<CartItem> CurrentCartItems;

    private void Awake() {
        CurrentCartItems = new List<CartItem>();
    }

    public void OnDrop(PointerEventData eventData) {
        var droppedItem = eventData.pointerDrag?.GetComponent<ShopItem>();
        if (droppedItem != null) {

            var itemType = (ItemType) droppedItem.ItemShopInfo.type;
            var isStackable = itemType != ItemType.WEAPON &&
                itemType != ItemType.EQUIP &&
                itemType != ItemType.PETEGG &&
                itemType != ItemType.PETEQUIP;

            // TODO: Check for type & isStackable & vending type
            // TODO: Check if there's only one item to buy or if we're selling with the select all toggle
            // TODO: display amount input

            AddItemToCart(droppedItem);
        }
    }

    private void AddItemToCart(ShopItem droppedItem) {
        var shopItem = Instantiate(ShopItemPrefab, CartScrollView.transform);
        shopItem.SetItemShopInfo(droppedItem.ItemShopInfo);
        CurrentCartItems.Add(shopItem);
        SetPriceLabel();
    }

    public void Clear() {
        CurrentCartItems.ForEach(t => Destroy(t.gameObject));
        CurrentCartItems.Clear();
        SetPriceLabel();
    }

    private void SetPriceLabel() {
        TotalPriceLabel.text = $"Total Price : {CurrentCartItems.Sum(it => it.ItemShopInfo.discount)} Zeny";
    }
}

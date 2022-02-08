using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopCart : MonoBehaviour, IDropHandler {

    [SerializeField]
    private GameObject CartScrollView;

    [SerializeField]
    private CartItem ShopItemPrefab;

    [SerializeField]
    private TextMeshProUGUI TotalPriceLabel;

    [SerializeField]
    private NpcShopController ShopController;

    [SerializeField]
    private NumberInput QuantityInput;

    [SerializeField]
    private Button SubmitButton;

    [SerializeField]
    private Button CancelButton;

    private List<CartItem> CurrentCartItems;
    private NpcShopType ShopType;

    internal void SetShopType(NpcShopType shopType) {
        ShopType = shopType;
    }

    private int TotalPrice => CurrentCartItems.Sum(it => it.ItemShopInfo.discount * it.Quantity);

    private void Awake() {
        CurrentCartItems = new List<CartItem>();

        CancelButton.onClick.AddListener(ShopController.Cancel);
        SubmitButton.onClick.AddListener(delegate {
            ShopController.SubmitCart(CurrentCartItems);
        });
    }

    public async void OnDrop(PointerEventData eventData) {
        var droppedItem = eventData.pointerDrag?.GetComponent<ShopItem>();
        var entity = Session.CurrentSession.Entity as Entity;
        if (droppedItem != null) {

            var cartItem = CurrentCartItems.FirstOrDefault(it => it.ItemShopInfo.itemID == droppedItem.ItemShopInfo.itemID);
            var itemType = (ItemType) droppedItem.ItemShopInfo.type;
            var isStackable = itemType != ItemType.WEAPON &&
                itemType != ItemType.EQUIP &&
                itemType != ItemType.PETEGG &&
                itemType != ItemType.PETEQUIP;

            if (isStackable) {
                var numberInput = Instantiate(QuantityInput);
                numberInput.transform.SetParent(gameObject.transform);
                numberInput.SetTitle(droppedItem.GetItemName());
                var quantity = await numberInput.AwaitConfirmation();
                Destroy(numberInput.gameObject);
               
                if (entity.GetBaseStatus().zeny < TotalPrice + (quantity * droppedItem.ItemShopInfo.discount)) {
                    MapUiController.Instance.ChatBox.DisplayMessage(55, ChatMessageType.ERROR);
                    return;
                }
                
                if (cartItem != null) {
                    cartItem.IncreaseQuantityBy(quantity);
                    SetPriceLabel();
                } else {
                    AddItemToCart(droppedItem, quantity);
                }
            } else {
                if (cartItem == null) {
                    AddItemToCart(droppedItem, 1);
                }
            }

            // TODO: Check for type & isStackable & vending type
            // TODO: Check if there's only one item to buy or if we're selling with the select all toggle
        }
    }

    private void AddItemToCart(ShopItem droppedItem, int quantity) {
        var shopItem = Instantiate(ShopItemPrefab, CartScrollView.transform);
        shopItem.SetItemShopInfo(droppedItem.ItemShopInfo, quantity);
        CurrentCartItems.Add(shopItem);
        SetPriceLabel();
    }

    public void Clear() {
        CurrentCartItems.ForEach(t => Destroy(t.gameObject));
        CurrentCartItems.Clear();
        SetPriceLabel();
    }

    private void SetPriceLabel() {
        TotalPriceLabel.text = $"Total Price : {TotalPrice} Zeny";
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    private NumberInput QuantityInputPrefab;

    [SerializeField]
    private Button SubmitButton;

    [SerializeField]
    private Button CancelButton;

    [SerializeField]
    private Toggle IgnoreQuantityToggle;

    private List<CartItem> CurrentCartItems;
    private NpcShopType ShopType;
    private NumberInput QuantityInput;

    internal void SetShopType(NpcShopType shopType) {
        ShopType = shopType;
        SubmitButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = ShopType == NpcShopType.BUY ? "Buy" : "Sell";
        IgnoreQuantityToggle.gameObject.SetActive(ShopType == NpcShopType.SELL);
    }

    private int TotalPrice => CurrentCartItems.Sum(it => it.ItemShopInfo.specialPrice * it.Quantity);

    private void Awake() {
        CurrentCartItems = new List<CartItem>();

        CancelButton.onClick.AddListener(ShopController.Cancel);
        SubmitButton.onClick.AddListener(delegate {
            ShopController.SubmitCart(CurrentCartItems);
        });
    }

    public async void OnDrop(PointerEventData eventData) {
        var droppedItem = eventData.pointerDrag?.GetComponent<ShopItem>();
        if (droppedItem != null) {
            await AddItem(droppedItem);
        }
    }

    public async Task AddItem(ShopItem droppedItem) {
        var entity = Session.CurrentSession.Entity as Entity;
        var cartItem = CurrentCartItems.FirstOrDefault(it => it.ItemShopInfo.itemID == droppedItem.ItemShopInfo.itemID);
        var itemType = (ItemType) droppedItem.ItemShopInfo.type;
        var isStackable = itemType != ItemType.WEAPON &&
            itemType != ItemType.EQUIP &&
            itemType != ItemType.PETEGG &&
            itemType != ItemType.PETEQUIP;

        if (isStackable) {
            if (ShopType == NpcShopType.BUY) {
                await ProcessBuyStackable(droppedItem, entity, cartItem);
            } else if (ShopType == NpcShopType.SELL) {
                await ProcessSellStackable(droppedItem, entity, cartItem);
            }
        } else {
            if (cartItem == null) {
                AddItemToCart(droppedItem, 1, isStackable);
                SetPriceLabel();
            }
        }
    }

    private async Task ProcessSellStackable(ShopItem droppedItem, Entity entity, CartItem cartItem) {
        var quantity = 0;
        if (IgnoreQuantityToggle.isOn) {
            quantity = entity.Inventory.ItemList.First(it => it.ItemID == droppedItem.Item.id).amount;
        } else {
            quantity = await FindQuantity(droppedItem);
            Destroy(QuantityInput.gameObject);
        }

        var updatedQuantity = (cartItem?.Quantity ?? 0) + quantity;
        if (updatedQuantity > droppedItem.Quantity) {
            // TODO error message
            return;
        }

        if (updatedQuantity == droppedItem.Quantity) {
            ShopController.RemoveItem(droppedItem);
        }

        if (cartItem != null) {
            cartItem.IncreaseQuantityBy(quantity);
            SetPriceLabel();
        } else {
            AddItemToCart(droppedItem, quantity, true);
        }
    }

    private async Task ProcessBuyStackable(ShopItem droppedItem, Entity entity, CartItem cartItem) {
        var quantity = await FindQuantity(droppedItem);
        Destroy(QuantityInput.gameObject);

        if (entity.GetBaseStatus().zeny < TotalPrice + (quantity * droppedItem.ItemShopInfo.specialPrice)) {
            MapUiController.Instance.ChatBox.DisplayMessage(55, ChatMessageType.ERROR);
            return;
        }

        if (cartItem != null) {
            cartItem.IncreaseQuantityBy(quantity);
            SetPriceLabel();
        } else {
            AddItemToCart(droppedItem, quantity, true);
            SetPriceLabel();
        }
    }

    private async Task<int> FindQuantity(ShopItem droppedItem) {
        QuantityInput = Instantiate(QuantityInputPrefab);
        QuantityInput.transform.SetParent(gameObject.transform, false);
        QuantityInput.SetTitle(droppedItem.GetItemName(), 1);
        var quantity = await QuantityInput.AwaitConfirmation();

        return quantity;
    }

    private void AddItemToCart(ShopItem droppedItem, int quantity, bool isStackable) {
        var shopItem = Instantiate(ShopItemPrefab, CartScrollView.transform);
        shopItem.SetItemShopInfo(droppedItem.ItemShopInfo, isStackable ? quantity : 1);
        CurrentCartItems.Add(shopItem);
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

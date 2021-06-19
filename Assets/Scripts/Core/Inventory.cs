using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Inventory {

    private Dictionary<int, ItemInfo> Items = new Dictionary<int, ItemInfo>();

    public bool IsEmpty => Items.Count == 0;
    public List<ItemInfo> ItemList => Items.Values.ToList();

    public void AddItem(ItemInfo item) {
        Items.TryGetValue(item.index, out var it);
        if (it != null) {
            it.amount++;
        } else {
            Items.Add(item.index, item);
        }
    }

    public void RemoveItem(ItemInfo item) {
        Items.TryGetValue(item.index, out var it);
        if (it != null) {
            Items.Remove(item.index);
        }
    }

    public ItemInfo RemoveItem(int index) {
        Items.TryGetValue(index, out var it);
        if (it != null) {
            Items.Remove(index);
            return it;
        }

        return null;
    }

    public void UpdateItem(short index, short count) {
        Items.TryGetValue(index, out ItemInfo item);
        if (item == null) return;

        item.amount = count;

        if (item.amount <= 0) {
            RemoveItem(index);
        }
        MapUiController.Instance.InventoryWindow.UpdateEquipment();
    }

    public void TakeOffItem(int index, int equipLocation) {
        Items.TryGetValue(index, out ItemInfo item);
        if (item == null) return;

        item.wearState = 0;
    }

    public void EquipItem(short index, int equipLocation) {
        Items.TryGetValue(index, out ItemInfo item);
        if (item == null) return;

        item.wearState = equipLocation;
    }

    public void OnUseItem(short index) {
        new CZ.USE_ITEM2() {
            AID = Session.CurrentSession.AccountID,
            index = index
        }.Send();
    }

    public void OnEquipItem(short index, int location) {
        new CZ.REQ_WEAR_EQUIP_V5() {
            index = index,
            location = location
        }.Send();
    }

    public void OnTakeOffItem(short index) {
        new CZ.REQ_TAKEOFF_EQUIP() {
            index = index
        }.Send();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Inventory {

    private Dictionary<int, Item> Items = new Dictionary<int, Item>();

    public bool IsEmpty => Items.Count == 0;
    public List<Item> ItemList => Items.Values.ToList();

    public void AddItem(Item item) {
        Items.TryGetValue(item.info.index, out var it);
        if(it != null) {
            it.info.amount++;
        } else {
            Items.Add(item.info.index, item);
        }
    }

    public void RemoveItem(Item item) {
        Items.TryGetValue(item.info.index, out var it);
        if(it != null) {
            Items.Remove(item.info.index);
        }
    }

    public Item RemoveItem(int index) {
        Items.TryGetValue(index, out var it);
        if(it != null) {
            Items.Remove(index);
            return it;
        }

        return null;
    }

    public void UpdateItem(short index, short count) {
        Items.TryGetValue(index, out Item item);
        if(item == null) return;

        item.info.amount = count;

        if(item.info.amount <= 0) {
            RemoveItem(index);
        }
        MapUiController.Instance.InventoryWindow.UpdateEquipment();
    }

    public void EquipItem(short index, int equipLocation) {
        Items.TryGetValue(index, out Item item);
        if(item == null) return;

        item.info.wearState = equipLocation;
    }

    public void OnUseItem(short index) {
        new CZ.USE_ITEM2() {
            AID = Core.Session.AccountID,
            index = index
        }.Send();
    }

    public void OnEquipItem(short index, int location) {
        new CZ.REQ_WEAR_EQUIP_V5() {
            index = index,
            location = location
        }.Send();
    }
}
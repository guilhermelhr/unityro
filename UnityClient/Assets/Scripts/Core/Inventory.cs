using System.Collections.Generic;
using System.Linq;

public class Inventory {

    private Dictionary<int, ItemInfo> Items = new Dictionary<int, ItemInfo>();
    public bool IsEmpty => Items.Count == 0;
    public List<ItemInfo> ItemList => Items.Values.ToList();

    public ItemInfo GetItem(int inventoryIndex) {
        ItemInfo item = null;
        Items.TryGetValue(inventoryIndex, out item);

        return item;
    }

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

    public ItemInfo RemoveItem(int index, int count) {
        Items.TryGetValue(index, out var it);

        if (it != null) {

            if (it.amount > 1) {
                it.amount -= count;
            } else {
                RemoveItem(it);
            }

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
        var entity = Session.CurrentSession.Entity as Entity;

        item.wearState = 0;

        if ((equipLocation & (int)EquipLocation.HEAD_TOP) > 0) entity.EquipInfo.HeadTop = 0;
        if ((equipLocation & (int)EquipLocation.HEAD_MID) > 0) entity.EquipInfo.HeadMid = 0;
        if ((equipLocation & (int)EquipLocation.HEAD_BOTTOM) > 0) entity.EquipInfo.HeadBottom = 0;
        if ((equipLocation & (int)EquipLocation.GARMENT) > 0) entity.EquipInfo.Robe = 0;
        if ((equipLocation & (int)EquipLocation.WEAPON) > 0) entity.EquipInfo.Weapon = 0;
        if ((equipLocation & (int)EquipLocation.SHIELD) > 0) entity.EquipInfo.Shield = 0;

        Session.CurrentSession.Entity.UpdateSprites();
    }

    public void EquipItem(short index, int equipLocation) {
        Items.TryGetValue(index, out ItemInfo item);
        if (item == null) return;
        var viewID = (short)item.item.ClassNum;
        var entity = Session.CurrentSession.Entity as Entity;

        if ((equipLocation & (int)EquipLocation.HEAD_TOP) > 0) entity.EquipInfo.HeadTop = viewID;
        if ((equipLocation & (int)EquipLocation.HEAD_MID) > 0) entity.EquipInfo.HeadMid = viewID;
        if ((equipLocation & (int)EquipLocation.HEAD_BOTTOM) > 0) entity.EquipInfo.HeadBottom = viewID;
        if ((equipLocation & (int)EquipLocation.GARMENT) > 0) entity.EquipInfo.Robe = viewID;

        if ((equipLocation & (int)EquipLocation.WEAPON) > 0) {
            entity.EquipInfo.Weapon = viewID;
        } else if ((equipLocation & (int)EquipLocation.SHIELD) > 0) {
            entity.EquipInfo.Shield = viewID;
        }

        item.wearState = equipLocation;

        if (item.itemType == (int)ItemType.AMMO) {
            MapUiController.Instance.EquipmentWindow.EquipAmmo(item);
        } else {
            Session.CurrentSession.Entity.UpdateSprites();
            MapUiController.Instance.UpdateEquipment();
        }
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
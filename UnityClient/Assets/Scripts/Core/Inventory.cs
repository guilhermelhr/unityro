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
        if (item == null)
            return;

        item.amount = count;

        if (item.amount <= 0) {
            RemoveItem(index);
        }
        MapUiController.Instance.InventoryWindow.UpdateEquipment();
    }

    public void TakeOffItem(int index, int equipLocation) {
        Items.TryGetValue(index, out ItemInfo item);
        if (item == null)
            return;
        var entity = Session.CurrentSession.Entity as Entity;

        item.wearState = 0;

        if ((equipLocation & (int) EquipmentLocation.HEAD_TOP) > 0)
            entity.EquipInfo.HeadTop = null;
        if ((equipLocation & (int) EquipmentLocation.HEAD_MID) > 0)
            entity.EquipInfo.HeadMid = null;
        if ((equipLocation & (int) EquipmentLocation.HEAD_BOTTOM) > 0)
            entity.EquipInfo.HeadBottom = null;
        if ((equipLocation & (int) EquipmentLocation.GARMENT) > 0)
            entity.EquipInfo.Robe = null;
        if ((equipLocation & (int) EquipmentLocation.WEAPON) > 0)
            entity.EquipInfo.Weapon = null;
        if ((equipLocation & (int) EquipmentLocation.SHIELD) > 0)
            entity.EquipInfo.Shield = null;
    }

    public void EquipItem(short index, int equipLocation, short viewID) {
        Items.TryGetValue(index, out ItemInfo item);
        if (item == null || viewID != item.viewID)
            return;

        var entity = Session.CurrentSession.Entity as Entity;

        UpdateEntityEquipInfo(equipLocation, viewID, entity);

        item.wearState = equipLocation;

        if (item.itemType == (int) ItemType.AMMO) {
            MapUiController.Instance.EquipmentWindow.EquipAmmo(item);
        } else {
            entity.UpdateSprites();
            MapUiController.Instance.UpdateEquipment();
        }
    }

    public void UpdateEntityEquipInfo(int equipLocation, short viewID, Entity entity) {
        if ((equipLocation & (int) EquipmentLocation.HEAD_TOP) > 0)
            entity.EquipInfo.HeadTop = new EquipmentInfo { ViewID = viewID, EquipmentLocation = EquipmentLocation.HEAD_TOP };
        if ((equipLocation & (int) EquipmentLocation.HEAD_MID) > 0)
            entity.EquipInfo.HeadMid = new EquipmentInfo { ViewID = viewID, EquipmentLocation = EquipmentLocation.HEAD_MID };
        if ((equipLocation & (int) EquipmentLocation.HEAD_BOTTOM) > 0)
            entity.EquipInfo.HeadBottom = new EquipmentInfo { ViewID = viewID, EquipmentLocation = EquipmentLocation.HEAD_BOTTOM };
        if ((equipLocation & (int) EquipmentLocation.GARMENT) > 0)
            entity.EquipInfo.Robe = new EquipmentInfo { ViewID = viewID, EquipmentLocation = EquipmentLocation.GARMENT };

        if (IsLocation(equipLocation, EquipmentLocation.SHIELD) && !IsLocation(equipLocation, EquipmentLocation.WEAPON)) { //shield only
            entity.EquipInfo.Shield = new EquipmentInfo { ViewID = viewID, EquipmentLocation = EquipmentLocation.SHIELD };
        } else if (IsLocation(equipLocation, EquipmentLocation.SHIELD) && IsLocation(equipLocation, EquipmentLocation.WEAPON)) { //two handed weapons (bow etc)
            entity.EquipInfo.Shield = new EquipmentInfo { ViewID = viewID, EquipmentLocation = EquipmentLocation.SHIELD };
            entity.EquipInfo.Weapon = new EquipmentInfo { ViewID = viewID, EquipmentLocation = EquipmentLocation.WEAPON };
        } else if (IsLocation(equipLocation, EquipmentLocation.SHIELD) || IsLocation(equipLocation, EquipmentLocation.WEAPON)) {
            /**
             * If item can be equipped in any hand, we first check for the right hand
             * then we always switch the others on the left hand
             */
            if (entity.EquipInfo.Weapon != null) {
                entity.EquipInfo.Shield = new EquipmentInfo { ViewID = viewID, EquipmentLocation = EquipmentLocation.SHIELD };
            } else {
                entity.EquipInfo.Weapon = new EquipmentInfo { ViewID = viewID, EquipmentLocation = EquipmentLocation.WEAPON };
            }
        } else {
            entity.EquipInfo.Weapon = new EquipmentInfo { ViewID = viewID, EquipmentLocation = EquipmentLocation.WEAPON };
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

    public static bool IsLocation(int location, EquipmentLocation isThis) => (location & (int) isThis) > 0;
}
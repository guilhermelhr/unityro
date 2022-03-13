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
        if (!Items.TryGetValue(index, out ItemInfo item))
            return;

        var equipLocationFlag = (EquipmentLocation) equipLocation;
        var entity = Session.CurrentSession.Entity as Entity;

        item.wearState = 0;

        if (EquipmentLocation.HEAD_TOP.HasFlag(equipLocationFlag))
            entity.EquipInfo.HeadTop = null;
        if (EquipmentLocation.HEAD_MID.HasFlag(equipLocationFlag))
            entity.EquipInfo.HeadMid = null;
        if (EquipmentLocation.HEAD_BOTTOM.HasFlag(equipLocationFlag))
            entity.EquipInfo.HeadBottom = null;
        if (EquipmentLocation.GARMENT.HasFlag(equipLocationFlag))
            entity.EquipInfo.Gargment = null;
        if (EquipmentLocation.HAND_RIGHT.HasFlag(equipLocationFlag))
            entity.EquipInfo.RightHand = null;
        if (EquipmentLocation.HAND_LEFT.HasFlag(equipLocationFlag))
            entity.EquipInfo.LeftHand = null;
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
        var equipLocationFlag = (EquipmentLocation) equipLocation;

        if (EquipmentLocation.HEAD_TOP.HasFlag(equipLocationFlag))
            entity.EquipInfo.HeadTop = new EquipInfo { ViewID = viewID, Location = equipLocationFlag };
        if (EquipmentLocation.HEAD_MID.HasFlag(equipLocationFlag))
            entity.EquipInfo.HeadMid = new EquipInfo { ViewID = viewID, Location = equipLocationFlag };
        if (EquipmentLocation.HEAD_BOTTOM.HasFlag(equipLocationFlag))
            entity.EquipInfo.HeadBottom = new EquipInfo { ViewID = viewID, Location = equipLocationFlag };
        if (EquipmentLocation.GARMENT.HasFlag(equipLocationFlag))
            entity.EquipInfo.Gargment = new EquipInfo { ViewID = viewID, Location = equipLocationFlag };

        if (EquipmentLocation.HAND_LEFT.HasFlag(equipLocationFlag) && !EquipmentLocation.HAND_RIGHT.HasFlag(equipLocationFlag)) { //shield only
            entity.EquipInfo.LeftHand = new EquipInfo { ViewID = viewID, Location = equipLocationFlag };
        } else if (EquipmentLocation.HAND_LEFT.HasFlag(equipLocationFlag) && EquipmentLocation.HAND_RIGHT.HasFlag(equipLocationFlag)) { //two handed weapons (bow etc)
            entity.EquipInfo.LeftHand = new EquipInfo { ViewID = viewID, Location = equipLocationFlag };
            entity.EquipInfo.RightHand = new EquipInfo { ViewID = viewID, Location = equipLocationFlag };
        } else if (EquipmentLocation.HAND_LEFT.HasFlag(equipLocationFlag) || EquipmentLocation.HAND_RIGHT.HasFlag(equipLocationFlag)) {
            /**
             * If item can be equipped in any hand, we first check for the right hand
             * then we always switch the others on the left hand
             */
            if (entity.EquipInfo.RightHand != null) {
                entity.EquipInfo.LeftHand = new EquipInfo { ViewID = viewID, Location = equipLocationFlag };
            } else {
                entity.EquipInfo.RightHand = new EquipInfo { ViewID = viewID, Location = equipLocationFlag };
            }
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
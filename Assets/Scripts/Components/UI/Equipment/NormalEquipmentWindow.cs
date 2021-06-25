using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NormalEquipmentWindow : MonoBehaviour {

    public List<UIEquipSlot> slots;

    public void UpdateEquipment() {
        var inventory = (Session.CurrentSession.Entity as Entity).Inventory;
        if (inventory == null || inventory.IsEmpty) return;

        Dictionary<EquipLocation, UIEquipSlot> slotDictionary = slots.ToDictionary(it => it.location);
        Dictionary<int, ItemInfo> equippedItems = inventory.ItemList.Where(it => it.wearState > 0 && it.itemType != (int)ItemType.AMMO).ToDictionary(it => it.location);

        foreach (var key in equippedItems.Keys) {
            if ((key & (int)EquipLocation.HEAD_BOTTOM) > 0) { slotDictionary[EquipLocation.HEAD_BOTTOM].SetItem(equippedItems[key]); }
            if ((key & (int)EquipLocation.WEAPON) > 0) { slotDictionary[EquipLocation.WEAPON].SetItem(equippedItems[key]); }
            if ((key & (int)EquipLocation.GARMENT) > 0) { slotDictionary[EquipLocation.GARMENT].SetItem(equippedItems[key]); }
            if ((key & (int)EquipLocation.ACCESSORY1) > 0) { slotDictionary[EquipLocation.ACCESSORY1].SetItem(equippedItems[key]); }
            if ((key & (int)EquipLocation.ARMOR) > 0) { slotDictionary[EquipLocation.ARMOR].SetItem(equippedItems[key]); }
            if ((key & (int)EquipLocation.SHIELD) > 0) { slotDictionary[EquipLocation.SHIELD].SetItem(equippedItems[key]); }
            if ((key & (int)EquipLocation.SHOES) > 0) { slotDictionary[EquipLocation.SHOES].SetItem(equippedItems[key]); }
            if ((key & (int)EquipLocation.ACCESSORY2) > 0) { slotDictionary[EquipLocation.ACCESSORY2].SetItem(equippedItems[key]); }
            if ((key & (int)EquipLocation.HEAD_TOP) > 0) { slotDictionary[EquipLocation.HEAD_TOP].SetItem(equippedItems[key]); }
            if ((key & (int)EquipLocation.HEAD_MID) > 0) { slotDictionary[EquipLocation.HEAD_MID].SetItem(equippedItems[key]); }
        }
    }

    internal void UnequipAmmo() {
        slots.Find(it => it.location == EquipLocation.AMMO).SetItem(null);
    }

    public void EquipAmmo(ItemInfo item) {
        slots.Find(it => it.location == EquipLocation.AMMO).SetItem(item);
    }
}
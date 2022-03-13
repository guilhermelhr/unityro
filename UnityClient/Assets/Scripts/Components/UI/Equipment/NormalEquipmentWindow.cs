using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NormalEquipmentWindow : MonoBehaviour {

    public List<UIEquipSlot> slots;

    public void UpdateEquipment() {
        var entity = (Session.CurrentSession.Entity as Entity);
        var inventory = entity.Inventory;
        if (inventory == null || inventory.IsEmpty) return;

        Dictionary<EquipmentLocation, UIEquipSlot> slotDictionary = slots.ToDictionary(it => it.location);
        Dictionary<EquipmentLocation, ItemInfo> equippedItemsDict = inventory.ItemList.Where(IsItemEquipped).ToDictionary(it => (EquipmentLocation)it.wearState);

        slotDictionary.Values.ToList().ForEach(slot => slot.SetItem(null));
        foreach (var itemKey in equippedItemsDict.Keys) {
            var item = equippedItemsDict[itemKey];
            inventory.UpdateEntityEquipInfo(item.wearState, item.viewID, entity);

            foreach(var slotKey in slotDictionary.Keys) {
                if (slotKey.HasFlag(itemKey)) { 
                    slotDictionary[slotKey].SetItem(item);
                }
            }
        }

        entity.UpdateSprites();
    }

    private bool IsItemEquipped(ItemInfo it) {
        return it.wearState > 0 &&
            it.itemType != (int)ItemType.AMMO &&
            it.itemType != (int)ItemType.CARD;
    }

    internal void UnequipAmmo() {
        slots.Find(it => it.location == EquipmentLocation.AMMO).SetItem(null);
    }

    public void EquipAmmo(ItemInfo item) {
        slots.Find(it => it.location == EquipmentLocation.AMMO).SetItem(item);
    }
}
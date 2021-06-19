using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NormalEquipmentWindow : MonoBehaviour {

    public List<UIEquipSlot> slots;

    public void UpdateEquipment() {
        var inventory = (Session.CurrentSession.Entity as Entity).Inventory;
        if (inventory == null || inventory.IsEmpty) return;

        Dictionary<int, UIEquipSlot> slotDictionary = slots.ToDictionary(it => (int)it.location);
        Dictionary<int, ItemInfo> equippedItems = inventory.ItemList.Where(it => it.wearState > 0).ToDictionary(it => it.wearState);

        foreach (var key in slotDictionary.Keys) {
            var slot = slotDictionary[key];
            equippedItems.TryGetValue(key, out var item);
            slot.SetItem(item);
        }
    }
}
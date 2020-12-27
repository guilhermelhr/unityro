using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NormalEquipmentWindow : MonoBehaviour {

    public List<UIEquipSlot> slots;

    public void UpdateEquipment() {
        var inventory = Core.Session.Entity.Inventory;
        if (inventory == null || inventory.IsEmpty) return;

        Dictionary<int, UIEquipSlot> slotDictionary = slots.ToDictionary(it => (int)it.location);
        Dictionary<int, Item> equippedItems = inventory.ItemList.Where(it => it.info.wearState > 0).ToDictionary(it => it.info.wearState);

        if (equippedItems.Count == 0) return;

        foreach (var key in equippedItems.Keys) {
            equippedItems.TryGetValue(key, out var item);
            if (item != null) {
                slotDictionary.TryGetValue(key, out var slot);
                slot?.SetItem(item);
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NormalEquipmentWindow : MonoBehaviour {

    public List<UIEquipSlot> slots;

    [SerializeField] private Entity WindowEntity;

    public void UpdateEquipment() {
        var entity = (Session.CurrentSession.Entity as Entity);
        WindowEntity.Clone(entity, LayerMask.NameToLayer("UI"), true);
        WindowEntity.SortingGroup.sortingOrder = 3;
        WindowEntity.SetReady(true, true);

        var inventory = entity.Inventory;
        if (inventory == null || inventory.IsEmpty) return;

        Dictionary<EquipLocation, UIEquipSlot> slotDictionary = slots.ToDictionary(it => it.location);
        Dictionary<int, ItemInfo> equippedItems = inventory.ItemList.Where(IsItemEquipped).ToDictionary(it => it.location);

        slotDictionary.Values.ToList().ForEach(slot => slot.SetItem(null));
        foreach (var itemKey in equippedItems.Keys) {
            foreach(var slotKey in slotDictionary.Keys) {
                if ((itemKey & (int)slotKey) > 0) { 
                    slotDictionary[slotKey].SetItem(equippedItems[itemKey]);
                }
            }
        }
    }

    private bool IsItemEquipped(ItemInfo it) {
        return it.wearState > 0 &&
            it.itemType != (int)ItemType.AMMO &&
            it.itemType != (int)ItemType.CARD;
    }

    internal void UnequipAmmo() {
        slots.Find(it => it.location == EquipLocation.AMMO).SetItem(null);
    }

    public void EquipAmmo(ItemInfo item) {
        slots.Find(it => it.location == EquipLocation.AMMO).SetItem(item);
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

public class NormalEquipmentWindow : MonoBehaviour {

    public List<UIEquipSlot> slots;

    public void UpdateEquipment() {
        var entity = Core.Session.Entity;
        slots.ForEach(slot => {
            switch(slot.location) {
                case EquipLocation.HEAD_TOP:
                    slot.SetItem(2302);
                    break;
                case EquipLocation.HEAD_MID:
                    slot.SetItem(2302);
                    break;
                case EquipLocation.HEAD_BOTTOM:
                    slot.SetItem(2302);
                    break;
                case EquipLocation.ARMOR:
                    slot.SetItem(2302);
                    break;
                case EquipLocation.WEAPON:
                    slot.SetItem(2302);
                    break;
                case EquipLocation.SHIELD:
                    slot.SetItem(2302);
                    break;
                case EquipLocation.SHOES:
                    slot.SetItem(2302);
                    break;
                case EquipLocation.GARMENT:
                    slot.SetItem(2302);
                    break;
                case EquipLocation.ACCESSORY1:
                    slot.SetItem(2302);
                    break;
                case EquipLocation.ACCESSORY2:
                    slot.SetItem(2302);
                    break;
                case EquipLocation.AMMO:
                    break;
            }
        });
    }
}
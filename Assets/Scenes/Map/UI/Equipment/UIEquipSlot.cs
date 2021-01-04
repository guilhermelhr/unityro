using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEquipSlot : GenericUIItem {

    public RawImage icon;
    public TextMeshProUGUI itemName;
    public EquipLocation location;

    public void SetItem(ItemInfo itemInfo) {
        this.itemInfo = itemInfo;
        if (itemInfo != null) {
            icon.enabled = true;
            icon.texture = itemInfo.texture;
            itemName.text = itemInfo.item.identifiedDisplayName;
        } else {
            icon.enabled = false;
            icon.texture = null;
            itemName.text = "";
        }
    }
}
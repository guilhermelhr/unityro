using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEquipSlot : GenericUIItem {

    public RawImage icon;
    public TextMeshProUGUI itemName;
    public EquipLocation location;

    public void SetItem(Item item) {
        this.item = item;
        icon.enabled = true;
        icon.texture = item.texture;
        itemName.text = item.identifiedDisplayName;
    }
}
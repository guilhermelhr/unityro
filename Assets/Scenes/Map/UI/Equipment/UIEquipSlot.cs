using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEquipSlot : MonoBehaviour {

    public Image icon;
    public TextMeshProUGUI name;
    public EquipLocation location;

    public Item item;

    public void SetItem(int itemID) {
        var item = DBManager.GetItemInfo(itemID);
        var itemPath = DBManager.GetItemPath(itemID, true);
        var SPR = FileManager.Load(itemPath + ".spr") as SPR;
        if (SPR == null) return;

        SPR.SwitchToRGBA();

        var sprite = SPR.GetSprites()[0];
        icon.sprite = sprite;
        name.text = item.identifiedDisplayName;
    }

    public void SetItem(Item item) {
        this.item = item;
        icon.enabled = true;
        icon.sprite = item.sprite;
        name.text = item.identifiedDisplayName;
    }
}
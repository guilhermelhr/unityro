using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItem : GenericUIItem {

    [SerializeField]
    private RawImage itemImage;

    [SerializeField]
    private TextMeshProUGUI amountLabel;

    public void SetItem(ItemInfo itemInfo) {
        this.itemInfo = itemInfo;
        if (itemInfo != null) {
            itemImage.enabled = true;
            itemImage.texture = itemInfo.res;
            amountLabel.enabled = itemInfo.amount > 0;
            amountLabel.text = "" + itemInfo.amount;
        } else {
            itemImage.enabled = false;
            itemImage.texture = null;
            amountLabel.enabled = false;
            amountLabel.text = "";
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItem : GenericUIItem {

    [SerializeField]
    private RawImage itemImage;

    [SerializeField]
    private TextMeshProUGUI amountLabel;

    public void SetItem(Item item) {
        this.item = item;
        if(item != null) {
            itemImage.enabled = true;
            itemImage.texture = item.texture;
            amountLabel.enabled = item.info.amount > 0;
            amountLabel.text = "" + item.info.amount;
        } else {
            itemImage.enabled = false;
            itemImage.texture = null;
            amountLabel.enabled = false;
            amountLabel.text = "";
        }
    }
}

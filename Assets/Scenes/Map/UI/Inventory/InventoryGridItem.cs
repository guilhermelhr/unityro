using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryGridItem : MonoBehaviour {

    [SerializeField]
    private RawImage itemImage;

    [SerializeField]
    private TextMeshProUGUI amountLabel;

    private Item item;

    private void Start() {
        
    }

    public void SetItem(Item item) {
        this.item = item;
        itemImage.enabled = true;
        itemImage.texture = item.texture;
        amountLabel.enabled = item.info.amount > 0;
        amountLabel.text = "" + item.info.amount;
    }
}

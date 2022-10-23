using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailsWindow : DraggableUIWindow {

    [SerializeField]
    private TextMeshProUGUI ItemName;

    [SerializeField]
    private RawImage ItemCollectionImage;

    [SerializeField]
    private Transform ScrollView;

    [SerializeField]
    private GameObject TextPrefab;

    public void SetItem(ItemInfo itemInfo) {
        ItemCollectionImage.texture = itemInfo.collection;
        ItemName.text = itemInfo.IsIdentified ? itemInfo.item.identifiedDisplayName : itemInfo.item.unidentifiedDisplayName;
        var text = itemInfo.IsIdentified ? itemInfo.item.identifiedDescriptionName : itemInfo.item.unidentifiedDescriptionName;

        foreach(var line in text.Split('\n')) {
            var go = Instantiate(TextPrefab, ScrollView.transform);
            var t = go.GetComponentInChildren<TextMeshProUGUI>();
            t.text = line.Replace("\"", "");
            t.overflowMode = TextOverflowModes.Overflow;
            t.enableWordWrapping = true;
        }
    }

    public void Close() {
        Destroy(this.gameObject);
    }
}

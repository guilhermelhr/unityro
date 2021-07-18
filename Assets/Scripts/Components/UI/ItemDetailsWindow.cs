using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class ItemDetailsWindow : MonoBehaviour {

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
            var go = Instantiate(TextPrefab);
            var t = go.GetComponentInChildren<TextMeshProUGUI>();
            t.text = line.Replace("\"", "");
            t.overflowMode = TextOverflowModes.Overflow;
            t.enableWordWrapping = true;
            t.transform.SetParent(ScrollView.transform);
        }
    }

    public void Close() {
        Destroy(this.gameObject);
    }
}

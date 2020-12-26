using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupController : MonoBehaviour {

    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI label;

    public void DisplayPopup(Sprite sprite, string label) {
        image.sprite = sprite;
        image.preserveAspect = true;
        this.label.text = label;

        gameObject.SetActive(true);
        StartCoroutine(TearDown());
    }

    private IEnumerator TearDown() {
        yield return new WaitForSeconds(3);

        image.sprite = null;
        label.text = "";
        gameObject.SetActive(false);
    }
}

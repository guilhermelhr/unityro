﻿using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupController : MonoBehaviour {

    [SerializeField] private RawImage image;
    [SerializeField] private TextMeshProUGUI label;

    public void DisplayPopup(Texture2D texture, string label) {
        image.texture = texture;
        this.label.text = label;

        gameObject.SetActive(true);
        StartCoroutine(TearDown());
    }

    private IEnumerator TearDown() {
        yield return new WaitForSeconds(3);

        image.texture = null;
        label.text = "";
        gameObject.SetActive(false);
    }
}

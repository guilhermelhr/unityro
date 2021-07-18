using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tab : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI label;

    public void SetLabel(string label) => this.label.text = label;
}

using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class CharServerListItemController : MonoBehaviour {

    private CharServerInfo charServerInfo;
    public Action<CharServerInfo> OnCharServerSelected;
    public Text charServerName;
    private Toggle toggle;

    public void BindData(CharServerInfo charServerInfo) {
        this.charServerInfo = charServerInfo;
        this.charServerName.text = charServerInfo.Name;
    }

    private void Awake() {
        toggle = GetComponent<Toggle>();
    }

    public void OnSelect() {
        toggle.isOn = true;
    }

    public void OnValueChanged() {
        if(toggle.isOn)
            OnCharServerSelected?.Invoke(charServerInfo);
    }
}
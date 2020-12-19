using System;
using UnityEngine;
using UnityEngine.UI;

public class CharServerListItemController : MonoBehaviour {

    private CharServerInfo charServerInfo;
    public Action<CharServerInfo> OnCharServerSelected;
    public Text charServerName;

    public void BindData(CharServerInfo charServerInfo) {
        this.charServerInfo = charServerInfo;
        this.charServerName.text = charServerInfo.Name;
    }

    private void Update() {
        if (Input.GetMouseButtonUp(0)) {
            OnCharServerSelected?.Invoke(charServerInfo);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUiController : MonoBehaviour {

    [SerializeField] private NpcBoxController NpcBox;
    [SerializeField] private NpcBoxMenuController NpcMenu;
    [SerializeField] private PopupController PopupController;

    void Awake() {
        #region NPCS
        Core.NetworkClient.HookPacket(ZC.SAY_DIALOG.HEADER, NpcBox.OnNpcMessage);
        Core.NetworkClient.HookPacket(ZC.CLOSE_DIALOG.HEADER, NpcBox.AddCloseButton);
        Core.NetworkClient.HookPacket(ZC.WAIT_DIALOG.HEADER, NpcBox.AddNextButton);
        Core.NetworkClient.HookPacket(ZC.CLOSE_SCRIPT.HEADER, NpcBox.CloseAndReset);
        Core.NetworkClient.HookPacket(ZC.MENU_LIST.HEADER, NpcMenu.SetMenu);
        #endregion

        NpcMenu.OnNpcMenuSelected = OnNpcMenuSelected;
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnNpcMenuSelected(uint NAID, byte index) {
        if (index == 255) {
            NpcBox.gameObject.SetActive(false);
        }

        new CZ.CHOOSE_MENU() {
            NAID = NAID,
            Index = index
        }.Send();
    }

    public void DisplayPopup(Sprite sprite, string label) {
        PopupController.DisplayPopup(sprite, label);
    }
}

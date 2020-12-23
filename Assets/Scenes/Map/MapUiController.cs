using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUiController : MonoBehaviour {

    [SerializeField] private NpcBoxController NpcBox;
    [SerializeField] private NpcBoxMenuController NpcMenu;
    [SerializeField] private PopupController PopupController;
    [SerializeField] public EquipmentWindowController EquipmentWindow;

    private HashSet<KeyCode> CurrentlyPressedKeys = new HashSet<KeyCode>();

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

    private void OnGUI() {
        if(!Event.current.isKey || Event.current.keyCode == KeyCode.None) return;
        switch(Event.current.type) {
            case EventType.KeyDown:
                CurrentlyPressedKeys.Add(Event.current.keyCode);
                break;
            case EventType.KeyUp:
                CurrentlyPressedKeys.Remove(Event.current.keyCode);
                break;
            default:
                break;
        }

        if (CurrentlyPressedKeys.Count == 2) {
            if (CurrentlyPressedKeys.Contains(KeyCode.LeftAlt) && CurrentlyPressedKeys.Contains(KeyCode.Q)) {
                EquipmentWindow.gameObject.SetActive(!EquipmentWindow.gameObject.activeInHierarchy);
            }
        }
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUiController : MonoBehaviour {

    public static MapUiController Instance;

    [SerializeField] private Tooltip Tooltip;
    [SerializeField] private NpcBoxController NpcBox;
    [SerializeField] private NpcBoxMenuController NpcMenu;
    [SerializeField] private PopupController PopupController;
    [SerializeField] public EquipmentWindowController EquipmentWindow;
    [SerializeField] public InventoryWindowController InventoryWindow;
    [SerializeField] public StatsWindowController StatsWindow;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }

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
        if (!Event.current.isKey || Event.current.keyCode == KeyCode.None) return;
        switch (Event.current.type) {
            case EventType.KeyDown:
                if (Event.current.modifiers == EventModifiers.Alt) {

                    switch (Event.current.keyCode) {
                        case KeyCode.Q:
                            EquipmentWindow.ToggleActive();
                            break;
                        case KeyCode.E:
                            InventoryWindow.ToggleActive();
                            break;
                        case KeyCode.A:
                            StatsWindow.ToggleActive();
                            break;
                        default:
                            break;
                    }
                }
                break;
            default:
                break;
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

    public void DisplayPopup(Texture2D itemRes, string label) {
        PopupController.DisplayPopup(itemRes, label);
    }

    public void UpdateEquipment() {
        EquipmentWindow.UpdateEquipment();
        InventoryWindow.UpdateEquipment();
    }

    public void DisplayTooltip(string text, Vector3 position) {
        Tooltip.SetText(text, position);
    }

    public void HideTooltip() {
        Tooltip.SetText(null, Vector3.zero);
    }
}

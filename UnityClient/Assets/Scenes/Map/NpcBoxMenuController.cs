using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;

public class NpcBoxMenuController : DraggableUIWindow {

    public Action<uint, byte> OnNpcMenuSelected;

    [SerializeField] private Transform LinearLayout;
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private Button nextButton;

    private uint OwnerID;
    private List<string> MenuValues = new List<string>();
    private List<MenuItemController> MenuItems = new List<MenuItemController>();
    private int SelectedIndex;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void SetMenu(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.MENU_LIST) {
            var pkt = packet as ZC.MENU_LIST;

            gameObject.SetActive(true);

            OwnerID = pkt.NAID;
            var options = pkt.Message.Split(':');
            MenuValues.Clear();
            MenuValues.AddRange(options);

            foreach (var option in MenuValues) {
                var menuItem = Instantiate(textPrefab).GetOrAddComponent<MenuItemController>();
                menuItem.Init(option);
                menuItem.OnItemSelected = OnItemSelected;

                menuItem.transform.SetParent(LinearLayout, false);
                MenuItems.Add(menuItem);
            }
        }
    }

    void OnItemSelected(string option) {
        SelectedIndex = MenuValues.IndexOf(option) + 1;
        foreach (var item in MenuItems) {
            item.highlighted = item.value == option;
        }
    }

    public void OnCancelClick() {
        OnNpcMenuSelected?.Invoke(OwnerID, 255);

        TearDown();
    }

    public void OnOkClicked() {
        OnNpcMenuSelected?.Invoke(OwnerID, (byte)SelectedIndex);

        TearDown();
    }

    private void TearDown() {
        gameObject.SetActive(false);
        MenuValues.Clear();
        MenuItems.Clear();

        foreach (Transform child in LinearLayout.transform) {
            Destroy(child.gameObject);
        }
    }

    internal class MenuItemController : MonoBehaviour, IPointerDownHandler {

        public Action<string> OnItemSelected;
        public string value;
        public bool highlighted = false;

        private TextMeshProUGUI textField;
        private Image background;

        public void Init(string value) {
            this.value = value;
            textField = GetComponentInChildren<TextMeshProUGUI>();
            background = GetComponent<Image>();
        }

        public void OnPointerDown(PointerEventData eventData) {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                OnItemSelected?.Invoke(value);
            }
        }

        private void Update() {
            textField.color = Color.black;
            textField.text = value;
            if (highlighted) {
                background.color = new Color(0, 1, 1, 0.3f);
            } else {
                background.color = new Color(0, 1, 1, 0);
            }
        }
    }
}

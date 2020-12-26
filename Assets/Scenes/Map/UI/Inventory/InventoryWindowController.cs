using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryWindowController : MonoBehaviour {

    [SerializeField]
    private GridLayoutGroup GridLayout;

    [SerializeField]
    private InventoryGridItem GridItemPrefab;

    public const int MIN_WINDOW_COLUMNS = 6;
    public const int MAX_WINDOW_COLUMNS = 8;
    public const int MIN_WINDOW_ROWS = 6;

    private List<InventoryGridItem> Items = new List<InventoryGridItem>();

    private void Awake() {
        if(Items.IsEmpty()) {
            InitGrid();
        }
    }

    private void InitGrid() {
        for(int i = 0; i < MIN_WINDOW_COLUMNS * MIN_WINDOW_ROWS; i++) {
            var item = Instantiate<InventoryGridItem>(GridItemPrefab);
            item.transform.SetParent(GridLayout.transform, false);
            Items.Add(item);
        }
    }

    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void UpdateEquipment() {
        if (Items.IsEmpty()) {
            InitGrid();
        }

        var inventory = Core.Session.Entity.Inventory;
        if(inventory == null || inventory.IsEmpty()) return;

        var filteredInventory = inventory.Where(it => it.info.wearState <= 0).ToList();
        for(int i = 0; i < Items.Count; i++) {
            if(i >= filteredInventory.Count) break;

            Items[i].SetItem(filteredInventory[i]);
        }
    }
}

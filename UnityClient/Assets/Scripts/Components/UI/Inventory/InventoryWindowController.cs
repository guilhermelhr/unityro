﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryWindowController : DraggableUIWindow {

    [SerializeField]
    private GridLayoutGroup GridLayout;

    [SerializeField]
    private InventoryCell GridCellPrefab;

    [SerializeField]
    private UIItem UIItemPrefab;

    [SerializeField]
    private InventoryType CurrentTab = InventoryType.ITEM;

    [SerializeField]
    private CustomPanel Tabs;

    [SerializeField]
    private string ResName;

    public const int MIN_WINDOW_COLUMNS = 6;
    public const int MAX_WINDOW_COLUMNS = 8;
    public const int MIN_WINDOW_ROWS = 6;

    private List<InventoryCell> Cells = new List<InventoryCell>();
    private int CURRENT_WINDOW_COLUMNS = MAX_WINDOW_COLUMNS;

    private void Awake() {
        if (Cells.IsEmpty()) {
            InitGrid();
        }
    }

    private void InitGrid() {
        for (int i = 0; i < CURRENT_WINDOW_COLUMNS * CURRENT_WINDOW_COLUMNS; i++) {
            var cell = Instantiate<InventoryCell>(GridCellPrefab);
            cell.transform.SetParent(GridLayout.transform, false);
            Cells.Add(cell);
        }
    }

    public void UpdateEquipment() {
        if (Cells.IsEmpty()) {
            InitGrid();
        }

        var inventory = (Session.CurrentSession.Entity as Entity).Inventory;
        if (inventory == null || inventory.IsEmpty) return;

        var filteredInventory = inventory.ItemList.Where(it => (it.wearState <= 0 || it.itemType == (int)ItemType.AMMO) && it.tab == CurrentTab).ToList();
        for (int i = 0; i < Cells.Count; i++) {
            if (i < filteredInventory.Count) {
                Cells[i].SetItem(filteredInventory[i]);
            } else {
                Cells[i].SetItem(null);
            }
        }
    }

    public void ChangeCurrentTab(int newTab) {
        if ((InventoryType)newTab != CurrentTab) {
            CurrentTab = (InventoryType)newTab;
        }

        Tabs.SetBackground($"{ResName}{(int)CurrentTab + 1}.png");
        UpdateEquipment();
    }
}

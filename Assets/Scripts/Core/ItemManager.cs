using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour {

    private void Awake() {
        Core.NetworkClient.HookPacket(ZC.ITEM_FALL_ENTRY5.HEADER, OnItemSpamInGround);
        Core.NetworkClient.HookPacket(ZC.ITEM_ENTRY.HEADER, OnItemSpamInGround);
        Core.NetworkClient.HookPacket(ZC.ITEM_PICKUP_ACK7.HEADER, OnItemPickup);
        Core.NetworkClient.HookPacket(ZC.ITEM_DISAPPEAR.HEADER, OnItemDisappear);
        Core.NetworkClient.HookPacket(ZC.INVENTORY_ITEMLIST_EQUIP.HEADER, OnInventoryUpdate);
        Core.NetworkClient.HookPacket(ZC.INVENTORY_ITEMLIST_NORMAL.HEADER, OnInventoryUpdate);
        Core.NetworkClient.HookPacket(ZC.USE_ITEM_ACK2.HEADER, OnUseItemAnswer);
        Core.NetworkClient.HookPacket(ZC.ACK_WEAR_EQUIP_V5.HEADER, OnItemEquipAnswer);
    }

    private void OnItemEquipAnswer(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.ACK_WEAR_EQUIP_V5 USE_ITEM_ACK2) {
            if (USE_ITEM_ACK2.result == 0) {
                Core.Session.Entity.Inventory.EquipItem(USE_ITEM_ACK2.index, USE_ITEM_ACK2.equipLocation);
                MapUiController.Instance.UpdateEquipment();
            } else {
                //TODO display error message
            }
        }
    }

    private void OnUseItemAnswer(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.USE_ITEM_ACK2 USE_ITEM_ACK2) {
            if (USE_ITEM_ACK2.AID == Core.Session.AccountID && USE_ITEM_ACK2.result > 0) {
                Core.Session.Entity.Inventory.UpdateItem(USE_ITEM_ACK2.index, USE_ITEM_ACK2.count);
            }
        }
    }

    private void OnInventoryUpdate(ushort cmd, int size, InPacket packet) {
        var list = new List<ItemInfo>();
        if (packet is ZC.INVENTORY_ITEMLIST_EQUIP) {
            var pkt = packet as ZC.INVENTORY_ITEMLIST_EQUIP;
            list = pkt.Inventory;
        } else if (packet is ZC.INVENTORY_ITEMLIST_NORMAL) {
            var pkt = packet as ZC.INVENTORY_ITEMLIST_NORMAL;
            list = pkt.Inventory;
        }

        if (list.IsEmpty()) return;

        // TODO apply a diff here
        // TODO find out how favorite tab works
        foreach (var itemInfo in list) {
            var item = DBManager.GetItemInfo(itemInfo.ItemID);
            if (item == null) continue;
            var texture = FileManager.Load(DBManager.GetItemResPath(item, itemInfo.IsIdentified)) as Texture2D;

            itemInfo.item = item;
            itemInfo.texture = texture;
            itemInfo.tab = FindItemTab(itemInfo);
            Core.Session.Entity.Inventory.AddItem(itemInfo);
        }
        MapController.Instance.UIController.UpdateEquipment();
    }

    private InventoryType FindItemTab(ItemInfo item) {
        switch ((ItemType)item.itemType) {
            case ItemType.HEALING:
            case ItemType.USABLE:
            case ItemType.USABLE_SKILL:
            case ItemType.USABLE_UNK:
                return InventoryType.ITEM;

            case ItemType.WEAPON:
            case ItemType.EQUIP:
            case ItemType.PETEGG:
            case ItemType.PETEQUIP:
                return InventoryType.EQUIP;

            default:
            case ItemType.ETC:
            case ItemType.CARD:
            case ItemType.AMMO:
                return InventoryType.ETC;
        }
    }

    private void OnItemDisappear(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.ITEM_DISAPPEAR) {
            var pkt = packet as ZC.ITEM_DISAPPEAR;

            Core.EntityManager.RemoveEntity(pkt.GID);
        }
    }

    private void OnItemPickup(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.ITEM_PICKUP_ACK7) {
            var pkt = packet as ZC.ITEM_PICKUP_ACK7;

            if (pkt.result != 0) {
                Debug.Log("Failed to pick item");
                return;
            }

            var itemInfo = pkt.itemInfo;

            Item item = DBManager.GetItemInfo(itemInfo.ItemID);
            itemInfo.item = item;

            Texture2D itemRes = FileManager.Load(DBManager.GetItemResPath(item, itemInfo.IsIdentified)) as Texture2D;
            itemInfo.texture = itemRes;

            itemInfo.tab = FindItemTab(itemInfo);

            Core.Session.Entity.Inventory.AddItem(itemInfo);
            MapController.Instance.UIController.UpdateEquipment();

            DisplayPopup(itemInfo);
        }
    }

    private void DisplayPopup(ItemInfo itemInfo) {
        var label = $"{(itemInfo.IsIdentified ? itemInfo.item.identifiedDisplayName : itemInfo.item.unidentifiedDisplayName)} - {itemInfo.amount} obtained";
        MapController.Instance.UIController.DisplayPopup(itemInfo.texture, label);
    }

    private void OnItemSpamInGround(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.ITEM_FALL_ENTRY5) {
            var pkt = packet as ZC.ITEM_FALL_ENTRY5;

            var x = pkt.x - 0.5 + pkt.subX / 12;
            var z = pkt.y - 0.5 + pkt.subY / 12;
            var y = Core.PathFinding.GetCellHeight((int)x, (int)z) + 5.0;

            Core.EntityManager.SpawnItem(new ItemSpawnInfo() {
                GID = pkt.id,
                mapID = pkt.mapID,
                Position = new Vector3((float)x, (float)y, (float)z),
                amount = pkt.amount,
                IsIdentified = pkt.identified == 1,
                dropEffectMode = pkt.dropEffectMode,
                showDropEffect = pkt.showDropEffect,
                animate = true
            });
        } else if (packet is ZC.ITEM_ENTRY ITEM_ENTRY) {
            var x = ITEM_ENTRY.x - 0.5 + ITEM_ENTRY.subX / 12;
            var z = ITEM_ENTRY.y - 0.5 + ITEM_ENTRY.subY / 12;
            var y = Core.PathFinding.GetCellHeight((int)x, (int)z);

            Core.EntityManager.SpawnItem(new ItemSpawnInfo() {
                GID = ITEM_ENTRY.id,
                mapID = ITEM_ENTRY.mapID,
                Position = new Vector3((float)x, (float)y, (float)z),
                amount = ITEM_ENTRY.amount,
                IsIdentified = ITEM_ENTRY.identified == 1,
                animate = false
            });
        }
    }
}

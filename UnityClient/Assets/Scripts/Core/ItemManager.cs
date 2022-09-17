using ROIO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ItemManager : MonoBehaviour {

    private NetworkClient NetworkClient;
    private EntityManager EntityManager;
    private PathFinder PathFinding;

    private void Awake() {
        DontDestroyOnLoad(this);

        NetworkClient = FindObjectOfType<NetworkClient>();
        EntityManager = FindObjectOfType<EntityManager>();
        PathFinding = FindObjectOfType<PathFinder>();
    }

    private void Start() {
        NetworkClient.HookPacket(ZC.ITEM_FALL_ENTRY5.HEADER, OnItemSpamInGround);
        NetworkClient.HookPacket(ZC.ITEM_ENTRY.HEADER, OnItemSpamInGround);
        NetworkClient.HookPacket(ZC.ITEM_PICKUP_ACK7.HEADER, OnItemPickup);
        NetworkClient.HookPacket(ZC.ITEM_DISAPPEAR.HEADER, OnItemDisappear);
        NetworkClient.HookPacket(ZC.INVENTORY_ITEMLIST_EQUIP.HEADER, OnInventoryUpdate);
        NetworkClient.HookPacket(ZC.INVENTORY_ITEMLIST_NORMAL.HEADER, OnInventoryUpdate);
        NetworkClient.HookPacket(ZC.USE_ITEM_ACK2.HEADER, OnUseItemAnswer);
        NetworkClient.HookPacket(ZC.ACK_WEAR_EQUIP_V5.HEADER, OnItemEquipAnswer);
        NetworkClient.HookPacket(ZC.ACK_TAKEOFF_EQUIP_V5.HEADER, OnItemTakeOffAnswer);
        NetworkClient.HookPacket(ZC.DELETE_ITEM_FROM_BODY.HEADER, OnInventoryRemoveItem);
        NetworkClient.HookPacket(ZC.EQUIP_ARROW.HEADER, OnEquipAmmo);
    }

    private void OnEquipAmmo(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.EQUIP_ARROW EQUIP_ARROW) {
            (Session.CurrentSession.Entity as Entity).Inventory.EquipItem(EQUIP_ARROW.Index, (int) EquipLocation.AMMO);
        }
    }

    private void OnInventoryRemoveItem(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.DELETE_ITEM_FROM_BODY DELETE_ITEM_FROM_BODY) {
            var item = (Session.CurrentSession.Entity as Entity).Inventory.RemoveItem((short) DELETE_ITEM_FROM_BODY.Index, (short) DELETE_ITEM_FROM_BODY.Count);
            if (item.amount <= 1) {
                MapUiController.Instance.EquipmentWindow.UnequipAmmo();
            }
            MapUiController.Instance.UpdateEquipment();
        }
    }

    private void OnItemTakeOffAnswer(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.ACK_TAKEOFF_EQUIP_V5 ACK_TAKEOFF_EQUIP_V5) {
            if (ACK_TAKEOFF_EQUIP_V5.result == 0) {
                (Session.CurrentSession.Entity as Entity).Inventory.TakeOffItem(ACK_TAKEOFF_EQUIP_V5.index, ACK_TAKEOFF_EQUIP_V5.equipLocation);
                MapUiController.Instance.UpdateEquipment();
            } else {
                //TODO display error message
            }
        }
    }

    private void OnItemEquipAnswer(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.ACK_WEAR_EQUIP_V5 ACK_WEAR_EQUIP_V5) {
            if (ACK_WEAR_EQUIP_V5.result == 0) {
                (Session.CurrentSession.Entity as Entity).Inventory.EquipItem(ACK_WEAR_EQUIP_V5.index, ACK_WEAR_EQUIP_V5.equipLocation);
                MapUiController.Instance.UpdateEquipment();
            } else {
                MapController.Instance.UIController.ChatBox.DisplayMessage(372, 0);
            }
        }
    }

    private void OnUseItemAnswer(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.USE_ITEM_ACK2 USE_ITEM_ACK2) {
            if (USE_ITEM_ACK2.AID == Session.CurrentSession.AccountID && USE_ITEM_ACK2.result > 0) {
                (Session.CurrentSession.Entity as Entity).Inventory.UpdateItem(USE_ITEM_ACK2.index, USE_ITEM_ACK2.count);
            }
        }
    }

    private async void OnInventoryUpdate(ushort cmd, int size, InPacket packet) {
        var list = new List<ItemInfo>();
        if (packet is ZC.INVENTORY_ITEMLIST_EQUIP) {
            var pkt = packet as ZC.INVENTORY_ITEMLIST_EQUIP;
            list = pkt.Inventory;
        } else if (packet is ZC.INVENTORY_ITEMLIST_NORMAL) {
            var pkt = packet as ZC.INVENTORY_ITEMLIST_NORMAL;
            list = pkt.Inventory;
        }

        if (list.IsEmpty())
            return;

        // TODO apply a diff here
        // TODO find out how favorite tab works
        foreach (var itemInfo in list) {
            var item = DBManager.GetItem(itemInfo.ItemID);
            if (item == null)
                continue;
            var res = await Addressables.LoadAssetAsync<Texture2D>(DBManager.GetItemResPath(item, itemInfo.IsIdentified)).Task;
            var collection = await Addressables.LoadAssetAsync<Texture2D>(DBManager.GetItemCollectionPath(item, itemInfo.IsIdentified)).Task;

            itemInfo.item = item;
            itemInfo.res = res;
            itemInfo.collection = collection;
            itemInfo.tab = FindItemTab(itemInfo);
            (Session.CurrentSession.Entity as Entity).Inventory.AddItem(itemInfo);
        }
        MapController.Instance.UIController.UpdateEquipment();
    }

    private InventoryType FindItemTab(ItemInfo item) {
        switch ((ItemType) item.itemType) {
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

            EntityManager.RemoveEntity(pkt.AID);
        }
    }

    private async void OnItemPickup(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.ITEM_PICKUP_ACK7 ITEM_PICKUP_ACK7) {

            if (ITEM_PICKUP_ACK7.result != 0) {
                Debug.Log("Failed to pick item");
                return;
            }

            var itemInfo = ITEM_PICKUP_ACK7.itemInfo;

            Item item = DBManager.GetItem(itemInfo.ItemID);
            itemInfo.item = item;

            Texture2D itemRes = await Addressables.LoadAssetAsync<Texture2D>(DBManager.GetItemResPath(item, itemInfo.IsIdentified)).Task;
            itemInfo.res = itemRes;

            itemInfo.tab = FindItemTab(itemInfo);

            (Session.CurrentSession.Entity as Entity).Inventory.AddItem(itemInfo);
            MapController.Instance.UIController.UpdateEquipment();

            MapController.Instance.UIController.ChatBox.DisplayMessage(153, ChatMessageType.BLUE,
                new KeyValuePair<string, string>("%s", itemInfo.IsIdentified ? item.identifiedDisplayName : item.unidentifiedDisplayName),
                new KeyValuePair<string, string>("%d", ITEM_PICKUP_ACK7.itemInfo.amount.ToString())
            );

            DisplayPopup(itemInfo);
        }
    }

    private void DisplayPopup(ItemInfo itemInfo) {
        var label = $"{(itemInfo.IsIdentified ? itemInfo.item.identifiedDisplayName : itemInfo.item.unidentifiedDisplayName)} - {itemInfo.amount} obtained";
        MapController.Instance.UIController.DisplayPopup(itemInfo.res, label);
    }

    private void OnItemSpamInGround(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.ITEM_FALL_ENTRY5) {
            var pkt = packet as ZC.ITEM_FALL_ENTRY5;

            var x = pkt.x - 0.5 + pkt.subX / 12;
            var z = pkt.y - 0.5 + pkt.subY / 12;
            var y = PathFinding.GetCellHeight((int) x, (int) z) + 5.0;

            EntityManager.SpawnItem(new ItemSpawnInfo() {
                AID = pkt.id,
                mapID = pkt.mapID,
                Position = new Vector3((float) x, (float) y, (float) z),
                amount = pkt.amount,
                IsIdentified = pkt.identified == 1,
                dropEffectMode = pkt.dropEffectMode,
                showDropEffect = pkt.showDropEffect,
                animate = true
            });
        } else if (packet is ZC.ITEM_ENTRY ITEM_ENTRY) {
            var x = ITEM_ENTRY.x - 0.5 + ITEM_ENTRY.subX / 12;
            var z = ITEM_ENTRY.y - 0.5 + ITEM_ENTRY.subY / 12;
            var y = PathFinding.GetCellHeight((int) x, (int) z) + 1.0;

            EntityManager.SpawnItem(new ItemSpawnInfo() {
                AID = ITEM_ENTRY.id,
                mapID = ITEM_ENTRY.mapID,
                Position = new Vector3((float) x, (float) y, (float) z),
                amount = ITEM_ENTRY.amount,
                IsIdentified = ITEM_ENTRY.identified == 1,
                animate = false
            });
        }
    }
}

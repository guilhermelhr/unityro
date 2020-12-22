using System;
using UnityEngine;

public class ItemManager : MonoBehaviour {

    private void Awake() {
        Core.NetworkClient.HookPacket(ZC.ITEM_FALL_ENTRY5.HEADER, OnItemSpamInGround);
        Core.NetworkClient.HookPacket(ZC.ITEM_PICKUP_ACK7.HEADER, OnItemPickup);
    }

    private void OnItemPickup(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.ITEM_PICKUP_ACK7) {
            var pkt = packet as ZC.ITEM_PICKUP_ACK7;

            if (pkt.result != 0) {
                Debug.Log("Failed to pick item");
            }
        }
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
                showDropEffect = pkt.showDropEffect
            });
        }
    }
}

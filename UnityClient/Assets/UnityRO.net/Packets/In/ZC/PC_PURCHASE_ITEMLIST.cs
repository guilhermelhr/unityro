using ROIO.Utils;
using System.Collections.Generic;
using UnityEngine;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_PC_PURCHASE_ITEMLIST")]
    public class PC_PURCHASE_ITEMLIST : InPacket {

        private const int BLOCK_SIZE = 13; // PACKET_ZC_PC_PURCHASE_ITEMLIST_sub

        public const PacketHeader HEADER = PacketHeader.ZC_PC_PURCHASE_ITEMLIST;

        public List<ItemNPCShopInfo> ItemList;

        public void Read(MemoryStreamReader br, int size) {
            var count = (br.Length - br.Position) / BLOCK_SIZE;
            ItemList = new List<ItemNPCShopInfo>();

            for (int i = 0; i < count; i++) {
                var price = br.ReadInt();
                var discount = br.ReadInt();
                var type = (byte)br.ReadByte();
                var itemID = br.ReadInt();

                ItemList.Add(new ItemNPCShopInfo {
                    price = price,
                    discount = discount,
                    type = type,
                    itemID = itemID
                });
            }
        }

        public class ItemNPCShopInfo {
            public int price;
            public int discount;
            public byte type;
            public int itemID;
        }
    }
}
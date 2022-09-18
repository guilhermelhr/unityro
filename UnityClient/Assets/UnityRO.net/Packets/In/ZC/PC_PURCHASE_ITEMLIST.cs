using ROIO.Utils;
using System.Collections.Generic;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_PC_PURCHASE_ITEMLIST")]
    public class PC_PURCHASE_ITEMLIST : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_PC_PURCHASE_ITEMLIST;
        private const int BLOCK_SIZE = 13; // PACKET_ZC_PC_PURCHASE_ITEMLIST_sub
        public PacketHeader Header => HEADER;

        public List<ItemNPCShopInfo> ItemList;

        public void Read(MemoryStreamReader br, int size) {
            var count = (br.Length - br.Position) / BLOCK_SIZE;
            ItemList = new List<ItemNPCShopInfo>();

            for (int i = 0; i < count; i++) {
                var price = br.ReadInt();
                var discount = br.ReadInt();
                var type = br.ReadByte();
                var itemID = br.ReadInt();

                ItemList.Add(new ItemNPCShopInfo {
                    price = price,
                    specialPrice = discount,
                    type = type,
                    itemID = itemID
                });
            }
        }
    }

    [PacketHandler(HEADER, "ZC_PC_SELL_ITEMLIST")]
    public class PC_SELL_ITEMLIST : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_PC_SELL_ITEMLIST;
        public const int BLOCK_SIZE = 10;
        public PacketHeader Header => HEADER;

        public List<ItemNPCShopInfo> ItemList;

        public void Read(MemoryStreamReader br, int size) {
            var count = (br.Length - br.Position) / BLOCK_SIZE;
            ItemList = new List<ItemNPCShopInfo>();

            for (int i = 0; i < count; i++) {
                var index = br.ReadShort();
                var price = br.ReadInt();
                var specialPrice = br.ReadInt();

                ItemList.Add(new ItemNPCShopInfo {
                    price = price,
                    specialPrice = specialPrice,
                    inventoryIndex = index,
                });
            }
        }
    }
}

public class ItemNPCShopInfo {
    public int price;
    public int specialPrice;
    public int type;
    public int itemID;
    public int inventoryIndex;
}
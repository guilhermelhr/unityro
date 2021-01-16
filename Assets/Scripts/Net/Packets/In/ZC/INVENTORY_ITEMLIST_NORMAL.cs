
using System.Collections.Generic;

public partial class ZC {

    /**
     * Non-stackable (Equippable) inventory list
     */
    [PacketHandler(HEADER, "ZC_INVENTORY_ITEMLIST_NORMAL")]
    public class INVENTORY_ITEMLIST_NORMAL : InPacket {

        private const int BLOCK_SIZE = 34;
        public const PacketHeader HEADER = PacketHeader.ZC_INVENTORY_ITEMLIST_NORMAL;

        public List<ItemInfo> Inventory = new List<ItemInfo>();

        public void Read(BinaryReader br, int size) {
            byte invType = br.ReadUByte();

            var count = (br.Length - br.Position) / BLOCK_SIZE;

            for (int i = 0; i < count; i++) {
                var itemInfo = new ItemInfo();

                itemInfo.index = br.ReadShort(); // 2
                itemInfo.ItemID = (int)br.ReadULong(); // 4
                itemInfo.itemType = br.ReadUByte(); // 1
                itemInfo.amount = br.ReadShort(); // 2
                itemInfo.wearState = (int)br.ReadULong(); // 4
                itemInfo.slot = new ItemInfo.Slot() { // 8
                    card1 = (int)br.ReadULong(),
                    card2 = (int)br.ReadULong(),
                    card3 = (int)br.ReadULong(),
                    card4 = (int)br.ReadULong()
                };
                itemInfo.expireTime = br.ReadLong(); // 4
                itemInfo.flag = br.ReadByte(); // 4

                Inventory.Add(itemInfo);
            }
        }
    }
}

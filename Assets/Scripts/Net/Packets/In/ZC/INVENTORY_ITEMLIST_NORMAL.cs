
using System.Collections.Generic;

public partial class ZC {

    /**
     * Non-stackable (Equippable) inventory list
     */
    [PacketHandler(HEADER, "ZC_INVENTORY_ITEMLIST_NORMAL")]
    public class INVENTORY_ITEMLIST_NORMAL : InPacket {

        private const int BLOCK_SIZE = 24;
        public const PacketHeader HEADER = PacketHeader.ZC_INVENTORY_ITEMLIST_NORMAL;

        public List<ItemInfo> Inventory = new List<ItemInfo>();

        //TODO UPDATE
        public void Read(BinaryReader br, int size) {

            var count = (br.Length - br.Position) / BLOCK_SIZE;

            for(int i = 0; i < count; i++) {
                var itemInfo = new ItemInfo {
                    index = br.ReadShort(),
                    ItemID = br.ReadShort(),
                    itemType = br.ReadByte(),
                    amount = br.ReadShort(),
                    wearState = br.ReadLong(),
                    slot = new ItemInfo.Slot() {
                        card1 = br.ReadUShort(),
                        card2 = br.ReadUShort(),
                        card3 = br.ReadUShort(),
                        card4 = br.ReadUShort()
                    },
                    expireTime = br.ReadLong(),
                    flag = br.ReadByte()
                };

                Inventory.Add(itemInfo);
            }
        }
    }
}


using System.Collections.Generic;

public partial class ZC {

    /**
     * Non-stackable (Equippable) inventory list
     */
    [PacketHandler(HEADER, "ZC_INVENTORY_ITEMLIST_EQUIP")]
    public class INVENTORY_ITEMLIST_EQUIP : InPacket {

        private const int BLOCK_SIZE = 67;
        public const PacketHeader HEADER = PacketHeader.ZC_INVENTORY_ITEMLIST_EQUIP;

        public List<ItemInfo> Inventory = new List<ItemInfo>();

        public void Read(BinaryReader br, int size) {
            byte invType = br.ReadUByte();

            var count = (br.Length - br.Position) / BLOCK_SIZE;

            for (int i = 0; i < count; i++) {
                var itemInfo = new ItemInfo {
                    index = br.ReadShort(),

                    ItemID = (int)br.ReadULong(),

                    itemType = br.ReadUByte(),

                    location = (int)br.ReadULong(),
                    wearState = (int)br.ReadULong(),
                    refine = br.ReadUByte(),

                    slot = new ItemInfo.Slot() {
                        card1 = (int)br.ReadULong(),
                        card2 = (int)br.ReadULong(),
                        card3 = (int)br.ReadULong(),
                        card4 = (int)br.ReadULong()
                    },

                    expireTime = br.ReadLong(),
                    bindOnEquip = br.ReadUShort(),
                    wItemSpriteNumber = br.ReadUShort(),
                    randomOptionCount = br.ReadUByte(),
                    options = new List<ItemInfo.Option>()
                };

                for (int j = 0; j < 5; j++) {
                    itemInfo.options.Add(new ItemInfo.Option() {
                        optIndex = br.ReadShort(),
                        value = br.ReadShort(),
                        param1 = br.ReadUByte()
                    });
                }

                itemInfo.flag = br.ReadByte();

                Inventory.Add(itemInfo);
            }
        }
    }
}

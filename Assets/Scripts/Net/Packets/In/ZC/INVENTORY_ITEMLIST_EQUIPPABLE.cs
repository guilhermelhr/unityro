
using System.Collections.Generic;
using System.IO;

public partial class ZC {

    /**
     * Non-stackable (Equippable) inventory list
     */
    [PacketHandler(HEADER, "ZC_INVENTORY_ITEMLIST_EQUIP")]
    public class INVENTORY_ITEMLIST_EQUIP : InPacket {

        private const int BLOCK_SIZE = 57;
        public const PacketHeader HEADER = PacketHeader.ZC_INVENTORY_ITEMLIST_EQUIP;

        public List<ItemInfo> Inventory = new List<ItemInfo>();

        public void Read(BinaryReader br, int size) {

            var count = (br.Length - br.Position) / BLOCK_SIZE;

            for(int i = 0; i < count; i++) {
                var itemInfo = new ItemInfo {
                    index = br.ReadShort(),
                    ItemID = br.ReadShort(),
                    itemType = br.ReadByte(),

                    location = br.ReadLong(),
                    wearState = br.ReadLong(),
                    refine = br.ReadByte(),
                    slot = new ItemInfo.Slot() {
                        card1 = br.ReadUShort(),
                        card2 = br.ReadUShort(),
                        card3 = br.ReadUShort(),
                        card4 = br.ReadUShort()
                    },

                    expireTime = br.ReadLong(),
                    bindOnEquip = br.ReadShort(),
                    look = br.ReadShort(),
                    randomOptionCount = br.ReadByte(),
                    options = new List<ItemInfo.Option>()
                };

                for(int j = 0; j < 5; j++) {
                    itemInfo.options.Add(new ItemInfo.Option() {
                        optIndex = br.ReadShort(),
                        value = br.ReadShort(),
                        param1 = br.ReadByte()
                    });
                }

                itemInfo.flag = br.ReadByte();

                Inventory.Add(itemInfo);
            }
        }
    }
}


using ROIO.Utils;
using System.Collections.Generic;

public partial class ZC {

    /**
     * Non-stackable (Equippable) inventory list
     */
    [PacketHandler(HEADER, "ZC_INVENTORY_ITEMLIST_EQUIP")]
    public class INVENTORY_ITEMLIST_EQUIP : InPacket {

        private const int BLOCK_SIZE = 68;
        public const PacketHeader HEADER = PacketHeader.ZC_INVENTORY_ITEMLIST_EQUIP;
        public PacketHeader Header => HEADER;

        public List<ItemInfo> Inventory = new List<ItemInfo>();

        public void Read(MemoryStreamReader br, int size) {
            byte invType = (byte)br.ReadByte();

            var count = (br.Length - br.Position) / BLOCK_SIZE;

            for (int i = 0; i < count; i++) {
                var itemInfo = new ItemInfo {
                    index = br.ReadShort(),

                    ItemID = (int)br.ReadUInt(),

                    itemType = br.ReadByte(),

                    location = (int)br.ReadUInt(),
                    wearState = (int)br.ReadUInt(),

                    slot = new ItemInfo.Slot() {
                        card1 = (int)br.ReadUInt(),
                        card2 = (int)br.ReadUInt(),
                        card3 = (int)br.ReadUInt(),
                        card4 = (int)br.ReadUInt()
                    },

                    expireTime = br.ReadInt(),
                    bindOnEquip = br.ReadUShort(),
                    wItemSpriteNumber = br.ReadUShort(),
                    randomOptionCount = (byte)br.ReadByte(),
                    options = new List<ItemInfo.Option>()
                };

                for (int j = 0; j < 5; j++) {
                    itemInfo.options.Add(new ItemInfo.Option() {
                        optIndex = br.ReadShort(),
                        value = br.ReadShort(),
                        param1 = (byte)br.ReadByte()
                    });
                }

                itemInfo.refine = (byte) br.ReadByte();
                itemInfo.enchantgrade = (byte) br.ReadByte();


                itemInfo.flag = br.ReadByte();

                Inventory.Add(itemInfo);
            }
        }
    }
}


using System.Collections.Generic;
using System.IO;

public partial class ZC {

    /**
     * Non-stackable (Equippable) inventory list
     */
    [PacketHandler(HEADER, "ZC_INVENTORY_ITEMLIST_EQUIPPABLE")]
    public class INVENTORY_ITEMLIST_EQUIPPABLE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_INVENTORY_ITEMLIST_EQUIPPABLE;

        private ItemInfo.Slot slot;

        public PacketHeader GetHeader() => HEADER;

        public bool Read(BinaryReader br) {

            var count = (br.Length - br.Position) / 57;
            var list = new List<ItemInfo>();

            for (int i = 0; i < count; i++) {
                var itemInfo = new ItemInfo();

                itemInfo.index = br.ReadShort();
                itemInfo.viewID = br.ReadShort();
                itemInfo.itemType = br.ReadByte();

                var locationAmount = br.ReadLong();

                var normal = locationAmount < 0;

                if (!normal) { // equip 31B
                    itemInfo.location = locationAmount;
                    itemInfo.wearState = br.ReadLong();
                    itemInfo.refine = br.ReadByte();
                    itemInfo.slot = new ItemInfo.Slot() {
                        card1 = br.ReadUShort(),
                        card2 = br.ReadUShort(),
                        card3 = br.ReadUShort(),
                        card4 = br.ReadUShort()
                    };

                    itemInfo.expireTime = br.ReadLong();
                    itemInfo.bindOnEquip = br.ReadShort();
                    itemInfo.look = br.ReadShort();
                    itemInfo.randomOptionCount = br.ReadByte();
                    itemInfo.options = new List<ItemInfo.Option>();
                    for (int j = 0; j < 5; j++) {
                        itemInfo.options.Add(new ItemInfo.Option() {
                            optIndex = br.ReadShort(),
                            value = br.ReadShort(),
                            param1 = br.ReadByte()
                        });
                    }

                    itemInfo.flag = br.ReadByte();
                } else { // normal 24B
                    itemInfo.ammount = locationAmount;
                    itemInfo.wearState = br.ReadLong();
                    itemInfo.slot = new ItemInfo.Slot() {
                        card1 = br.ReadUShort(),
                        card2 = br.ReadUShort(),
                        card3 = br.ReadUShort(),
                        card4 = br.ReadUShort()
                    };
                    itemInfo.expireTime = br.ReadLong();
                    itemInfo.flag = br.ReadByte();

                    // If this eventually breaks, we need to skip bytes to match sizes
                    //br.Seek(7, SeekOrigin.Current);
                }
                list.Add(itemInfo);
            }
            return true;
        }
    }
}

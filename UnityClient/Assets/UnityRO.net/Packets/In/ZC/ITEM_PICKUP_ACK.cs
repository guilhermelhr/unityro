using ROIO.Utils;
using System.Collections.Generic;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_ITEM_PICKUP_ACK", SIZE)]
    public class ITEM_PICKUP_ACK : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_ITEM_PICKUP_ACK;
        public const int SIZE = 70;
        public PacketHeader Header => HEADER;

        public bool IsIdentified;
        public bool IsDamaged;
        public bool IsFavorite;
        public int result;
        public ItemInfo itemInfo;

        /**
         * 0a37 
         * <index>.W 
         * <amount>.W 
         * <name id>.W 
         * <identified>.B 
         * <damaged>.B 
         * <refine>.B 
         * <card1>.W 
         * <card2>.W 
         * <card3>.W 
         * <card4>.W 
         * <equip location>.L 
         * <item type>.B 
         * <result>.B 
         * <expire time>.L 
         * <bindOnEquipType>.W 
         * { <option id>.W <option value>.W <option param>.B }*5 
         * <favorite>.B 
         * <view id>.W 
         * (ZC_ITEM_PICKUP_ACK_V7)
         */
        public void Read(MemoryStreamReader br, int size) {
            itemInfo = new ItemInfo {
                index = br.ReadShort(),
                amount = br.ReadShort(),
                ItemID = (int)br.ReadUInt()
            };
            IsIdentified = br.ReadByte() == 1;
            itemInfo.IsDamaged = br.ReadByte() == 1;

            itemInfo.slot = new ItemInfo.Slot() {
                card1 = (int)br.ReadUInt(),
                card2 = (int)br.ReadUInt(),
                card3 = (int)br.ReadUInt(),
                card4 = (int)br.ReadUInt()
            };

            itemInfo.location = br.ReadInt();
            itemInfo.itemType = br.ReadByte();
            result = br.ReadByte();
            itemInfo.expireTime = br.ReadInt();
            itemInfo.bindOnEquip = br.ReadUShort();

            itemInfo.options = new List<ItemInfo.Option>();
            for (int j = 0; j < 5; j++) {
                itemInfo.options.Add(new ItemInfo.Option() {
                    optIndex = br.ReadShort(),
                    value = br.ReadShort(),
                    param1 = (byte)br.ReadByte()
                });
            }

            IsFavorite = br.ReadByte() == 1;
            itemInfo.flag = IsIdentified ? 0x1 : 0;
            itemInfo.flag |= IsFavorite ? 0x2 : 0;

            itemInfo.viewID = br.ReadShort();
            itemInfo.refine = (byte) br.ReadByte();
            itemInfo.enchantgrade = (byte) br.ReadByte();
        }
    }
}

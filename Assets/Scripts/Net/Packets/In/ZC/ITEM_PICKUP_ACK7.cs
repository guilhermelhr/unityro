using System;
using System.Collections.Generic;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_ITEM_PICKUP_ACK_V7", SIZE)]
    public class ITEM_PICKUP_ACK7 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_ITEM_PICKUP_ACK_V7;
        public const int SIZE = 59;

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
        public void Read(BinaryReader br, int size) {
            itemInfo = new ItemInfo {
                index = br.ReadShort(),
                amount = br.ReadShort(),
                ItemID = br.ReadShort()
            };
            IsIdentified = br.ReadByte() == 1;
            itemInfo.IsDamaged = br.ReadByte() == 1;
            itemInfo.refine = br.ReadUByte();

            itemInfo.slot = new ItemInfo.Slot() {
                card1 = br.ReadUShort(),
                card2 = br.ReadUShort(),
                card3 = br.ReadUShort(),
                card4 = br.ReadUShort()
            };

            itemInfo.location = br.ReadLong();
            itemInfo.itemType = br.ReadByte();
            result = br.ReadByte();
            itemInfo.expireTime = br.ReadLong();
            itemInfo.bindOnEquip = br.ReadUShort();

            itemInfo.options = new List<ItemInfo.Option>();
            for (int j = 0; j < 5; j++) {
                itemInfo.options.Add(new ItemInfo.Option() {
                    optIndex = br.ReadShort(),
                    value = br.ReadShort(),
                    param1 = br.ReadUByte()
                });
            }

            IsFavorite = br.ReadByte() == 1;
            itemInfo.flag = IsIdentified ? 0x1 : 0;
            itemInfo.flag |= IsFavorite ? 0x2 : 0;
            itemInfo.viewID = br.ReadShort();
        }
    }
}

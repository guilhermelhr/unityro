using System;
using System.Collections.Generic;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_ITEM_PICKUP_ACK_V7", SIZE)]
    public class ITEM_PICKUP_ACK7 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_ITEM_PICKUP_ACK_V7;
        public const int SIZE = 59;

        public short index;
        public short count;
        public short id;
        public bool IsIdentified;
        public bool IsDamaged;
        public int Refine;
        public short card1;
        public short card2;
        public short card3;
        public short card4;
        public int location;
        public int itemType;
        public int result;
        public int expireDate;
        public short bindOnEquipType;
        public List<ItemInfo.Option> options;
        public int IsFavorite;
        public short viewID;

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
        public bool Read(BinaryReader br) {

            index = br.ReadShort();
            count = br.ReadShort();
            id = br.ReadShort();
            IsIdentified = br.ReadByte() == 1;
            IsDamaged = br.ReadByte() == 1;
            Refine = br.ReadByte();

            card1 = br.ReadShort();
            card2 = br.ReadShort();
            card3 = br.ReadShort();
            card4 = br.ReadShort();

            location = br.ReadLong();
            itemType = br.ReadByte();
            result = br.ReadByte();
            expireDate = br.ReadLong();
            bindOnEquipType = br.ReadShort();

            options = new List<ItemInfo.Option>();
            for (int j = 0; j < 5; j++) {
                options.Add(new ItemInfo.Option() {
                    optIndex = br.ReadShort(),
                    value = br.ReadShort(),
                    param1 = br.ReadByte()
                });
            }

            IsFavorite = br.ReadByte();
            viewID = br.ReadShort();

            return true;
        }
    }
}

using System;

public partial class ZC {
    [PacketHandler(HEADER, "ZC_ITEM_ENTRY", SIZE)]
    public class ITEM_ENTRY : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_ITEM_ENTRY;
        public const int SIZE = 17;

        public int id;
        public int mapID;
        public int identified;
        public short x;
        public short y;
        public int subX;
        public int subY;
        public short amount;

        /// 009d <id>.L <name id>.W <identified>.B <x>.W <y>.W <amount>.W <subX>.B <subY>.B
        public bool Read(BinaryReader br) {
            mapID = br.ReadLong();
            id = br.ReadShort();
            identified = br.ReadByte();
            x = br.ReadShort();
            y = br.ReadShort();
            amount = br.ReadShort();
            subX = br.ReadByte();
            subY = br.ReadByte();

            return true;
        }
    }
}

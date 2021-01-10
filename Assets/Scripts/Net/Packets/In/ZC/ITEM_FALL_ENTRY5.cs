using System;

public partial class ZC {

    [PacketHandler(HEADER, "", SIZE)]
    public class ITEM_FALL_ENTRY5 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_ITEM_FALL_ENTRY5;
        public const int SIZE = 22;

        public int id;
        public int mapID;
        public short itemType;
        public int identified;
        public short x;
        public short y;
        public int subX;
        public int subY;
        public short amount;
        public int showDropEffect;
        public short dropEffectMode;

        public void Read(BinaryReader br, int size) {
            mapID = br.ReadLong();
            id = br.ReadShort();
            itemType = br.ReadShort();
            identified = br.ReadByte();
            x = br.ReadShort();
            y = br.ReadShort();
            subX = br.ReadByte();
            subY = br.ReadByte();
            amount = br.ReadShort();
            showDropEffect = br.ReadByte();
            dropEffectMode = br.ReadShort();
        }
    }
}

using System;

public partial class ZC {

    [PacketHandler(HEADER, "", SIZE)]
    public class ITEM_FALL_ENTRY5 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_ITEM_FALL_ENTRY5;
        public const int SIZE = 22;

        public int mapID;
        public int id;
        public ushort itemType;
        public byte identified;
        public short x;
        public short y;
        public byte subX;
        public byte subY;
        public short amount;
        public int showDropEffect;
        public short dropEffectMode;

        public void Read(BinaryReader br, int size) {
            mapID = (int)br.ReadULong();
            id = (int)br.ReadULong();
            itemType = br.ReadUShort();
            identified = br.ReadUByte();
            x = br.ReadShort();
            y = br.ReadShort();
            subX = br.ReadUByte();
            subY = br.ReadUByte();
            amount = br.ReadShort();
            showDropEffect = br.ReadByte();
            dropEffectMode = br.ReadShort();
        }
    }
}

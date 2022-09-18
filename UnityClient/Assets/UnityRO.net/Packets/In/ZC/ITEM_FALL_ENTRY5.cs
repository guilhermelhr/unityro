using ROIO.Utils;
using System;

public partial class ZC {

    [PacketHandler(HEADER, "", SIZE)]
    public class ITEM_FALL_ENTRY5 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_ITEM_FALL_ENTRY5;
        public const int SIZE = 24;
        public PacketHeader Header => HEADER;

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

        public void Read(MemoryStreamReader br, int size) {
            mapID = (int)br.ReadUInt();
            id = (int)br.ReadUInt();
            itemType = br.ReadUShort();
            identified = (byte)br.ReadByte();
            x = br.ReadShort();
            y = br.ReadShort();
            subX = (byte)br.ReadByte();
            subY = (byte)br.ReadByte();
            amount = br.ReadShort();
            showDropEffect = br.ReadByte();
            dropEffectMode = br.ReadShort();
        }
    }
}

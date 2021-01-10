using System;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_VANISH", SIZE)]
    public class NOTIFY_VANISH : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_VANISH;
        public const int SIZE = 7;

        public uint GID;
        public int Type;

        public void Read(BinaryReader br, int size) {
            GID = br.ReadULong();
            Type = br.ReadUByte();
        }
    }
}

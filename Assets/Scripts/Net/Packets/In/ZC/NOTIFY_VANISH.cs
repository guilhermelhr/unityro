using System;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_VANISH", SIZE)]
    public class NOTIFY_VANISH : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_VANISH;
        public const int SIZE = 7;

        public uint GID;
        public int Type;

        public bool Read(BinaryReader br) {

            GID = br.ReadULong();
            Type = br.ReadUByte();

            return true;
        }
    }
}

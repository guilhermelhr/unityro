using System;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_TIME", SIZE)]
    public class NOTIFY_TIME : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_TIME;
        public const int SIZE = 6;

        public ulong Time;

        public void Read(BinaryReader br, int size) {
            Time = br.ReadULong();
        }
    }
}
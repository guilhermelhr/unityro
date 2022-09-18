using ROIO.Utils;
using System;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_TIME", SIZE)]
    public class NOTIFY_TIME : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_TIME;
        public PacketHeader Header => HEADER;
        public const int SIZE = 6;

        public ulong Time;

        public void Read(MemoryStreamReader br, int size) {
            Time = br.ReadUInt();
        }
    }
}
using ROIO.Utils;
using System;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_COUPLESTATUS", SIZE)]
    public class COUPLESTATUS : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_COUPLESTATUS;
        public const int SIZE = 14;
        public PacketHeader Header => HEADER;
        public EntityStatus status;
        public int value;
        public int plusValue;

        public void Read(MemoryStreamReader br, int size) {
            status = (EntityStatus)br.ReadUInt();
            value = br.ReadInt();
            plusValue = br.ReadInt();
        }
    }
}

using System;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_COUPLESTATUS", SIZE)]
    public class COUPLESTATUS : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_COUPLESTATUS;
        public const int SIZE = 14;

        public EntityStatus status;
        public int value;
        public int plusValue;

        public void Read(BinaryReader br, int size) {
            status = (EntityStatus)br.ReadULong();
            value = br.ReadLong();
            plusValue = br.ReadLong();
        }
    }
}

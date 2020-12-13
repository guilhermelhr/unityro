using System;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_COUPLESTATUS", SIZE)]
    public class COUPLESTATUS : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_COUPLESTATUS;
        public const int SIZE = 14;

        public bool Read(BinaryReader br) {
            var statupType = br.ReadULong();
            var defaultStatus = br.ReadLong();
            var plusStatus = br.ReadLong();

            return true;
        }
    }
}

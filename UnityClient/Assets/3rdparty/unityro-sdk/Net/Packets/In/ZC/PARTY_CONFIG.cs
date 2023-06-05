using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_PARTY_CONFIG", SIZE)]
    public class PARTY_CONFIG : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_PARTY_CONFIG;
        public const int SIZE = 3;
        public PacketHeader Header => HEADER;

        public void Read(MemoryStreamReader br, int size) {
        }
    }
}

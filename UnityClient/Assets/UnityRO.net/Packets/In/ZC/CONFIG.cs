using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_CONFIG", SIZE)]
    public class CONFIG : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_CONFIG;
        public const int SIZE = 10;
        public PacketHeader Header => HEADER;
        public void Read(MemoryStreamReader br, int size) {
        }
    }
}

using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_REFUSE_QUIT", SIZE)]
    public class REFUSE_QUIT : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_REFUSE_QUIT;
        public const int SIZE = 2;
        public PacketHeader Header => HEADER;
        public void Read(MemoryStreamReader br, int size) {}
    }
}

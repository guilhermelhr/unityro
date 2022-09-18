using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_CONFIG_NOTIFY", SIZE)]
    public class CONFIG_NOTIFY : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_CONFIG_NOTIFY;
        public const int SIZE = 3;
        public PacketHeader Header => HEADER;
        public void Read(MemoryStreamReader br, int size) {
        }
    }
}

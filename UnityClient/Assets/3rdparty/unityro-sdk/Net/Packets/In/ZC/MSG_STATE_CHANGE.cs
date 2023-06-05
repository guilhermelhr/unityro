using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_MSG_STATE_CHANGE", SIZE)]
    public class MSG_STATE_CHANGE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_MSG_STATE_CHANGE;
        public const int SIZE = 9;
        public PacketHeader Header => HEADER;

        public void Read(MemoryStreamReader br, int size) {
        }
    }
}

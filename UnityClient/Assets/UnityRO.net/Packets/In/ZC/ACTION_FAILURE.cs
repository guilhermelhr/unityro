
using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_ACTION_FAILURE", SIZE)]
    public class ACTION_FAILURE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_ACTION_FAILURE;
        public const int SIZE = 4;
        public PacketHeader Header => HEADER;

        public short ErrorCode;

        public void Read(MemoryStreamReader br, int size) {
            ErrorCode = br.ReadShort();
        }
    }
}

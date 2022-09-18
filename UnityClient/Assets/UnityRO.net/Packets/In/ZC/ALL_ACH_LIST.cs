using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_ALL_ACH_LIST")]
    public class ALL_ACH_LIST : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_ALL_ACH_LIST;
        public PacketHeader Header => HEADER;
        public void Read(MemoryStreamReader br, int size) {
        }
    }
}

using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_ACH_UPDATE", SIZE)]
    public class ACH_UPDATE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_ACH_UPDATE;
        public PacketHeader Header => HEADER;

        public const int SIZE = 66;

        public void Read(MemoryStreamReader br, int size) {
        }
    }
}

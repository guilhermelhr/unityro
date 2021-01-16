public partial class ZC {

    [PacketHandler(HEADER, "ZC_ACH_UPDATE", SIZE)]
    public class ACH_UPDATE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_ACH_UPDATE;
        public const int SIZE = 66;

        public void Read(BinaryReader br, int size) {
        }
    }
}

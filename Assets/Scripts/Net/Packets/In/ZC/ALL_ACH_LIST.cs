public partial class ZC {

    [PacketHandler(HEADER, "ZC_ALL_ACH_LIST")]
    public class ALL_ACH_LIST : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_ALL_ACH_LIST;

        public void Read(BinaryReader br, int size) {
        }
    }
}

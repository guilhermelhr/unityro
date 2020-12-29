public partial class ZC {

    [PacketHandler(HEADER, "ZC_AID", SIZE)]
    public class AID : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_AID;
        public const int SIZE = 6;

        public bool Read(BinaryReader br) {

            return true;
        }
    }
}

public partial class ZC {

    [PacketHandler(HEADER, "ZC_MSG_STATE_CHANGE3", SIZE)]
    public class MSG_STATE_CHANGE3 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_MSG_STATE_CHANGE3;
        public const int SIZE = 24;

        public bool Read(BinaryReader br) {
            return true;
        }
    }
}

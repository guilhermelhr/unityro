public partial class ZC {

    [PacketHandler(HEADER, "ZC_CLOSE_DIALOG", SIZE)]
    public class CLOSE_DIALOG : InPacket {
        public const PacketHeader HEADER = PacketHeader.ZC_CLOSE_DIALOG;
        public const int SIZE = 6;

        public bool Read(BinaryReader br) {
            var NAID = br.ReadULong();

            return true;
        }
    }
}
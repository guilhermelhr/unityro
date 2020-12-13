public partial class ZC {

    [PacketHandler(HEADER, "ZC_OPEN_EDITDLG", SIZE)]
    public class OPEN_EDITDLG : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_OPEN_EDITDLG;
        public const int SIZE = 6;

        public bool Read(BinaryReader br) {
            var NAID = br.ReadULong();
            return true;
        }
    }
}

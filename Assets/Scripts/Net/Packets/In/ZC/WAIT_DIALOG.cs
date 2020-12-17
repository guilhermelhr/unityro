public partial class ZC {

    [PacketHandler(HEADER, "ZC_WAIT_DIALOG", SIZE)]
    public class WAIT_DIALOG : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_WAIT_DIALOG;
        public const int SIZE = 6;

        public uint NAID;

        public bool Read(BinaryReader br) {
            NAID = br.ReadULong();

            return true;
        }
    }
}
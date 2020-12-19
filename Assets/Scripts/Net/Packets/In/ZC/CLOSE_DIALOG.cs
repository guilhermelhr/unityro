public partial class ZC {

    /**
     * Server sent a Close Button
     */
    [PacketHandler(HEADER, "ZC_CLOSE_DIALOG", SIZE)]
    public class CLOSE_DIALOG : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_CLOSE_DIALOG;
        public const int SIZE = 6;

        public uint NAID;

        public bool Read(BinaryReader br) {
            NAID = br.ReadULong();

            return true;
        }
    }
}
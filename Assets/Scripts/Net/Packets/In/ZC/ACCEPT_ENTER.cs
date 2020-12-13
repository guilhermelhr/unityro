public partial class ZC {

    [PacketHandler(HEADER, "ZC_ACCEPT_ENTER", SIZE)]
    public class ACCEPT_ENTER : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_ACCEPT_ENTER;
        public const int SIZE = 11;

        public bool Read(BinaryReader br) {

            var startTime = br.ReadULong();
            var posDir = br.ReadPos();
            var xSize = br.ReadUByte();
            var ySize = br.ReadUByte();

            return true;
        }
    }
}

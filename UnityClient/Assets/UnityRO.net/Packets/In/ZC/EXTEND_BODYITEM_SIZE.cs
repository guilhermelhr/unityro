using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_EXTEND_BODYITEM_SIZE", SIZE)]
    public class EXTEND_BODYITEM_SIZE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_EXTEND_BODYITEM_SIZE;
        public const int SIZE = 4;
        public PacketHeader Header => HEADER;

        public short expansionSize;

        public void Read(MemoryStreamReader br, int size) {
            expansionSize = br.ReadShort();
        }
    }
}

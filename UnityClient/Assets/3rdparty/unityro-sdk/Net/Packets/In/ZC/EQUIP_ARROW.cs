
using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_EQUIP_ARROW", SIZE)]
    public class EQUIP_ARROW : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_EQUIP_ARROW;
        public const int SIZE = 4;
        public PacketHeader Header => HEADER;
        public short Index;

        public void Read(MemoryStreamReader br, int size) {
            Index = br.ReadShort();
        }
    }
}

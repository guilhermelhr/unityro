using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_INVENTORY_START", SIZE)]
    public class INVENTORY_END : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_INVENTORY_END;
        public const int SIZE = 4;
        public PacketHeader Header => HEADER;

        public void Read(MemoryStreamReader br, int size) {
            var invType = br.ReadByte();
            var flag = br.ReadByte();
        }
    }
}
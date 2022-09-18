using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_INVENTORY_START")]
    public class INVENTORY_START : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_INVENTORY_START;
        public PacketHeader Header => HEADER;

        public void Read(MemoryStreamReader br, int size) {
            var inventoryType = br.ReadShort();
            var name = br.ReadBinaryString(br.Length - br.Position);
        }
    }
}

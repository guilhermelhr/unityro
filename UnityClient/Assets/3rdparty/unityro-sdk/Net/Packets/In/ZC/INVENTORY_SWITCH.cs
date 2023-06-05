using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_INVENTORY_SWITCH")]
    public class INVENTORY_SWITCH : InPacket {
        public const PacketHeader HEADER = PacketHeader.ZC_INVENTORY_SWITCH;
        public PacketHeader Header => HEADER;

        public void Read(MemoryStreamReader br, int size) {
        }
    }
}

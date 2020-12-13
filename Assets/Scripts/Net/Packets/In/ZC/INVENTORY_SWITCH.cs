public partial class ZC {

    [PacketHandler(HEADER, "ZC_INVENTORY_SWITCH")]
    public class INVENTORY_SWITCH : InPacket {
        public const PacketHeader HEADER = PacketHeader.ZC_INVENTORY_SWITCH;

        public bool Read(BinaryReader br) {
            return true;
        }
    }
}

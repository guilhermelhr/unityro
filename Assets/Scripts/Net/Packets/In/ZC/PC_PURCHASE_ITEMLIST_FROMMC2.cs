public partial class ZC {

    [PacketHandler(HEADER, "ZC_PC_PURCHASE_ITEMLIST_FROMMC2")]
    public class PC_PURCHASE_ITEMLIST_FROMMC2 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_PC_PURCHASE_ITEMLIST_FROMMC2;

        public void Read(BinaryReader br, int size) {
            
        }
    }
}

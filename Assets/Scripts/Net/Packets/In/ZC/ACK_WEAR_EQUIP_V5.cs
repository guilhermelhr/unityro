public partial class ZC {

    [PacketHandler(HEADER, "ZC_ACK_WEAR_EQUIP_V5", SIZE)]
    public class ACK_WEAR_EQUIP_V5 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_ACK_WEAR_EQUIP_V5;
        public const int SIZE = 11;
        
        public short index;
        public int equipLocation;
        public short ViewID;
        public int result;

        public bool Read(BinaryReader br) {
            index = br.ReadShort();
            equipLocation = br.ReadLong();
            ViewID = br.ReadShort();
            result = br.ReadByte();

            return true;
        }
    }
}

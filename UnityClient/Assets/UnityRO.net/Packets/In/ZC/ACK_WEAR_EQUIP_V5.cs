using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_ACK_WEAR_EQUIP_V5", SIZE)]
    public class ACK_WEAR_EQUIP_V5 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_ACK_WEAR_EQUIP_V5;
        public const int SIZE = 11;
        public PacketHeader Header => HEADER;

        public short index;
        public int equipLocation;
        public short ViewID;
        public int result;

        public void Read(MemoryStreamReader br, int size) {
            index = br.ReadShort();
            equipLocation = br.ReadInt();
            ViewID = br.ReadShort();
            result = br.ReadByte();
        }
    }
}

using ROIO.Utils;
using System;
public partial class ZC {

    [PacketHandler(HEADER, "ZC_ACK_TAKEOFF_EQUIP_V5", SIZE)]
    public class ACK_TAKEOFF_EQUIP_V5 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_ACK_TAKEOFF_EQUIP_V5;
        public PacketHeader Header => HEADER;
        public const int SIZE = 9;

        public int index;
        public int equipLocation;
        public int result;

        public void Read(MemoryStreamReader br, int size) {
            index = br.ReadShort();
            equipLocation = br.ReadInt();
            result = br.ReadByte();
        }
    }
}

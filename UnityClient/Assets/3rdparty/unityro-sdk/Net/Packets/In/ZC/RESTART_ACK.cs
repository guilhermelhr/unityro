using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_RESTART_ACK", SIZE)]
    public class RESTART_ACK : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_RESTART_ACK;
        public const int SIZE = 3;
        public PacketHeader Header => HEADER;

        public byte type;

        public void Read(MemoryStreamReader br, int size) {
            type = (byte) br.ReadByte();
        }
    }
}

using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "SELECT_DEALTYPE", SIZE)]
    public class SELECT_DEALTYPE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_SELECT_DEALTYPE;
        public const int SIZE = 6;
        public PacketHeader Header => HEADER;

        public uint NAID;

        public void Read(MemoryStreamReader br, int size) {
            NAID = br.ReadUInt();
        }
    }
}

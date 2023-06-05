using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_WAIT_DIALOG", SIZE)]
    public class WAIT_DIALOG : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_WAIT_DIALOG;
        public const int SIZE = 6;
        public PacketHeader Header => HEADER;

        public uint NAID;

        public void Read(MemoryStreamReader br, int size) {
            NAID = br.ReadUInt();
        }
    }
}
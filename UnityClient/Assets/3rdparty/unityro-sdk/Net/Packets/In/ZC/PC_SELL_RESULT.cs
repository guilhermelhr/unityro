using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_PC_SELL_RESULT", SIZE)]
    public class PC_SELL_RESULT : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_PC_SELL_RESULT;
        public const int SIZE = 3;
        public PacketHeader Header => HEADER;

        public int Result { get; private set; }

        public void Read(MemoryStreamReader br, int size) {
            Result = br.ReadByte();
        }
    }
}

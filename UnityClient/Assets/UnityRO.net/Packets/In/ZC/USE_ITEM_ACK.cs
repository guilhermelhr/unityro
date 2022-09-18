using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_USE_ITEM_ACK", SIZE)]
    public class USE_ITEM_ACK : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_USE_ITEM_ACK;
        public const int SIZE = 7;
        public PacketHeader Header => HEADER;

        public short index;
        public short count;
        public byte result;

        public void Read(MemoryStreamReader fp, int size) {
            this.index = fp.ReadShort();
            this.count = fp.ReadShort();
            this.result = (byte)fp.ReadByte();
        }
    }
}

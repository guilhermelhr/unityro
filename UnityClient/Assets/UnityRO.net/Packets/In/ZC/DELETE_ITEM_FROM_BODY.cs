using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_DELETE_ITEM_FROM_BODY", SIZE)]
    public class DELETE_ITEM_FROM_BODY : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_DELETE_ITEM_FROM_BODY;
        public const int SIZE = 8;
        public PacketHeader Header => HEADER;
        public short DeleteType;
        public ushort Index;
        public ushort Count;

        public void Read(MemoryStreamReader br, int size) {
            this.DeleteType = br.ReadShort();
            this.Index = br.ReadUShort();
            this.Count = br.ReadUShort();
        }
    }
}

public partial class ZC {

    [PacketHandler(HEADER, "ZC_USE_ITEM_ACK", SIZE)]
    public class USE_ITEM_ACK : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_USE_ITEM_ACK;
        public const int SIZE = 7;

        public short index;
        public short count;
        public byte result;

        /// 01c8 <index>.W <name id>.W <id>.L <amount>.W <result>.B (ZC_USE_ITEM_ACK2)
        public bool Read(BinaryReader fp) {
            this.index = fp.ReadShort();
            this.count = fp.ReadShort();
            this.result = fp.ReadUByte();

            return true;
        }
    }
}

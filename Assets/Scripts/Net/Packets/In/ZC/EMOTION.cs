public partial class ZC {

    [PacketHandler(HEADER, "ZC_EMOTION", SIZE)]
    public class EMOTION : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_EMOTION;
        public const int SIZE = 7;

        public uint GID;
        public byte type;

        public void Read(BinaryReader fp, int size) {
            this.GID = fp.ReadULong();
            this.type = fp.ReadUByte();
        }
    }

}
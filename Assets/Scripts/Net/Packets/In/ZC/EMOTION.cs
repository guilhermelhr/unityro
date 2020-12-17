public partial class ZC {

    [PacketHandler(HEADER, "ZC_EMOTION", SIZE)]
    public class EMOTION : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_EMOTION;
        public const int SIZE = 7;

        public uint GID;
        public byte type;

        public bool Read(BinaryReader fp) {

            this.GID = fp.ReadULong();
            this.type = fp.ReadUByte();

            return true;
        }
    }

}
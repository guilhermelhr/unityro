public partial class ZC {

    [PacketHandler(HEADER, "SPRITE_CHANGE2", SIZE)]
    public class SPRITE_CHANGE2 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_SPRITE_CHANGE2;
        public const int SIZE = 11;

        public PacketHeader GetHeader() => HEADER;

        public bool Read(BinaryReader br) {

            var GID = br.ReadULong();
            var type = br.ReadUByte();
            var value = br.ReadULong();

            return true;
        }
    }
}

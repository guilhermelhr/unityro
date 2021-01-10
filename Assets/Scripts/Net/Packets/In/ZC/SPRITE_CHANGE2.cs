public partial class ZC {

    [PacketHandler(HEADER, "SPRITE_CHANGE2", SIZE)]
    public class SPRITE_CHANGE2 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_SPRITE_CHANGE2;
        public const int SIZE = 15;

        public void Read(BinaryReader br, int size) {
            var GID = br.ReadLong();
            var type = br.ReadByte();
            var value = br.ReadShort();
            var value2 = br.ReadShort();
        }
    }
}

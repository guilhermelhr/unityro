public partial class HC {

    [PacketHandler(HEADER, "HC_ACCEPT_MAKECHAR", SIZE)]
    public class ACCEPT_MAKECHAR : InPacket {

        public const PacketHeader HEADER = PacketHeader.HC_ACCEPT_MAKECHAR;
        public const int SIZE = 157;

        public CharacterData characterData;

        public void Read(BinaryReader br, int size) {
            characterData = CharacterData.parse(br);
        }
    }
}

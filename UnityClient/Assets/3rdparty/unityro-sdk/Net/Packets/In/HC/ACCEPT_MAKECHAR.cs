using ROIO.Utils;

public partial class HC {

    [PacketHandler(HEADER, "HC_ACCEPT_MAKECHAR", SIZE)]
    public class ACCEPT_MAKECHAR : InPacket {

        public const PacketHeader HEADER = PacketHeader.HC_ACCEPT_MAKECHAR;
        public const int SIZE = CharacterData.BLOCK_SIZE + 2;

        public CharacterData characterData;
        public PacketHeader Header => HEADER;
        public void Read(MemoryStreamReader br, int size) {
            characterData = CharacterData.parse(br);
        }
    }
}

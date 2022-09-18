using ROIO.Utils;

public partial class HC {

    [PacketHandler(HEADER, "HC_BLOCK_CHARACTER")]
    public class BLOCK_CHARACTER : InPacket {

        public const PacketHeader HEADER = PacketHeader.HC_BLOCK_CHARACTER;

        public PacketHeader Header => HEADER;

        public void Read(MemoryStreamReader br, int size) {
            var count = (br.Length - br.Position) / 24;

            for(var i = 0; i < count; i++) {
                var GID = br.ReadUInt();
                var szExpireDate = br.ReadBinaryString(20);
            }
        }
    }
}
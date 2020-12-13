public partial class HC {

    [PacketHandler(HEADER, "HC_BLOCK_CHARACTER")]
    public class BLOCK_CHARACTER : InPacket {

        public const PacketHeader HEADER = PacketHeader.HC_BLOCK_CHARACTER;

        public PacketHeader GetHeader() => HEADER;

        public bool Read(BinaryReader br) {
            var count = (br.Length - br.Position) / 24;

            for(var i = 0; i < count; i++) {
                var GID = br.ReadULong();
                var szExpireDate = br.ReadBinaryString(20);
            }

            return true;
        }
    }

}
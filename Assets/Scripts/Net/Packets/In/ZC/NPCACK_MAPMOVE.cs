public partial class ZC {

    [PacketHandler(HEADER, "ZC_NPCACK_MAPMOVE", SIZE)]
    public class NPCACK_MAPMOVE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NPCACK_MAPMOVE;
        public const int SIZE = 22;

        public bool Read(BinaryReader br) {
            var mapName = br.ReadBinaryString(16);
            var xPos = br.ReadShort();
            var yPos = br.ReadShort();

            return true;
        }
    }
}

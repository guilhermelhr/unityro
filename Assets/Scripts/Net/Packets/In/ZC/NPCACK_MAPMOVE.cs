public partial class ZC {

    [PacketHandler(HEADER, "ZC_NPCACK_MAPMOVE", SIZE)]
    public class NPCACK_MAPMOVE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NPCACK_MAPMOVE;
        public const int SIZE = 22;

        public string MapName;
        public short PosX;
        public short PosY;

        public void Read(BinaryReader br, int size) {
            MapName = br.ReadBinaryString(16);
            PosX = br.ReadShort();
            PosY = br.ReadShort();
        }
    }
}

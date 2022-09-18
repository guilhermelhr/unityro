using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NPCACK_MAPMOVE", SIZE)]
    public class NPCACK_MAPMOVE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NPCACK_MAPMOVE;
        public const int SIZE = 22;
        public PacketHeader Header => HEADER;

        public string MapName;
        public short PosX;
        public short PosY;

        public void Read(MemoryStreamReader br, int size) {
            MapName = br.ReadBinaryString(16);
            PosX = br.ReadShort();
            PosY = br.ReadShort();
        }
    }
}

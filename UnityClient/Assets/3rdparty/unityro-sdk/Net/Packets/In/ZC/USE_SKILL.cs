using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_USE_SKILL", SIZE)]
    public class USE_SKILL : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_USE_SKILL;
        public const int SIZE = 17;

        public PacketHeader Header => HEADER;

        public short skillID;
        public int level;
        public int targetAID;
        public int srcAID;
        public byte result;

        public void Read(MemoryStreamReader br, int size) {
            skillID = br.ReadShort();
            level = br.ReadInt();
            targetAID = br.ReadInt();
            srcAID = br.ReadInt();
            result = (byte) br.ReadByte();
        }
    }
}

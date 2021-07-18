using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "SPRITE_CHANGE2", SIZE)]
    public class SPRITE_CHANGE2 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_SPRITE_CHANGE2;
        public const int SIZE = 15;

        public uint GID;
        public int type;
        public short value;
        public short value2;

        public void Read(MemoryStreamReader br, int size) {
            GID = br.ReadUInt();
            type = br.ReadByte();
            value = br.ReadShort();
            value2 = br.ReadShort();
        }
    }
}

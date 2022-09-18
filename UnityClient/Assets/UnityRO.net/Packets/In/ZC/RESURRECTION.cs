using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_RESSURECTION", SIZE)]
    public class RESURRECTION : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_RESURRECTION;
        public const int SIZE = 8;
        public PacketHeader Header => HEADER;

        public uint GID;
        public short Type;

        public void Read(MemoryStreamReader br, int size) {
            GID = br.ReadUInt();
            Type = br.ReadShort();
        }
    }
}

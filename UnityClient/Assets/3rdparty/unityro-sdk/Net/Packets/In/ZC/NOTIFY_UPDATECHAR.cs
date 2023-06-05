using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_UPDATECHAR", SIZE)]
    public class NOTIFY_UPDATECHAR : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_UPDATECHAR;
        public PacketHeader Header => HEADER;
        public const int SIZE = 9;

        public uint GID;
        public short Style;
        public int Item;

        public void Read(MemoryStreamReader br, int size) {
            GID = br.ReadUInt();
            Style = br.ReadShort();
            Item = br.ReadByte();
        }
    }
}

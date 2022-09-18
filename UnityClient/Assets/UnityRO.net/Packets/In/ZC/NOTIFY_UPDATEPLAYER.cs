using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_UPDATEPLAYER", SIZE)]
    public class NOTIFY_UPDATEPLAYER : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_UPDATEPLAYER;
        public const int SIZE = 5;
        public PacketHeader Header => HEADER;

        public short Style;
        public int Item;

        public void Read(MemoryStreamReader br, int size) {
            Style = br.ReadShort();
            Item = br.ReadByte();
        }
    }
}

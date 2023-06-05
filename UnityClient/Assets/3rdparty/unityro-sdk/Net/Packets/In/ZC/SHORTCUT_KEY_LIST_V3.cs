using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_SHORTCUT_KEY_LIST_V4", SIZE)]
    public class SHORTCUT_KEY_LIST_V3 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_SHORTCUT_KEY_LIST_V4;
        public const int SIZE = 271;
        public PacketHeader Header => HEADER;

        public void Read(MemoryStreamReader br, int size) {
        }
    }
}

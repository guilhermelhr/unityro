using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_QUEST_NOTIFY_EFFECT", SIZE)]
    public class QUEST_NOTIFY_EFFECT : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_QUEST_NOTIFY_EFFECT;
        public const int SIZE = 14;
        public PacketHeader Header => HEADER;

        public void Read(MemoryStreamReader br, int size) {
        }
    }
}
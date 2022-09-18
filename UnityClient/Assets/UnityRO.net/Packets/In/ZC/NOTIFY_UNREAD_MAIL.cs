using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_UNREAD_MAIL", SIZE)]
    public class NOTIFY_UNREAD_MAIL : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_UNREAD_MAIL;
        public PacketHeader Header => HEADER;
        public const int SIZE = 3;
        public void Read(MemoryStreamReader br, int size) {
            var count = br.ReadByte();
        }
    }
}

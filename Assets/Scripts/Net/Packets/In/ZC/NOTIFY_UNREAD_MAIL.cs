public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_UNREAD_MAIL", SIZE)]
    public class NOTIFY_UNREAD_MAIL : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_UNREAD_MAIL;
        public const int SIZE = 3;
        public bool Read(BinaryReader br) {
            var count = br.ReadByte();

            return true;
        }
    }
}

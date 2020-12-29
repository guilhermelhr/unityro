public partial class HC {

    [PacketHandler(HEADER, "HC_NOTIFY_CHARLIST", SIZE)]
    public class NOTIFY_CHARLIST : InPacket {

        public const PacketHeader HEADER = PacketHeader.HC_NOTIFY_CHARLIST;
        public const int SIZE = 6;

        public bool Read(BinaryReader br) {
            var totalCnt = br.ReadLong();
            return true;
        }
    }
}

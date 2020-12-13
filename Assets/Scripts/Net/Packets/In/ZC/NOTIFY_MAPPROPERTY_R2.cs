public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_MAPPROPERTY_R2", SIZE)]
    public class NOTIFY_MAPPROPERTY_R2 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_MAPPROPERTY_R2;
        public const int SIZE = 8;

        public bool Read(BinaryReader br) {

            var type = br.ReadShort();
            var flag = br.ReadLong();

            return true;
        }
    }
}

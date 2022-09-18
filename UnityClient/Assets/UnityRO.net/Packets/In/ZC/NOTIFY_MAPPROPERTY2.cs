using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_MAPPROPERTY2", SIZE)]
    public class NOTIFY_MAPPROPERTY2 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_MAPPROPERTY2;
        public PacketHeader Header => HEADER;
        public const int SIZE = 8;

        public void Read(MemoryStreamReader br, int size) {
            var type = br.ReadShort();
            var flag = br.ReadInt();
        }
    }
}

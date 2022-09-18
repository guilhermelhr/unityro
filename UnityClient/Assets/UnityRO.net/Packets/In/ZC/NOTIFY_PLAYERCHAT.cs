
using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_PLAYERCHAT")]
    public class NOTIFY_PLAYERCHAT : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_PLAYERCHAT;
        public PacketHeader Header => HEADER;
        public string Message;

        public PacketHeader GetHeader() => HEADER;

        public void Read(MemoryStreamReader br, int size) {
            Message = br.ReadBinaryString(size);
        }
    }
}

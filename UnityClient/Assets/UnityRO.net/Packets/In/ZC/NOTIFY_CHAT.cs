using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_CHAT")]
    public class NOTIFY_CHAT : InPacket {
        
        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_CHAT;
        public PacketHeader Header => HEADER;

        public uint GID;
        public string Message;

        public void Read(MemoryStreamReader br, int size) {
            GID = br.ReadUInt();
            Message = br.ReadBinaryString((int)(br.Length - br.Position));
        }
    }
}

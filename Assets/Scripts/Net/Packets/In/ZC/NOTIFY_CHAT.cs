public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_CHAT")]
    public class NOTIFY_CHAT : InPacket {
        
        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_CHAT;

        public uint GID;
        public string Message;

        public bool Read(BinaryReader br) {

            GID = br.ReadULong();
            Message = br.ReadBinaryString((int)(br.Length - br.Position));

            return true;
        }
    }
}

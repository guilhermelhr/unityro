using ROIO.Utils;

public partial class AC 
{
    [PacketHandler(HEADER, "AC_REFUSE_LOGIN")]
    public class REFUSE_LOGIN : InPacket
    {
        public byte ErrorCode { get; set; }
        public string BlockDate { get; set; }

        public const PacketHeader HEADER = PacketHeader.AC_REFUSE_LOGIN;
        public PacketHeader Header => HEADER;
        public void Read(MemoryStreamReader br, int size) 
        {
            ErrorCode = (byte) br.ReadByte();
            BlockDate = br.ReadBinaryString(20);
        }
    }

}

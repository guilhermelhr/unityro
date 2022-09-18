using ROIO.Utils;
using System;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_MENU_LIST")]
    public class MENU_LIST : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_MENU_LIST;
        public PacketHeader Header => HEADER;

        public uint NAID;
        public string Message;

        public void Read(MemoryStreamReader br, int size) {
            NAID = br.ReadUInt();
            Message = br.ReadBinaryString(br.Length - br.Position);
        }
    }
}

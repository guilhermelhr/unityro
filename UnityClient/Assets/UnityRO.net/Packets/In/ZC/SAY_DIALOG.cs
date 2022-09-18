using ROIO.Utils;
using System;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_SAY_DIALOG")]
    public class SAY_DIALOG : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_SAY_DIALOG;
        public PacketHeader Header => HEADER;

        public uint NAID;
        public string Message;

        public void Read(MemoryStreamReader br, int size) {
            NAID = br.ReadUInt();
            Message = br.ReadBinaryString((int)(br.Length - br.Position));
        }
    }
}

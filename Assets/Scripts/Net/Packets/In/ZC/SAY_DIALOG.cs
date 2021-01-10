using System;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_SAY_DIALOG")]
    public class SAY_DIALOG : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_SAY_DIALOG;

        public uint NAID;
        public string Message;

        public void Read(BinaryReader br, int size) {
            NAID = br.ReadULong();
            Message = br.ReadBinaryString((int)(br.Length - br.Position));
        }
    }
}

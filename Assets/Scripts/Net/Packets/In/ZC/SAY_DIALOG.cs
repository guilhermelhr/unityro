using System;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_SAY_DIALOG")]
    public class SAY_DIALOG : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_SAY_DIALOG;

        public uint NAID;
        public string Message;

        public bool Read(BinaryReader br) {

            NAID = br.ReadULong();
            Message = br.ReadBinaryString((int)(br.Length - br.Position));

            return true;
        }
    }
}

using System;
using System.IO;

public partial class CZ {
    public class CLOSE_DIALOG : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_CLOSE_DIALOG;
        public const int SIZE = 6;

        public uint NAID;

        public CLOSE_DIALOG() : base(HEADER, SIZE) { }

        public override bool Send(BinaryWriter writer) {
            base.Send(writer);

            writer.Write(NAID);
            writer.Flush();

            return true;
        }
    }
}

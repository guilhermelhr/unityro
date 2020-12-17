using System;
using System.IO;

public partial class CZ {

    public class CONTACTNPC : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_CONTACTNPC;
        public const int SIZE = 7;

        public uint NAID;
        public byte Type;

        public CONTACTNPC() : base(HEADER, SIZE) { }

        public override bool Send(BinaryWriter writer) {
            base.Send(writer);

            writer.Write(NAID);
            writer.Write(Type);
            writer.Flush();

            return true;
        }
    }
}

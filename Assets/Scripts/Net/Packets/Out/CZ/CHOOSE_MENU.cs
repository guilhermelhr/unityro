using System;
using System.IO;

public partial class CZ {

    public class CHOOSE_MENU : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_CHOOSE_MENU;
        public const int SIZE = 7;

        public CHOOSE_MENU() : base(HEADER, SIZE) { }

        public uint NAID;
        public byte Index;

        public override bool Send(BinaryWriter writer) {
            base.Send(writer);

            writer.Write(NAID);
            writer.Write(Index);
            writer.Flush();

            return true;
        }
    }
}

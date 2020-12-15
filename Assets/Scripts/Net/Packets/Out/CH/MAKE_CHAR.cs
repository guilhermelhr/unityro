using System.IO;

public partial class CH {

    public class MAKE_CHAR : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CH_MAKE_CHAR;
        public const int SIZE = 2 + 24 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 2 + 2;

        public string Name;
        public byte Str = 0;
        public byte Agi = 0;
        public byte Vit = 0;
        public byte Int = 0;
        public byte Dex = 0;
        public byte Luk = 0;
        public byte CharNum = 0;
        public ushort HeadPal = 0;
        public ushort Head = 0;

        public MAKE_CHAR() : base(HEADER, SIZE) { }

        public override bool Send(BinaryWriter writer) {
            base.Send(writer);

            writer.WriteCString(Name, 24);
            writer.Write(Str);
            writer.Write(Agi);
            writer.Write(Vit);
            writer.Write(Int);
            writer.Write(Dex);
            writer.Write(Luk);
            writer.Write(CharNum);
            writer.Write(HeadPal);
            writer.Write(Head);

            return true;
        }
    }
}
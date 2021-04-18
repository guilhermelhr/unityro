public partial class CH {

    public class MAKE_CHAR2 : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CH_MAKE_CHAR2;
        public const int SIZE = 36;

        public string Name;
        public byte CharNum = 0;
        public ushort HeadPal = 0;
        public ushort Head = 1;
        public int StartJob = 0;
        public byte Sex = 0;

        public MAKE_CHAR2() : base(HEADER, SIZE) { }

        public override void Send() {
            Write(Name, 24);
            Write(CharNum);
            Write(HeadPal);
            Write(Head);
            Write(StartJob);
            Write(Sex);

            base.Send();
        }
    }
}
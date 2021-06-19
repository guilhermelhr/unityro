public partial class CZ {

    public class CONTACTNPC : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_CONTACTNPC;
        public const int SIZE = 7;

        public uint NAID;
        public byte Type;

        public CONTACTNPC() : base(HEADER, SIZE) { }

        public override void Send() {
            Write(NAID);
            Write(Type);

            base.Send();
        }
    }
}

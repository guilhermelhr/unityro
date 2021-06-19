public partial class CZ {

    public class REQ_NEXT_SCRIPT : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_REQ_NEXT_SCRIPT;
        public const int SIZE = 6;

        public uint NAID;

        public REQ_NEXT_SCRIPT() : base(HEADER, SIZE) { }

        public override void Send() {
            Write(NAID);

            base.Send();
        }
    }
}

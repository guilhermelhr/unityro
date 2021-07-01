public partial class CZ {

    public class REQUEST_TIME : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_REQUEST_TIME;
        public const int SIZE = 2;

        public REQUEST_TIME() : base(HEADER, SIZE) { }

        public override void Send() {
            base.Send();
        }
    }
}

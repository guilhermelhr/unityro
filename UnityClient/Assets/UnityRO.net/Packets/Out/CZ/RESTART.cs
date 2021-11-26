public partial class CZ {

    public class RESTART : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_RESTART;
        public const int SIZE = 3;

        private byte type;

        public RESTART(byte type) : base(HEADER, SIZE) {
            this.type = type;
        }

        public override void Send() {
            Write(type);

            base.Send();
        }
    }
}

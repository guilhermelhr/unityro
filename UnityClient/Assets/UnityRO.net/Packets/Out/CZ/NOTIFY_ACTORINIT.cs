public partial class CZ {

    public class NOTIFY_ACTORINIT : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_NOTIFY_ACTORINIT;
        public const int SIZE = 2;

        public NOTIFY_ACTORINIT() : base(HEADER, SIZE) { }

        public override void Send() {
            base.Send();
        }
    }
}

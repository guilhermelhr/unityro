public partial class CZ {

    public class CLOSE_DIALOG : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_CLOSE_DIALOG;
        public const int SIZE = 6;

        public uint NAID;

        public CLOSE_DIALOG() : base(HEADER, SIZE) { }

        public override void Send() {
            Write(NAID);

            base.Send();
        }
    }
}

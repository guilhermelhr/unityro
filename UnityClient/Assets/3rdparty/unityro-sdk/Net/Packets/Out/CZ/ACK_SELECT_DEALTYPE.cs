public partial class CZ {

    public class ACK_SELECT_DEALTYPE : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_ACK_SELECT_DEALTYPE;
        public const int SIZE = 7;

        public uint NAID;
        public byte Type;

        public ACK_SELECT_DEALTYPE() : base(HEADER, SIZE) { }

        public override void Send() {
            Write(NAID);
            Write(Type);

            base.Send();
        }
    }
}

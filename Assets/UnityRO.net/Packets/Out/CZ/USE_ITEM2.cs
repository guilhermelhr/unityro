public partial class CZ {

    public class USE_ITEM2 : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_USE_ITEM2;
        public const int SIZE = 8;

        public int AID;
        public short index;

        public USE_ITEM2() : base(HEADER, SIZE) { }

        public override void Send() {
            Write(index);
            Write(AID);

            base.Send();
        }
    }
}

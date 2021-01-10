public partial class CZ {

    public class ITEM_PICKUP2 : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_ITEM_PICKUP2;
        public const int SIZE = 6;

        public int ID;

        public ITEM_PICKUP2() : base(HEADER, SIZE) { }

        public override void Send() {
            Write(ID);

            base.Send();
        }
    }
}

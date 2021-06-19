public partial class CZ {

    public class REQ_WEAR_EQUIP_V5 : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_REQ_WEAR_EQUIP_V5;
        public const int SIZE = 8;

        public short index;
        public int location;

        public REQ_WEAR_EQUIP_V5() : base(HEADER, SIZE) { }

        public override void Send() {
            Write(index);
            Write(location);

            base.Send();
        }
    }
}

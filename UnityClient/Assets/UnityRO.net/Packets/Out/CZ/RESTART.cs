public partial class CZ {

    public class RESTART : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_RESTART;
        public const int SIZE = 3;

        public const byte TYPE_SAVE_POINT = 0;
        public const byte TYPE_CHAR_SELECT = 1;
        
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

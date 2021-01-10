public partial class CZ {

    public const PacketHeader HEADER = PacketHeader.CZ_REQUEST_MOVE2;
    public const int SIZE = 5;

    public class REQUEST_MOVE2 : OutPacket {

        public short x;
        public short y;
        public byte dir;

        public REQUEST_MOVE2() : base(HEADER, SIZE) { }

        public REQUEST_MOVE2(int x, int y, int dir) : base(HEADER, SIZE) {
            this.x = (short)x;
            this.y = (short)y;
            this.dir = (byte)dir;
        }

        public override void Send() {
            WritePos(x, y, dir);

            base.Send();
        }
    }
}

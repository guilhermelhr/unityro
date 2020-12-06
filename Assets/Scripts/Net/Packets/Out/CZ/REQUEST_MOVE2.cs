using System.IO;

public partial class CZ {

    public const PacketHeader HEADER = PacketHeader.CZ_REQUEST_MOVE2;
    public const int SIZE = 5;

    public class REQUEST_MOVE2 : OutPacket {

        private short x;
        private short y;
        private byte dir;

        public REQUEST_MOVE2(int x, int y, int dir) : base(HEADER, SIZE) {
            this.x = (short)x;
            this.y = (short)y;
            this.dir = (byte)dir;
        }

        public override bool Send(BinaryWriter writer) {
            base.Send(writer);

            writer.WritePos(x, y, dir);

            writer.Flush();

            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class CZ {

    public const PacketHeader HEADER = PacketHeader.CZ_REQUEST_MOVE2;
    public const int SIZE = 5;

    public class REQUEST_MOVE2 : OutPacket {

        private int destX;
        private int destY;
        private int dir;

        public REQUEST_MOVE2(int destX, int destY, int dir) : base(HEADER, SIZE) {
            this.destX = destX;
            this.destY = destY;
            this.dir = dir;
        }

        public override bool Send(BinaryWriter writer) {
            base.Send(writer);

            return true;
        }
    }
}

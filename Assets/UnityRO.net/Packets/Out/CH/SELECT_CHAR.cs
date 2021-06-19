using System;
using System.Linq;

public partial class CH {

    public class SELECT_CHAR : OutPacket {

        private const PacketHeader HEADER = PacketHeader.CH_SELECT_CHAR;
        private const int SIZE = 3;

        private int charIndex;

        public SELECT_CHAR(int num) : base(HEADER, SIZE){
            this.charIndex = num;
        }

        public override void Send() {
            Write((byte)charIndex);

            base.Send();
        }
    }
}

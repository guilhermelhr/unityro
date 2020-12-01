using System.IO;

public partial class CH {
    public class SELECT_CHAR : OutPacket {

        private const PacketHeader HEADER = PacketHeader.CH_SELECT_CHAR;
        private const int SIZE = 2 + 1;

        private int charIndex;

        public SELECT_CHAR(int num) : base(HEADER, SIZE){
            this.charIndex = num;
        }

        public override bool Send(BinaryWriter writer) {
            base.Send(writer);

            writer.Write((byte)charIndex);
            writer.Flush();

            return true;
        }
    }
}

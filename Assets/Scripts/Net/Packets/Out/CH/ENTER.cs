using System.IO;

public partial class CH {
    public class ENTER : OutPacket {

        private const PacketHeader HEADER = PacketHeader.CH_ENTER;
        private const int SIZE = 2 + 4 + 4 + 4 + 2 + 1;

        private int aid, lig1, lig2;
        private byte sex;

        public ENTER(int aid, int lig1, int lig2, byte sex) : base(HEADER, SIZE) {
            this.aid = aid;
            this.lig1 = lig1;
            this.lig2 = lig2;
            this.sex = sex;
        }

        public override bool Send(BinaryWriter writer) {
            base.Send(writer);

            writer.Write(aid);
            writer.Write(lig1);
            writer.Write(lig2);
            writer.Write((short)0);
            writer.Write(sex);
            writer.Flush();

            return true;
        }
    }
}

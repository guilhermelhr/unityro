using System.IO;

public partial class CZ {
    public class ENTER : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_ENTER;
        public const int SIZE = 13;

        private int aid, gid, auth;
        private byte sex;

        public ENTER(int aid, int gid, int auth, byte sex) : base(HEADER, SIZE) {
            this.aid = aid;
            this.gid = gid;
            this.auth = auth;
            this.sex = sex;
        }

        public override bool Send(BinaryWriter writer) {
            base.Send(writer);

            writer.Write(aid);
            writer.Write(gid);
            writer.Write(auth);
            writer.Write(0);
            writer.Write(sex);
            writer.Flush();

            return true;
        }
    }
}

using System.IO;

public partial class CA {
    public class LOGIN : OutPacket {
        
        private const ushort header = 0x64;
        private const int size = 2 + 4 + 24 + 24 + 1;

        private string ID;
        private string Passwd;
        private int Version;
        private int clienttype;

        public LOGIN(string ID, string Passwd, int clienttype, int Version = 0) : base(header, size) {
            this.ID = ID;
            this.Passwd = Passwd;
            this.clienttype = clienttype;
            this.Version = Version;
        }

        override public bool Send(BinaryWriter writer) {
            base.Send(writer);

            writer.Write((int)Version);
            writer.WriteCString(ID, 24);
            writer.WriteCString(Passwd, 24);
            writer.Write((byte)clienttype);
            writer.Flush();

            return true;
        }
    }

}
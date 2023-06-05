public partial class CA {

    public class LOGIN : OutPacket {
        
        private const PacketHeader header = PacketHeader.CA_LOGIN;
        private const int size = 55;

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

        override public void Send() {
            Write(Version);
            Write(ID, 24);
            Write(Passwd, 24);
            Write((byte)clienttype);

            base.Send();
        }
    }

}
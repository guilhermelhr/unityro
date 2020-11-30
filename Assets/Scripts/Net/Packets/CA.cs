using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class CA {

    public class Login : OutPacket {
        
        private const ushort header = 0x64;
        private const int size = 2 + 4 + 24 + 24 + 1;

        private string ID;
        private string Passwd;
        private int Version;
        private int clienttype;

        public Login(string ID, string Passwd, int clienttype, int Version = 0) : base(header, 55) {
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
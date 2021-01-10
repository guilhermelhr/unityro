using System.IO;
using System.Net;

public partial class AC {
    [PacketHandler(HEADER, "AC_ACCEPT_LOGIN3")]
    public class ACCEPT_LOGIN3 : InPacket {

        public const PacketHeader HEADER = PacketHeader.AC_ACCEPT_LOGIN3;
        public const int BLOCK_SIZE = 32;

        public int LoginID1 { get; set; }
        public int AccountID { get; set; }
        public int LoginID2 { get; set; }
        public byte Sex { get; set; }
        public CharServerInfo[] Servers { get; set; }

        public void Read(BinaryReader br, int size) {

            LoginID1 = br.ReadLong();
            AccountID = br.ReadLong();
            LoginID2 = br.ReadLong();

            br.Seek(30, SeekOrigin.Current);

            Sex = br.ReadUByte();

            br.Seek(17, SeekOrigin.Current);

            long serverCount = (br.Length - br.Position) / BLOCK_SIZE;
            Servers = new CharServerInfo[serverCount];
            for(int i = 0; i < serverCount; i++) {
                CharServerInfo csi = new CharServerInfo();
                csi.IP = new IPAddress(br.ReadULong());
                csi.Port = br.ReadUShort();
                csi.Name = br.ReadBinaryString(20);
                csi.UserCount = br.ReadUShort();
                csi.State = br.ReadShort();
                csi.Property = br.ReadUShort();

                Servers[i] = csi;
            }
        }
    }
}
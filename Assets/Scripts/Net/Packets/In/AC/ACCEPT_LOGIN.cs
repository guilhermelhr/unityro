using System.IO;
using System.Net;

public partial class AC {
    [PacketHandler(
        (ushort)HEADER,
        "AC_ACCEPT_LOGIN",
        PacketHandlerAttribute.VariableSize,
        PacketHandlerAttribute.PacketDirection.In
    )]
    public class ACCEPT_LOGIN : InPacket {

        public const PacketHeader HEADER = PacketHeader.AC_ACCEPT_LOGIN3;
        public int LoginID1 { get; set; }
        public int AccountID { get; set; }
        public int LoginID2 { get; set; }
        public byte Sex { get; set; }

        public CharServerInfo[] Servers { get; set; }

        public bool Read(byte[] data) {
            BinaryReader br = new BinaryReader(data);

            LoginID1 = br.ReadLong();
            AccountID = br.ReadLong();
            LoginID2 = br.ReadLong();
            br.Seek(30, SeekOrigin.Current);
            Sex = br.ReadUByte();
            br.Seek(17, SeekOrigin.Current);

            /**
             * This seems very wrong to me
             * Which always return 5
             * Is there another way of knowing the server count?
             * header(64) + size (160) * server_num (taken from rAthena)
             * Note: Here we've already skipped 4 bytes from reading the cmd and size
             */

            long serverCount = (data.Length - br.Position) / 32;
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

            return true;
        }
    }
}
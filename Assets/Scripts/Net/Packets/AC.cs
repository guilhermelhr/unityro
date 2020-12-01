using System.IO;

public class AC {


    [PacketHandler(
        (ushort)PacketHeader.AC.ACCEPT_LOGIN3,
        "AC_ACCEPT_LOGIN",
        PacketHandlerAttribute.VariableSize,
        PacketHandlerAttribute.PacketDirection.In
    )]
    public class AcceptLogin : InPacket {

        public const ushort HEADER = (ushort)PacketHeader.AC.ACCEPT_LOGIN3;

        public int LoginID1 { get; set; }
        public int AccountID { get; set; }
        public int LoginID2 { get; set; }
        public byte Sex { get; set; }

        public bool Read(byte[] data) {
            System.IO.BinaryReader br = new System.IO.BinaryReader(new MemoryStream(data));

            int serverCount = (data.Length - 43) / 32;

            LoginID1 = br.ReadInt32();
            AccountID = br.ReadInt32();
            LoginID2 = br.ReadInt32();

            br.ReadBytes(30);

            Sex = br.ReadByte();


            return true;
        }
    }
}
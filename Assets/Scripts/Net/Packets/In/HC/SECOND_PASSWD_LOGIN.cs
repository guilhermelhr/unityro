using System;

public partial class HC {

    [PacketHandler(HEADER, "HC_SECOND_PASSWD_LOGIN", SIZE)]
    public class SECOND_PASSWD_LOGIN : InPacket {

        public const PacketHeader HEADER = PacketHeader.HC_SECOND_PASSWD_LOGIN;
        public const int SIZE = 12;

        public void Read(BinaryReader br, int size) {
            var seed = br.ReadLong();
            var accountId = br.ReadLong();
            var state = br.ReadShort();
        }
    }
}

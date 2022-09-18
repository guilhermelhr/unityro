using ROIO.Utils;
using System;

public partial class HC {

    [PacketHandler(HEADER, "HC_SECOND_PASSWD_LOGIN", SIZE)]
    public class SECOND_PASSWD_LOGIN : InPacket {

        public const PacketHeader HEADER = PacketHeader.HC_SECOND_PASSWD_LOGIN;
        public PacketHeader Header => HEADER;
        public const int SIZE = 12;

        public void Read(MemoryStreamReader br, int size) {
            var seed = br.ReadInt();
            var accountId = br.ReadInt();
            var state = br.ReadShort();
        }
    }
}

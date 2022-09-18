using ROIO.Utils;
using System;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_MSG", SIZE)]
    public class MSG : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_MSG;
        public const int SIZE = 4;
        public PacketHeader Header => HEADER;

        public ushort MessageID;

        public void Read(MemoryStreamReader br, int size) {
            MessageID = br.ReadUShort();
        }
    }
}
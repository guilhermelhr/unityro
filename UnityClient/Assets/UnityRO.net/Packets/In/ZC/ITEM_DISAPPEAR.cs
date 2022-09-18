using ROIO.Utils;
using System;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_ITEM_DISAPPEAR", SIZE)]
    public class ITEM_DISAPPEAR : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_ITEM_DISAPPEAR;
        public const int SIZE = 6;
        public PacketHeader Header => HEADER;

        public uint AID;

        public void Read(MemoryStreamReader br, int size) {
            AID = br.ReadUInt();
        }
    }
}

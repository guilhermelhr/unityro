using ROIO.Utils;
using System;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_INVENTORY_EXPANSION_INFO", SIZE)]
    public class INVENTORY_EXPANSION_INFO : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_INVENTORY_EXPANSION_INFO;
        public const int SIZE = 4;
        public PacketHeader Header => HEADER;

        public short ExpansionSize;

        public void Read(MemoryStreamReader br, int size) {
            ExpansionSize = br.ReadShort();
        }
    }
}

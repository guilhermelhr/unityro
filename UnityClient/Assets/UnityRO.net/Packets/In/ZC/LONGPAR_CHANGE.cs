using ROIO.Utils;
using System;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_LONGPAR_CHANGE", SIZE)]
    public class LONGPAR_CHANGE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_LONGPAR_CHANGE;
        public const int SIZE = 8;
        public PacketHeader Header => HEADER;

        public EntityStatus varID;
        public int value;

        public void Read(MemoryStreamReader br, int size) {
            varID = (EntityStatus)br.ReadUShort();
            value = br.ReadInt();
        }
    }
}

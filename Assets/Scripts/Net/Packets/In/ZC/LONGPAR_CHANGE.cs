using System;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_LONGPAR_CHANGE", SIZE)]
    public class LONGPAR_CHANGE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_LONGPAR_CHANGE;
        public const int SIZE = 8;

        public EntityStatus varID;
        public int value;

        public bool Read(BinaryReader br) {
            varID = (EntityStatus)br.ReadUShort();
            value = br.ReadLong();

            return true;
        }
    }
}

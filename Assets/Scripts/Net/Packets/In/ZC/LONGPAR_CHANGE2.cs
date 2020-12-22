using System;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_LONGPAR_CHANGE2", SIZE)]
    public class LONGPAR_CHANGE2 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_LONGPAR_CHANGE2;
        public const int SIZE = 12;

        public EntityStatus varID;
        public int value;

        public bool Read(BinaryReader br) {
            varID = (EntityStatus)br.ReadUShort();
            value = br.ReadLong();

            return true;
        }
    }
}

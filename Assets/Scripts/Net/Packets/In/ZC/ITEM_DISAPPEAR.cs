using System;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_ITEM_DISAPPEAR", SIZE)]
    public class ITEM_DISAPPEAR : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_ITEM_DISAPPEAR;
        public const int SIZE = 6;

        public uint GID;

        public bool Read(BinaryReader br) {

            GID = br.ReadULong();

            return true;
        }
    }
}

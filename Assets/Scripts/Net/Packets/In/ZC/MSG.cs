using System;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_MSG", SIZE)]
    public class MSG : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_MSG;
        public const int SIZE = 4;

        public ushort MessageID;

        public bool Read(BinaryReader br) {

            MessageID = br.ReadUShort();

            return true;
        }
    }
}
public partial class ZC {

    [PacketHandler(HEADER, "ZC_STOPMOVE", SIZE)]
    public class STOPMOVE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_STOPMOVE;
        public const int SIZE = 10;

        public uint AID;
        public short PosX;
        public short PosY;

        public bool Read(BinaryReader br) {

            AID = br.ReadULong();
            PosX = br.ReadShort();
            PosY = br.ReadShort();

            return true;
        }
    }
}
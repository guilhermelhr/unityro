public partial class ZC {

    [PacketHandler(HEADER, "ZC_USESKILL_ACK2", SIZE)]
    public class USESKILL_ACK2 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_USESKILL_ACK2;
        public const int SIZE = 25;

        public uint AID;
        public uint targetID;
        public short xPos;
        public short yPos;
        public uint delayTime;
        public byte isDisposable;
        public uint property;

        public ushort SKID { get; private set; }

        public void Read(BinaryReader fp, int size) {
            AID = fp.ReadULong();
            targetID = fp.ReadULong();
            xPos = fp.ReadShort();
            yPos = fp.ReadShort();
            SKID = fp.ReadUShort();
            property = fp.ReadULong();
            delayTime = fp.ReadULong();
            isDisposable = fp.ReadUByte();
        }
    }
}
using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_USESKILL_ACK2", SIZE)]
    public class USESKILL_ACK2 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_USESKILL_ACK2;
        public const int SIZE = 25;
        public PacketHeader Header => HEADER;

        public uint AID;
        public uint targetID;
        public short xPos;
        public short yPos;
        public uint delayTime;
        public byte isDisposable;
        public uint property;
        public ushort SKID;

        public void Read(MemoryStreamReader fp, int size) {
            AID = fp.ReadUInt();
            targetID = fp.ReadUInt();
            xPos = fp.ReadShort();
            yPos = fp.ReadShort();
            SKID = fp.ReadUShort();
            property = fp.ReadUInt();
            delayTime = fp.ReadUInt();
            isDisposable = (byte) fp.ReadByte();
        }
    }
}
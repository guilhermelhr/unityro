using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_SKILL2", SIZE)]
    public class NOTIFY_SKILL2 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_SKILL2;
        public PacketHeader Header => HEADER;
        public const int SIZE = 33;

        public short SKID;
        public uint AID;
        public uint targetID;
        public uint startTime;
        public int attackMT;
        public int attackedMT;
        public int damage;
        public short level;
        public short count;
        public byte action;

        public void Read(MemoryStreamReader fp, int size) {
            this.SKID = fp.ReadShort();
            this.AID = (uint) fp.ReadInt();
            this.targetID = (uint) fp.ReadInt();
            this.startTime = (uint) fp.ReadInt();
            this.attackMT = fp.ReadInt();
            this.attackedMT = fp.ReadInt();
            this.damage = fp.ReadInt();
            this.level = fp.ReadShort();
            this.count = fp.ReadShort();
            this.action = (byte) fp.ReadByte();
        }
    }
}

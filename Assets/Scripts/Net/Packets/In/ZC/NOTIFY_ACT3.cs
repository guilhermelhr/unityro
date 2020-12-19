using System;
using System.IO;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_ACT3", SIZE)]
    public class NOTIFY_ACT3 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_ACT3;
        public const int SIZE = 34;

        public uint GID;
        public uint targetGID;
        public uint startTime;
        public int attackMT;
        public int attackedMT;
        public int damage;
        public short count;
        public byte action;
        public int leftDamage;

        public bool Read(BinaryReader br) {

            this.GID = br.ReadULong();
            this.targetGID = br.ReadULong();
            this.startTime = br.ReadULong();
            this.attackMT = br.ReadLong();
            this.attackedMT = br.ReadLong();
            this.damage = br.ReadLong();

            br.Seek(1, SeekOrigin.Current);

            this.count = br.ReadShort();
            this.action = br.ReadUByte();
            this.leftDamage = br.ReadLong();

            return true;
        }
    }
}

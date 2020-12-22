using System;
using System.IO;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_ACT", SIZE)]
    public class NOTIFY_ACT : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_ACT;
        public const int SIZE = 29;

        public EntityActionRequest ActionRequest;

        public bool Read(BinaryReader fp) {
            ActionRequest = new EntityActionRequest() {
                GID = fp.ReadULong(),
                targetGID = fp.ReadULong(),
                startTime = fp.ReadULong(),
                attackMT = fp.ReadLong(),
                attackedMT = fp.ReadLong(),
                damage = fp.ReadShort(),
                count = fp.ReadShort(),
                action = fp.ReadUByte(),
                leftDamage = fp.ReadShort()
            };

            return true;
        }
    }
}

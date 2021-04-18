using System;
using System.IO;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_ACT", SIZE)]
    public class NOTIFY_ACT : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_ACT;
        public const int SIZE = 29;

        public EntityActionRequest ActionRequest;

        public void Read(BinaryReader fp, int size) {
            ActionRequest = new EntityActionRequest() {
                GID = fp.ReadULong(),
                targetGID = fp.ReadULong(),
                startTime = fp.ReadULong(),
                sourceSpeed = (ushort)fp.ReadLong(),
                targetSpeed = (ushort)fp.ReadLong(),
                damage = fp.ReadShort(),
                count = fp.ReadShort(),
                action = (ActionRequestType) fp.ReadUByte(),
                leftDamage = fp.ReadShort()
            };
        }
    }
}

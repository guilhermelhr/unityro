using ROIO.Utils;
using System;
using System.IO;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_ACT", SIZE)]
    public class NOTIFY_ACT : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_ACT;
        public const int SIZE = 29;
        public PacketHeader Header => HEADER;

        public EntityActionRequest ActionRequest;

        public void Read(MemoryStreamReader fp, int size) {
            ActionRequest = new EntityActionRequest() {
                GID = fp.ReadUInt(),
                targetGID = fp.ReadUInt(),
                startTime = fp.ReadUInt(),
                sourceSpeed = (ushort)fp.ReadInt(),
                targetSpeed = (ushort)fp.ReadInt(),
                damage = fp.ReadShort(),
                count = fp.ReadShort(),
                action = (ActionRequestType) fp.ReadByte(),
                leftDamage = fp.ReadShort()
            };
        }
    }
}

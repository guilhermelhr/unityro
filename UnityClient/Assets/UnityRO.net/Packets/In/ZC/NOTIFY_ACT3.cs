using ROIO.Utils;
using System;
using System.IO;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_ACT3", SIZE)]
    public class NOTIFY_ACT3 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_ACT3;
        public const int SIZE = 34;
        public PacketHeader Header => HEADER;

        public EntityActionRequest ActionRequest;

        public void Read(MemoryStreamReader br, int size) {
            ActionRequest = new EntityActionRequest() {
                GID = br.ReadUInt(),
                targetGID = br.ReadUInt(),
                startTime = br.ReadUInt(),
                sourceSpeed = (ushort)br.ReadInt(),
                targetSpeed = (ushort)br.ReadInt(),
                damage = br.ReadInt()
            };

            br.Seek(1, SeekOrigin.Current);

            ActionRequest.count = br.ReadShort();
            ActionRequest.action = (ActionRequestType) br.ReadByte();
            ActionRequest.leftDamage = br.ReadInt();
        }
    }
}

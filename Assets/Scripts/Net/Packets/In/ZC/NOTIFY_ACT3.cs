using System;
using System.IO;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_ACT3", SIZE)]
    public class NOTIFY_ACT3 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_ACT3;
        public const int SIZE = 34;

        public EntityActionRequest ActionRequest;

        public bool Read(BinaryReader br) {
            ActionRequest = new EntityActionRequest() {
                GID = br.ReadULong(),
                targetGID = br.ReadULong(),
                startTime = br.ReadULong(),
                attackMT = br.ReadLong(),
                attackedMT = br.ReadLong(),
                damage = br.ReadLong()
            };

            br.Seek(1, SeekOrigin.Current);

            ActionRequest.count = br.ReadShort();
            ActionRequest.action = br.ReadUByte();
            ActionRequest.leftDamage = br.ReadLong();

            return true;
        }
    }
}

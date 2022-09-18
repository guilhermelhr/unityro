using ROIO.Utils;
using System;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_VANISH", SIZE)]
    public class NOTIFY_VANISH : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_VANISH;
        public const int SIZE = 7;
        public PacketHeader Header => HEADER;

        public uint AID;
        public int Type;

        public void Read(MemoryStreamReader br, int size) {
            AID = br.ReadUInt();
            Type = br.ReadByte();
        }
    }
}

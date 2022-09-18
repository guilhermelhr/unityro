using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_RANKING", SIZE)]
    public class NOTIFY_RANKING : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_RANKING;
        public PacketHeader Header => HEADER;
        public const int SIZE = 14;

        public uint AID;
        public int Ranking;
        public int Total;

        public void Read(MemoryStreamReader br, int size) {
            AID = br.ReadUInt();
            Ranking = br.ReadInt();
            Total = br.ReadInt();
        }
    }
}

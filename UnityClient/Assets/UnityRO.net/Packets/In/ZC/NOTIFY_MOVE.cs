using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_MOVE", SIZE)]
    public class NOTIFY_MOVE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_MOVE;
        public PacketHeader Header => HEADER;
        public const int SIZE = 16;

        public uint GID;
        public int[] StartPosition;
        public int[] EndPosition;
        public uint MoveStartTime;

        public void Read(MemoryStreamReader br, int size) {
            this.GID = br.ReadUInt();
            var moveData = br.ReadPos2();
            StartPosition = new int[2] { moveData[0], moveData[1] };
            EndPosition = new int[2] { moveData[2], moveData[3] };
            this.MoveStartTime = br.ReadUInt();
        }
    }
}

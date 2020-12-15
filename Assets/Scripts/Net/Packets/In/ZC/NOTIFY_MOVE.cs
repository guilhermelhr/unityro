public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_MOVE", SIZE)]
    public class NOTIFY_MOVE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_MOVE;
        public const int SIZE = 16;

        public uint GID;
        public int[] StartPosition;
        public int[] EndPosition;
        public uint MoveStartTime;

        public bool Read(BinaryReader br) {
            this.GID = br.ReadULong();
            var moveData = br.ReadPos2();
            StartPosition = new int[2] { moveData[0], moveData[1] };
            EndPosition = new int[2] { moveData[2], moveData[3] };
            this.MoveStartTime = br.ReadULong();

            return true;
        }
    }
}

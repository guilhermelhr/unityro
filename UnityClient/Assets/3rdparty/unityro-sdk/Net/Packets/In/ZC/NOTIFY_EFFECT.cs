using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_EFFECT", SIZE)]
    public class NOTIFY_EFFECT : InPacket {
        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_EFFECT;
        public const int SIZE = 10;

        public PacketHeader Header => HEADER;

        public int sourceAID;

        /// effect id:
        ///     0 = base level up
        ///     1 = job level up
        ///     2 = refine failure
        ///     3 = refine success
        ///     4 = game over
        ///     5 = pharmacy success
        ///     6 = pharmacy failure
        ///     7 = base level up (super novice)
        ///     8 = job level up (super novice)
        ///     9 = base level up (taekwon)
        public int effectID;

        public void Read(MemoryStreamReader br, int size) {
            sourceAID = br.ReadInt();
            effectID = br.ReadInt();
        }
    }
}

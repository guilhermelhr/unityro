
public partial class ZC {

    [PacketHandler(HEADER, "ZC_ATTACK_RANGE", SIZE)]
    public class ATTACK_RANGE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_ATTACK_RANGE;
        public const int SIZE = 4;

        public bool Read(BinaryReader br) {
            var currentRange = br.ReadShort();

            return true;
        }
    }
}

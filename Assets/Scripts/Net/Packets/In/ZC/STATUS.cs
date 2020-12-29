public partial class ZC {

    [PacketHandler(HEADER, "ZC_STATUS", SIZE)]
    public class STATUS : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_STATUS;
        public const int SIZE = 44;

        /// Character status (ZC_STATUS).
        /// 00bd 
        /// <stpoint>.W 
        /// <str>.B <need str>.B 
        /// <agi>.B <need agi>.B 
        /// <vit>.B <need vit>.B
        /// <int>.B <need int>.B 
        /// <dex>.B <need dex>.B 
        /// <luk>.B <need luk>.B 
        /// <atk>.W <atk2>.W
        /// <matk min>.W <matk max>.W 
        /// <def>.W <def2>.W
        /// <mdef>.W <mdef2>.W 
        /// <hit>.W
        /// <flee>.W <flee2>.W 
        /// <crit>.W 
        /// <aspd>.W <aspd2>.W
        public bool Read(BinaryReader br) {

            return true;
        }
    }
}

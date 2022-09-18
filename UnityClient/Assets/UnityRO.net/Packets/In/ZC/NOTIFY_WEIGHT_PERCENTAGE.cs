using ROIO.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_WEIGHT_PERCENTAGE", SIZE)]
    public class NOTIFY_WEIGHT_PERCENTAGE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_WEIGHT_PERCENTAGE;
        public const int SIZE = 6;
        public PacketHeader Header => HEADER;

        public int WeightPercentage;

        public void Read(MemoryStreamReader br, int size) {
            WeightPercentage = br.ReadInt();
        }
    }
}

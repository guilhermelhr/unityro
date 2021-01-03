using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_MSG_STATE_CHANGE", SIZE)]
    public class MSG_STATE_CHANGE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_MSG_STATE_CHANGE;
        public const int SIZE = 9;

        public bool Read(BinaryReader br) {
            return true;
        }
    }
}

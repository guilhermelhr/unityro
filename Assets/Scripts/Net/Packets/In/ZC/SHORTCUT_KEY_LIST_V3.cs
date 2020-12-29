using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_SHORTCUT_KEY_LIST_V3", SIZE)]
    public class SHORTCUT_KEY_LIST_V3 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_SHORTCUT_KEY_LIST_V3;
        public const int SIZE = 269;

        public bool Read(BinaryReader br) {

            return true;
        }
    }
}

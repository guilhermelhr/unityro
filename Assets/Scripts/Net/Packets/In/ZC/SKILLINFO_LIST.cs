using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_SKILLINFO_LIST")]
    public class SKILLINFO_LIST : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_SKILLINFO_LIST;

        public bool Read(BinaryReader br) {

            return true;
        }
    }
}

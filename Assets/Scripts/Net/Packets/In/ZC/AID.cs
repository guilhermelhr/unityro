using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_AID", SIZE, PacketHandlerAttribute.PacketDirection.In)]
    public class AID : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_AID;
        public const int SIZE = 6;

        public PacketHeader GetHeader() {
            return HEADER;
        }

        public bool Read(BinaryReader br) {

            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_PLAYERMOVE", SIZE, PacketHandlerAttribute.PacketDirection.In)]
    public class NOTIFY_PLAYERMOVE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_PLAYERMOVE;
        public const int SIZE = 12;

        public bool Read(byte[] data) {


            return true;
        }
    }
}

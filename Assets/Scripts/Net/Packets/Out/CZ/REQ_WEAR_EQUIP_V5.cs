using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class CZ {
    public class REQ_WEAR_EQUIP_V5 : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_REQ_WEAR_EQUIP_V5;
        public const int SIZE = 8;

        public short index;
        public int location;

        public REQ_WEAR_EQUIP_V5() : base(HEADER, SIZE) { }

        public override bool Send(BinaryWriter writer) {
            base.Send(writer);

            writer.Write(index);
            writer.Write(location);
            writer.Flush();

            return true;
        }
    }
}

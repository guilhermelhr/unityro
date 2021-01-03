using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class CZ {
    public class USE_ITEM2 : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_USE_ITEM2;
        public const int SIZE = 8;

        public long AID;
        public int index;

        public USE_ITEM2() : base(HEADER, SIZE) { }

        public override bool Send(BinaryWriter writer) {
            base.Send(writer);

            writer.Write(index);
            writer.Write(AID);
            writer.Flush();

            return true;
        }
    }
}

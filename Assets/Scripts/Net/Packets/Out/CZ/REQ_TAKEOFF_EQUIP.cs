using System;
using System.IO;

public partial class CZ {
    public class REQ_TAKEOFF_EQUIP : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_REQ_TAKEOFF_EQUIP;
        public const int size = 4;

        public REQ_TAKEOFF_EQUIP() : base(HEADER, SIZE) { }

        public short index;

        public override bool Send(BinaryWriter writer) {
            base.Send(writer);

            writer.Write(index);
            writer.Flush();

            return true;
        }
    }
}

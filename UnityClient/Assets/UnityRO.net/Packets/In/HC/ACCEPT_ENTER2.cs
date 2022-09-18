using ROIO.Utils;
using System.IO;
using System.Runtime.InteropServices;

public partial class HC {

    [PacketHandler(HEADER, "HC_ACCEPT_ENTER2", 29)]
    public class ACCEPT_ENTER2 : InPacket {

        public const PacketHeader HEADER = PacketHeader.HC_ACCEPT_ENTER2;
        public PacketHeader Header => HEADER;
        public void Read(MemoryStreamReader br, int Size) {

            var size = br.ReadShort();
            var normal_slot = br.ReadByte(); //MIN_CHARS 15
            var premium_slot = br.ReadByte(); //chars_vip;
            var billing_slot = br.ReadByte();
            var producible_slot = br.ReadByte();
            var valid_slot = br.ReadByte(); //MAX_CHARS 15

            br.Seek(9, SeekOrigin.Current);
        }
    }
}

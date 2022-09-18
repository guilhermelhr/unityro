using ROIO.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NPCSPRITE_CHANGE", SIZE)]
    public class NPCSPRITE_CHANGE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NPCSPRITE_CHANGE;
        public const int SIZE = 11;
        public PacketHeader Header => HEADER;

        public uint GID;
        public byte objecttype;
        public uint value;

        public void Read(MemoryStreamReader br, int size) {
            this.GID = br.ReadUInt();
            this.objecttype = (byte)br.ReadByte();
            this.value = br.ReadUInt();
        }
    }
}

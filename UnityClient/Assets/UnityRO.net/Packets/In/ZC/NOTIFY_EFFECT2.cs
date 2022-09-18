using ROIO.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_EFFECT2", SIZE)]
    public class NOTIFY_EFFECT2 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_EFFECT2;
        public PacketHeader Header => HEADER;
        public const int SIZE = 10;

        public uint GID;
        public int EffectId;

        public void Read(MemoryStreamReader br, int size) {
            GID = br.ReadUInt();
            EffectId = br.ReadInt();
        }
    }
}

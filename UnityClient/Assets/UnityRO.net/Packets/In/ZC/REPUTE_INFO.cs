using ROIO.Utils;
using System.Collections.Generic;
using UnityEngine;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_REPUTE_INFO")]
    public class REPUTE_INFO : InPacket {

        private const int BLOCK_SIZE = 16;
        public const PacketHeader HEADER = PacketHeader.ZC_REPUTE_INFO;
        public PacketHeader Header => HEADER;

        public byte success;
        public List<REPUTE_INFO_sub> entries = new List<REPUTE_INFO_sub>();

        public class REPUTE_INFO_sub {
            public ulong type;
            public long points;
        }

        public void Read(MemoryStreamReader br, int size) {
            success = (byte) br.ReadByte();

            var count = (br.Length - br.Position) / BLOCK_SIZE;
            for (int i = 0; i < count; i++) {
                var entry = new REPUTE_INFO_sub {
                    type = br.ReadULong(),
                    points = br.ReadLong()
                };
                entries.Add(entry);
            }
        }
    }
}

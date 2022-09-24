using ROIO.Utils;
using System;
using System.Collections.Generic;
using System.IO;

public partial class HC {

    [PacketHandler(HEADER, "HC_ACCEPT_ENTER")]
    public class ACCEPT_ENTER : InPacket {

        public const PacketHeader HEADER = PacketHeader.HC_ACCEPT_ENTER;
        public const int MAX_CHARS = 15;

        public int MaxSlots { get; set; }
        public int AvailableSlots { get; set; }
        public int PremiumSlots { get; set; }
        public List<CharacterData> Chars { get; set; }

        public PacketHeader Header => HEADER;

        public void Read(MemoryStreamReader br, int size) {
            /**
             * There's this on rA source:
             * j = 24 + offset; // offset
             * WFIFOHEAD(fd,j + MAX_CHARS*MAX_CHAR_BUF);
             * which means, 
             * 20bytes we skip with br.Seek
             * +4 bytes of the characters block size
             * +3 bytes of the next 3 fields
             */

            MaxSlots = br.ReadByte();
            PremiumSlots = br.ReadByte();
            AvailableSlots = br.ReadByte();

            br.Seek(20, SeekOrigin.Current);
            int numChars = (int)((br.Length - br.Position) / CharacterData.BLOCK_SIZE);

            Chars = new List<CharacterData>();
            for(int i = 0; i < numChars; i++) {
                Chars.Add(CharacterData.parse(br));
            }
        }
    }
}

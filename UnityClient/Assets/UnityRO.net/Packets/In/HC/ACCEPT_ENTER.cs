using ROIO.Utils;
using System;
using System.Collections.Generic;
using System.IO;

public partial class HC {

    [PacketHandler(HEADER, "HC_ACCEPT_ENTER")]
    public class ACCEPT_ENTER : InPacket {

        public const PacketHeader HEADER = PacketHeader.HC_ACCEPT_ENTER;

        public int MaxSlots { get; set; }
        public int AvailableSlots { get; set; }
        public int PremiumSlots { get; set; }
        public List<CharacterData> Chars { get; set; }

        public PacketHeader GetHeader() => HEADER;

        public void Read(MemoryStreamReader br, int size) {

            int numChars = (int)((br.Length - 23) / 144);

            MaxSlots = br.ReadByte();
            PremiumSlots = br.ReadByte();
            AvailableSlots = br.ReadByte();

            br.Seek(20, SeekOrigin.Current);

            Chars = new List<CharacterData>();
            for(int i = 0; i < numChars; i++) {
                Chars.Add(CharacterData.parse(br));
            }
        }
    }
}

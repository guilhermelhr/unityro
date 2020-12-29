using System.IO;

public partial class HC {

    [PacketHandler(HEADER, "HC_ACCEPT_ENTER2", 29)]
    public class ACCEPT_ENTER2 : InPacket {

        public const PacketHeader HEADER = PacketHeader.HC_ACCEPT_ENTER2;

        public int MaxSlots { get; set; }
        public int AvailableSlots { get; set; }
        public int PremiumSlots { get; set; }
        public CharacterData[] Chars { get; set; }

        public PacketHeader GetHeader() => HEADER;

        public bool Read(BinaryReader br) {

            var size = br.ReadShort();
            var normal_slot = br.ReadByte(); //MIN_CHARS 15
            var premium_slot = br.ReadByte(); //chars_vip;
            var billing_slot = br.ReadByte();
            var producible_slot = br.ReadByte();
            var valid_slot = br.ReadByte(); //MAX_CHARS 15

            br.Seek(9, SeekOrigin.Current);

            return true;
        }
    }
}

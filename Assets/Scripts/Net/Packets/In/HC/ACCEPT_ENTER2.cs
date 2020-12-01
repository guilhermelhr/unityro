using System;
using System.IO;

public partial class HC {

    [PacketHandler(HEADER,
        "HC_ACCEPT_ENTER2",
        PacketHandlerAttribute.VariableSize,
        PacketHandlerAttribute.PacketDirection.In
        )]
    public class ACCEPT_ENTER2 : InPacket {

        public const PacketHeader HEADER = PacketHeader.HC_ACCEPT_ENTER2;

        public int MaxSlots { get; set; }
        public int AvailableSlots { get; set; }
        public int PremiumSlots { get; set; }
        public CharServerChatData[] Chars { get; set; }

        public bool Read(byte[] data) {
            BinaryReader br = new BinaryReader(data);

            var normal_slot = br.ReadUByte();
            var premium_slot = br.ReadUByte();
            var billing_slot = br.ReadUByte();
            var producible_slot = br.ReadUByte();
            var valid_slot = br.ReadByte();

            br.Seek(20, SeekOrigin.Current);

            return true;
        }
    }
}

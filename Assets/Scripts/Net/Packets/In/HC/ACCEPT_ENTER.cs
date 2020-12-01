using System;
using System.IO;

public partial class HC {

    [PacketHandler((ushort)HEADER,
        "HC_ACCEPT_ENTER",
        PacketHandlerAttribute.VariableSize,
        PacketHandlerAttribute.PacketDirection.In
        )]
    public class ACCEPT_ENTER : InPacket {

        public const PacketHeader HEADER = PacketHeader.HC_ACCEPT_ENTER;

        public int MaxSlots { get; set; }
        public int AvailableSlots { get; set; }
        public int PremiumSlots { get; set; }
        public CharServerChatData[] Chars { get; set; }

        public bool Read(byte[] data) {
            BinaryReader br = new BinaryReader(data);

            int numChars = (data.Length - 23) / 144;

            MaxSlots = br.ReadUByte();
            PremiumSlots = br.ReadUByte();
            AvailableSlots = br.ReadUByte();

            br.Seek(20, SeekOrigin.Current);

            Chars = new CharServerChatData[numChars];
            for(int i = 0; i < numChars; i++) {
                CharServerChatData cd = new CharServerChatData();

                cd.GID = br.ReadLong();
                cd.Exp = br.ReadLong();
                br.Seek(4, SeekOrigin.Current);
                cd.Zeny = br.ReadLong();
                cd.JobExp = br.ReadLong();
                br.Seek(4, SeekOrigin.Current);
                cd.JobLevel = br.ReadLong();
                cd.BodyState = br.ReadLong();
                cd.HealthState = br.ReadLong();
                cd.Option = br.ReadLong();
                cd.Karma = br.ReadLong();
                cd.Manner = br.ReadLong();
                cd.StatusPoint = br.ReadShort();
                cd.HP = br.ReadLong();
                cd.MaxHP = br.ReadLong();
                cd.SP = br.ReadShort();
                cd.MaxSP = br.ReadShort();
                cd.Speed = br.ReadShort();
                cd.Job = br.ReadShort();

                cd.Hair = br.ReadShort();
                cd.Body = br.ReadShort();
                cd.Weapon = br.ReadShort();
                cd.BaseLevel = br.ReadShort();
                cd.SkillPoint = br.ReadShort();
                cd.Accessory = br.ReadShort();
                cd.Shield = br.ReadShort();
                cd.Accessory2 = br.ReadShort();
                cd.Accessory3 = br.ReadShort();
                cd.HairColor = br.ReadShort();
                cd.ClothesColor = br.ReadShort();
                cd.Name = br.ReadBinaryString(24);
                cd.Str = br.ReadUByte();
                cd.Agi = br.ReadUByte();
                cd.Vit = br.ReadUByte();
                cd.Int = br.ReadUByte();
                cd.Dex = br.ReadUByte();
                cd.Luk = br.ReadUByte();
                cd.Slot = br.ReadShort();
                cd.Rename = br.ReadShort();
                cd.MapName = br.ReadBinaryString(16);
                cd.DeleteDate = br.ReadLong();
                cd.Robe = br.ReadLong();
                cd.Moves = br.ReadLong();
                cd.AddOns = br.ReadLong();
                cd.Sex = br.ReadByte();

                Chars[i] = cd;
            }

            return true;
        }
    }
}

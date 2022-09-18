using ROIO.Utils;
using System;
using System.IO;

public partial class ZC {

    [PacketHandler(HEADER, "ACK_TOUSESKILL", SIZE)]
    public class ACK_TOUSESKILL : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_ACK_TOUSESKILL;
        public const int SIZE = 14;
        public PacketHeader Header => HEADER;

        public short SkillId;
        public int Type;
        public uint ItemId;
        public byte Flag;
        public byte Cause;

        public ACK_TOUSESKILL() { }

        public void Read(MemoryStreamReader br, int size) {
            SkillId = br.ReadShort();
            Type = br.ReadInt();
            ItemId = br.ReadUInt();
            Flag = (byte) br.ReadByte();
            Cause = (byte) br.ReadByte();
        }
    }
}

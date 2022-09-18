using ROIO.Utils;
using System;
public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_EXP2", SIZE)]
    public class NOTIFY_EXP2 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_EXP2;
        public PacketHeader Header => HEADER;
        public const int SIZE = 18;

        public uint id;
        public uint exp;
        public short expType;
        public short questExp;

        public void Read(MemoryStreamReader br, int size) {
            id = br.ReadUInt();
            exp = br.ReadUInt(); //negative if losing
            expType = br.ReadShort(); //SP_BASEEXP, SP_JOBEXP
            questExp = br.ReadShort();
        }
    }
}

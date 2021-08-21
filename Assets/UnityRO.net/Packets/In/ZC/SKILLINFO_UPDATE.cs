using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_SKILLINFO_UPDATE", SIZE)]
    public class SKILLINFO_UPDATE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_SKILLINFO_UPDATE;
        public const int SIZE = 11;

        public short SkillId;
        public short SkillLevel;
        public short SpCost;
        public short AttackRange;
        public bool IsUpgradeable;

        public void Read(MemoryStreamReader br, int size) {
            SkillId = br.ReadShort();
            SkillLevel = br.ReadShort();
            SpCost = br.ReadShort();
            AttackRange = br.ReadShort();
            IsUpgradeable = br.ReadByte() == 1;
        }
    }
}

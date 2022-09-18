using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_SKILLINFO_UPDATE", SIZE)]
    public class SKILLINFO_UPDATE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_SKILLINFO_UPDATE;
        public const int SIZE = 11;
        public PacketHeader Header => HEADER;

        public SkillInfo SkillInfo;

        public void Read(MemoryStreamReader br, int size) {
            SkillInfo = new SkillInfo {
                SkillID = br.ReadShort(),
                Level = br.ReadShort(),
                SpCost = br.ReadShort(),
                AttackRange = br.ReadShort(),
                CanUpgrade = br.ReadByte() == 1
            };
        }
    }
}

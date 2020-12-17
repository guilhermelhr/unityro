public partial class ZC {

    [PacketHandler(HEADER, "ZC_HP_INFO", SIZE)]
    public class HP_INFO : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_HP_INFO;
        public const int SIZE = 14;

        public uint GID;
        public int Hp;
        public int MaxHp;

        public bool Read(BinaryReader fp) {

            this.GID = fp.ReadULong();
            this.Hp = fp.ReadLong();
            this.MaxHp = fp.ReadLong();

            return true;
        }
    }

}
using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_HP_INFO", SIZE)]
    public class HP_INFO : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_HP_INFO;
        public const int SIZE = 14;
        public PacketHeader Header => HEADER;

        public uint GID;
        public int Hp;
        public int MaxHp;

        public void Read(MemoryStreamReader fp, int size) {
            this.GID = fp.ReadUInt();
            this.Hp = fp.ReadInt();
            this.MaxHp = fp.ReadInt();
        }
    }
}
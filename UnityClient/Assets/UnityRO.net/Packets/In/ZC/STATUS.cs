using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_STATUS", SIZE)]
    public class STATUS : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_STATUS;
        public const int SIZE = 44;
        public PacketHeader Header => HEADER;

        public short stpoint;
        public int str;
        public int needStr;
        public int agi;
        public int needAgi;
        public int vit;
        public int needVit;
        public int inte;
        public int needInte;
        public int dex;
        public int needDex;
        public int luk;
        public int needLuk;
        public short atk;
        public short atk2;
        public short matkMin;
        public short matkMax;
        public short def;
        public short def2;
        public short mdef;
        public short mdef2;
        public short hit;
        public short flee;
        public short flee2;
        public short crit;
        public short aspd;
        public short aspd2;

        /// Character status (ZC_STATUS).
        /// 00bd 
        /// <stpoint>.W 
        /// <str>.B <need str>.B 
        /// <agi>.B <need agi>.B 
        /// <vit>.B <need vit>.B
        /// <int>.B <need int>.B 
        /// <dex>.B <need dex>.B 
        /// <luk>.B <need luk>.B 
        /// <atk>.W <atk2>.W
        /// <matk min>.W <matk max>.W 
        /// <def>.W <def2>.W
        /// <mdef>.W <mdef2>.W 
        /// <hit>.W
        /// <flee>.W <flee2>.W 
        /// <crit>.W 
        /// <aspd>.W <aspd2>.W
        public void Read(MemoryStreamReader br, int size) {
            stpoint = br.ReadShort();
            str = br.ReadByte();
            needStr = br.ReadByte();
            agi = br.ReadByte();
            needAgi = br.ReadByte();
            vit = br.ReadByte();
            needVit = br.ReadByte();
            inte = br.ReadByte();
            needInte = br.ReadByte();
            dex = br.ReadByte();
            needDex = br.ReadByte();
            luk = br.ReadByte();
            needLuk = br.ReadByte();
            atk = br.ReadShort();
            atk2 = br.ReadShort();
            matkMin = br.ReadShort();
            matkMax = br.ReadShort();
            def = br.ReadShort();
            def2 = br.ReadShort();
            mdef = br.ReadShort();
            mdef2 = br.ReadShort();
            hit = br.ReadShort();
            flee = br.ReadShort();
            flee2 = br.ReadShort();
            crit = br.ReadShort();
            aspd = br.ReadShort();
            aspd2 = br.ReadShort();
        }
    }
}

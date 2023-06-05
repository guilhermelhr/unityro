using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_EMOTION", SIZE)]
    public class EMOTION : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_EMOTION;
        public const int SIZE = 7;
        public PacketHeader Header => HEADER;
        public uint GID;
        public byte type;

        public void Read(MemoryStreamReader fp, int size) {
            this.GID = fp.ReadUInt();
            this.type = (byte)fp.ReadByte();
        }
    }

}
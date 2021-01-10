public partial class ZC {

    /**
     * Server wants to Close the NPC
     */
    [PacketHandler(HEADER, "ZC_CLOSE_SCRIPT", SIZE)]
    public class CLOSE_SCRIPT : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_CLOSE_SCRIPT;
        public const int SIZE = 6;

        public uint NAID;

        public void Read(BinaryReader br, int size) {
            NAID = br.ReadULong();
        }
    }
}
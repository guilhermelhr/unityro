using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_ATTACK_FAILURE_FOR_DISTANCE", SIZE)]
    public class ATTACK_FAILURE_FOR_DISTANCE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_ATTACK_FAILURE_FOR_DISTANCE;
        public const int SIZE = 16;
        public PacketHeader Header => HEADER;

        public int targetAID;
        public short targetXPos;
        public short targetYPos;
        public short xPos;
        public short yPos;
        public short currentAttackRange;

        public void Read(MemoryStreamReader br, int size) {
            targetAID = br.ReadInt();
            targetXPos = br.ReadShort();
            targetYPos = br.ReadShort();
            xPos = br.ReadShort();
            yPos = br.ReadShort();
            currentAttackRange = br.ReadShort();
        }
    }
}

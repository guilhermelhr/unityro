public partial class CZ {

    public class UPGRADE_SKILLLEVEL : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_UPGRADE_SKILLLEVEL;
        public const int SIZE = 4;

        public short SkillID;

        public UPGRADE_SKILLLEVEL() : base(HEADER, SIZE) { }

        public override void Send() {
            Write(SkillID);

            base.Send();
        }
    }
}

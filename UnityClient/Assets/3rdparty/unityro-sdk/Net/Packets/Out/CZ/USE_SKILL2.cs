public partial class CZ {

    public class USE_SKILL2 : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_USE_SKILL2;
        public const int SIZE = 10;

        public short SkillId;
        public short SelectedLevel;
        public int TargetId;

        public USE_SKILL2() : base(HEADER, SIZE) { }

        public override void Send() {
            Write(SelectedLevel);
            Write(SkillId);
            Write(TargetId);

            base.Send();
        }
    }
}

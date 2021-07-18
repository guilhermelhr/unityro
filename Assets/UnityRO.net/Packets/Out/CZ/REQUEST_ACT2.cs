public partial class CZ {

    public class REQUEST_ACT2 : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_REQUEST_ACT2;
        public const int SIZE = 7;

        public uint TargetID;
        public EntityActionType action;

        public REQUEST_ACT2() : base(HEADER, SIZE) { }

        public override void Send() {
            Write(TargetID);
            Write((byte)action);

            base.Send();
        }
    }
}

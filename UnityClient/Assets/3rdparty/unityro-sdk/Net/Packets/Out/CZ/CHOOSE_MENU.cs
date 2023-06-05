public partial class CZ {

    public class CHOOSE_MENU : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_CHOOSE_MENU;
        public const int SIZE = 7;

        public CHOOSE_MENU() : base(HEADER, SIZE) { }

        public uint NAID;
        public byte Index;

        public override void Send() {
            Write(NAID);
            Write(Index);

            base.Send();
        }
    }
}

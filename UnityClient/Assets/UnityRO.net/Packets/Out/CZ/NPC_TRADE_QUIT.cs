public partial class CZ {
    public class NPC_TRADE_QUIT : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_NPC_TRADE_QUIT;
        public const int SIZE = 2;

        public NPC_TRADE_QUIT() : base(HEADER, SIZE) { }

        public override void Send() {
            base.Send();
        }
    }
}

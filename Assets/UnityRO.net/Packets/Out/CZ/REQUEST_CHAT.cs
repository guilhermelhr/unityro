public partial class CZ {

    public class REQUEST_CHAT : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_REQUEST_CHAT;

        public string message;

        public REQUEST_CHAT(string message) : base(HEADER, -1) {
            this.message = $"{Session.CurrentSession.Entity.GetBaseStatus().name} : {message}";
        }

        public override void Send() {
            Write(message, message.Length);

            base.Send();
        }
    }
}

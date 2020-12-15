using System;
using System.IO;

public partial class CZ {

    public class REQUEST_CHAT : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_REQUEST_CHAT;

        public string message;

        public REQUEST_CHAT(string message) : base(HEADER, -1) {
            this.message = $"{Core.Session.Entity.name} : {message}";
        }

        public override bool Send(BinaryWriter writer) {
            base.Send(writer);
            writer.WriteCString(message, Size);
            return true;
        }

        protected override void ComputeSize() {
            Size = 2 + 2 + message.Length + 1;
        }
    }
}

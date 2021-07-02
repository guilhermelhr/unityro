using UnityEngine;

public partial class CZ {

    public class REQUEST_TIME2 : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_REQUEST_TIME2;
        public const int SIZE = 6;

        public REQUEST_TIME2() : base(HEADER, SIZE) { }

        public override void Send() {
            Write((int)Time.realtimeSinceStartup);
            base.Send();
        }
    }
}

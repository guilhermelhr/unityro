
using UnityEngine;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_PLAYERCHAT")]
    public class NOTIFY_PLAYERCHAT : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_PLAYERCHAT;

        public string Message;

        public PacketHeader GetHeader() => HEADER;

        public bool Read(BinaryReader br) {
            Message = br.ReadBinaryString((int)(br.Length - br.Position));
            return true;
        }
    }
}

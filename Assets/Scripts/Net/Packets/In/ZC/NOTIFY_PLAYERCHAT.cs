
using UnityEngine;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_PLAYERCHAT")]
    public class NOTIFY_PLAYERCHAT : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_PLAYERCHAT;

        public string Message;

        public PacketHeader GetHeader() => HEADER;

        public void Read(BinaryReader br, int size) {
            Message = br.ReadBinaryString(size);
        }
    }
}

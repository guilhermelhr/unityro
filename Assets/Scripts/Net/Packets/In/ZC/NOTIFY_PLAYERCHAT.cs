
using UnityEngine;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_PLAYERCHAT")]
    public class NOTIFY_PLAYERCHAT : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_PLAYERCHAT;

        public PacketHeader GetHeader() => HEADER;

        public bool Read(BinaryReader br) {
            var msg = br.ReadBinaryString((int)(br.Length - br.Position));
            Debug.LogWarning($"ZC_NOTIFY_PLAYERCHAT: {msg}");

            return true;
        }
    }
}

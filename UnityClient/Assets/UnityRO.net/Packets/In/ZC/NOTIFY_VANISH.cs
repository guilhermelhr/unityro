using ROIO.Utils;
using System;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_VANISH", SIZE)]
    public class NOTIFY_VANISH : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_VANISH;
        public const int SIZE = 7;
        public PacketHeader Header => HEADER;

        public uint AID;
        public VanishType Type;

        public void Read(MemoryStreamReader br, int size) {
            AID = br.ReadUInt();
            Type = (VanishType) br.ReadByte();
        }

        public enum VanishType : int {
            OUT_OF_SIGHT = 0,
            DIED = 1,
            LOGGED_OUT = 2,
            TELEPORT = 3,
            TRICK_DEAD = 4
        }
    }
}

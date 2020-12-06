using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_PLAYERMOVE", SIZE, PacketHandlerAttribute.PacketDirection.In)]
    public class NOTIFY_PLAYERMOVE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_PLAYERMOVE;
        public const int SIZE = 12;

        public int[] startPosition;
        public int[] endPosition;
        public ulong movementTick;

        public PacketHeader GetHeader() {
            return HEADER;
        }

        public bool Read(byte[] data) {
            var reader = new BinaryReader(data);
            movementTick = reader.ReadULong();
            var moveData = reader.ReadPos2();
            startPosition = new int[2] { moveData[0], moveData[1] };
            endPosition = new int[2] { moveData[2], moveData[3] };

            return true;
        }
    }
}

using ROIO.Utils;
using UnityEngine;

public partial class ZC {

    //Notifies client of a character parameter change.

    [PacketHandler(HEADER, "ZC_PAR_CHANGE", SIZE)]
    public class PAR_CHANGE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_PAR_CHANGE;
        public const int SIZE = 8;
        public PacketHeader Header => HEADER;

        public EntityStatus varID;
        public int value;

        public void Read(MemoryStreamReader br, int size) {
            varID = (EntityStatus)br.ReadUShort();
            value = br.ReadInt();
        }
    }
}

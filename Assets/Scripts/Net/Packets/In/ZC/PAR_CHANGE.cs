using UnityEngine;

public partial class ZC {

    //Notifies client of a character parameter change.

    [PacketHandler(HEADER, "ZC_PAR_CHANGE", SIZE)]
    public class PAR_CHANGE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_PAR_CHANGE;
        public const int SIZE = 8;

        public bool Read(BinaryReader br) {
            var varID = br.ReadUShort();
            var value = br.ReadLong();

            Debug.LogWarning($"ZC_PAR_CHANGE {(EntityStatus)varID}:{value}");

            return true;
        }
    }
}

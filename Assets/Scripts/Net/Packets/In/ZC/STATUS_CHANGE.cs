using System;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_STATUS_CHANGE", SIZE)]
    public class STATUS_CHANGE : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_STATUS_CHANGE;
        public const int SIZE = 5;

        public EntityStatus status;
        public int value;

        //used to update the amount of points necessary to increase that stat
        public bool Read(BinaryReader br) {
            status = (EntityStatus)br.ReadShort();
            value = br.ReadByte();

            return true;
        }
    }
}
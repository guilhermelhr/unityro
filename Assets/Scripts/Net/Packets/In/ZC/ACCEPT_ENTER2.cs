using System;
using System.IO;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_ACCEPT_ENTER2", SIZE, PacketHandlerAttribute.PacketDirection.In)]
    public class ACCEPT_ENTER2 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_ACCEPT_ENTER2;
        public const int SIZE = 13;

        public PacketHeader GetHeader() {
            return HEADER;
        }

        public bool Read(byte[] data) {
            var br = new BinaryReader(data);
            var tick = br.ReadLong();
            var posX = br.ReadByte();
            var posY = br.ReadByte();
            var dir = br.ReadByte();

            br.Seek(2, SeekOrigin.Current);

            var font = br.ReadShort();
            var sex = br.ReadByte();

            return true;
        }
    }
}

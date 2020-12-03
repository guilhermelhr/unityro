using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public partial class HC {

    [PacketHandler(HEADER,
        "HC_NOTIFY_ZONESVR2",
        SIZE,
        PacketHandlerAttribute.PacketDirection.In
    )]
    public class NOTIFY_ZONESVR : InPacket {

        public const PacketHeader HEADER = PacketHeader.HC_NOTIFY_ZONESVR2;
        public const int SIZE = 28;

        public int GID;
        public string Mapname;
        public IPAddress IP;
        public short Port;

        public PacketHeader GetHeader() => HEADER;

        public bool Read(byte[] data) {
            BinaryReader br = new BinaryReader(data);

            GID = br.ReadLong();
            Mapname = br.ReadBinaryString(16);
            IP = new IPAddress(br.ReadUBytes(4));
            Port = br.ReadShort();

            return true;
        }
    }
}

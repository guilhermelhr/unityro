using System.Net;

public partial class HC {

    [PacketHandler(HEADER, "HC_NOTIFY_ZONESVR2", SIZE)]
    public class NOTIFY_ZONESVR2 : InPacket {

        public const PacketHeader HEADER = PacketHeader.HC_NOTIFY_ZONESVR2;
        public const int SIZE = 156;

        public int GID;
        public string Mapname;
        public IPAddress IP;
        public short Port;

        public void Read(BinaryReader br, int size) {
            GID = br.ReadLong();
            Mapname = br.ReadBinaryString(16);
            IP = new IPAddress(br.ReadUBytes(4));
            Port = br.ReadShort();
            br.Seek(128, System.IO.SeekOrigin.Current);
        }
    }
}


using ROIO.Utils;

public interface InPacket : NetworkPacket {
    void Read(MemoryStreamReader br, int size);
    PacketHeader Header { get; }
}

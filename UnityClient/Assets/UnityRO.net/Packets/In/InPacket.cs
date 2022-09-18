
using ROIO.Utils;

public interface InPacket {
    void Read(MemoryStreamReader br, int size);
    PacketHeader Header { get; }
}

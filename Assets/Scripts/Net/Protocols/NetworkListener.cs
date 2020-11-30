
using System.IO;

public interface NetworkListener {

    void OnTcpConnected(BinaryWriter writer);
    void OnUdpConnected();
    void OnDisconnected(NetworkProtocol protocol);

}

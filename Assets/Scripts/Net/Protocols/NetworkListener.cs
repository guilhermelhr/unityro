
using System.IO;

public interface NetworkListener {

    void OnTcpConnected();
    void OnUdpConnected();
    void OnDisconnected(NetworkProtocol protocol);

}

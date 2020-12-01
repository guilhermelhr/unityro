
using System.IO;

public interface NetworkListener {

    void OnTcpConnected();
    void OnDisconnected(NetworkProtocol protocol);

}


public interface NetworkListener {

    void OnTcpConnected(int port);
    void OnUdpConnected();
    void OnDisconnected(NetworkProtocol protocol);

}

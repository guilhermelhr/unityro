using System;
using System.IO;
using System.Net;

public abstract class NetworkProtocol {
    public abstract void Connect();
    public abstract void Connect(string host, int port);
    public abstract void ChangeServer(IPAddress host, int port);
    public abstract void Disconnect();
    public abstract BinaryWriter GetBinaryWriter();
    protected abstract void OnClientConnected();
    protected abstract void OnDataReceived(IAsyncResult result);
}
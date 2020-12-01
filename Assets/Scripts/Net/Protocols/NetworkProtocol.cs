using System;
using System.IO;

public abstract class NetworkProtocol {
    public abstract void Connect();
    public abstract void Connect(string host, int port);
    public abstract void Disconnect();
    public abstract BinaryWriter GetBinaryWriter();
    protected abstract void OnSocketConnected(IAsyncResult result);
    protected abstract void OnDataReceived(IAsyncResult result);
}
using System;
using System.IO;

public abstract class NetworkProtocol {
    public abstract void Connect(int localPort);
    public abstract void Disconnect();
    public abstract BinaryWriter GetBinaryWriter();
    protected abstract void OnSocketConnected(IAsyncResult result);
    protected abstract void OnDataReceived(IAsyncResult result);
}
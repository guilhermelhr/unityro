using System;

public abstract class NetworkProtocol {
    public abstract void Connect(int localPort);
    public abstract void Disconnect();
    protected abstract void OnSocketConnected(IAsyncResult result);
    protected abstract void OnDataReceived(IAsyncResult result);
    protected abstract bool HandleData(byte[] data);
}
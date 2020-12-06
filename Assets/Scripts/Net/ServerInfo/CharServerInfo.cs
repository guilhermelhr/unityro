
using System.Net;

public class CharServerInfo {
    public IPAddress IP { get; set; }
    public int Port { get; set; }
    public string Name { get; set; }
    public int UserCount { get; set; }
    public short State { get; internal set; }
    public ushort Property { get; internal set; }

    public override string ToString() {
        return Name + " (" + UserCount + " Player)";
    }
}

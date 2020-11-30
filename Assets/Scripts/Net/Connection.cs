using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class Connection {

    private TcpClient _client;
    public TcpClient Client => _client;

    private NetworkStream _ns;
    public NetworkStream Stream => _ns;

    private BinaryWriter _bw;
    public BinaryWriter BinaryWriter => _bw;
}
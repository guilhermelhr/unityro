using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[AttributeUsage(AttributeTargets.Class)]
public class PacketHandlerAttribute : Attribute {
    public const int VariableSize = -1;
    public enum PacketDirection {
        None,
        In,
        Out
    }

    public ushort MethodId { get; private set; }
    public string Name { get; private set; }
    public int Size { get; private set; }
    public PacketDirection Direction { get; private set; }

    public PacketHandlerAttribute(ushort methodId, string name, int size, PacketDirection direction) {
        this.MethodId = methodId;
        this.Name = name;
        this.Size = size;
        this.Direction = direction;
    }
}
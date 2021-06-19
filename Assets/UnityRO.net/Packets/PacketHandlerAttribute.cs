using System;

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

    public PacketHandlerAttribute(PacketHeader methodId, string name, int size = VariableSize, PacketDirection direction = PacketDirection.In) {
        this.MethodId = (ushort) methodId;
        this.Name = name;
        this.Size = size;
        this.Direction = direction;
    }
}
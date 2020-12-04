using System.IO;

public abstract class OutPacket {

    private PacketHeader header;
    public int Size;
    private bool isFixed;

    public OutPacket(PacketHeader header, int size) {
        this.header = header;
        this.Size = size;

        isFixed = size > 0;
    }

    public virtual bool Send() {
        return Send(Core.NetworkClient.GetBinaryWriter());
    }

    public virtual bool Send(BinaryWriter writer) {
        writer.Write((ushort) header);

        if (!isFixed) {
            ComputeSize();
            writer.Write((ushort)Size);
        }

        return true;
    }

    protected virtual void ComputeSize() { }
}
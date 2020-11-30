using System.IO;

public abstract class OutPacket {

    private ushort header;
    public int Size;
    private bool isFixed;

    public OutPacket(ushort header, int size) {
        this.header = header;
        this.Size = size;

        isFixed = size > 0;
    }

    public virtual bool Send(BinaryWriter writer) {
        writer.Write(header);

        if (!isFixed) {
            ComputeSize();
            writer.Write((ushort)Size);
        }

        return true;
    }

    protected virtual void ComputeSize() { }
}
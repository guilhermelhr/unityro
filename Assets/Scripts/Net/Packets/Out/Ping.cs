using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Ping : OutPacket {
    private int time;

    public Ping(int ticks) : base(PacketHeader.PING, 6) {
        this.time = ticks;
    }

    public override bool Send(BinaryWriter writer) {
        base.Send(writer);

        writer.Write(time);
        writer.Flush();

        return true;
    }
}

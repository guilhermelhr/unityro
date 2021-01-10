public class Ping : OutPacket {

    private int time;

    public Ping(int ticks) : base(PacketHeader.PING, 6) {
        this.time = ticks;
    }

    public override void Send() {
        Write(time);

        base.Send();
    }
}

﻿
public interface InPacket {
    bool Read(byte[] data);
    PacketHeader GetHeader();
}

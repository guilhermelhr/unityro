﻿using System;
using System.IO;

public partial class CZ {

    public class REQUEST_ACT2 : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_REQUEST_ACT2;
        public const int SIZE = 7;

        public uint TargetGID;
        public byte action;

        public REQUEST_ACT2() : base(HEADER, SIZE) {
        }

        public override bool Send(BinaryWriter writer) {
            base.Send(writer);

            writer.Write(TargetGID);
            writer.Write(action);
            writer.Flush();

            return true;
        }
    }
}

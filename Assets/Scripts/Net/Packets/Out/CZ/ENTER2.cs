using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public partial class CZ {
    public class ENTER2 : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_ENTER2;
        public const int SIZE = 19;

        private int AccountId, CharacterId, LoginId1, clienttime;
        private byte sex;

        public ENTER2(int AccountId, int CharacterId, int LoginId1, int clienttime, byte sex) : base(HEADER, SIZE) {
            this.AccountId = AccountId;
            this.CharacterId = CharacterId;
            this.LoginId1 = LoginId1;
            this.clienttime = clienttime;
            this.sex = sex;
        }

        public override bool Send(BinaryWriter writer) {
            var stream = Core.NetworkClient.CurrentConnection.Stream;

            var bytes = BitConverter.GetBytes((ushort)HEADER)
                .Concat(BitConverter.GetBytes(AccountId))
                .Concat(BitConverter.GetBytes(CharacterId))
                .Concat(BitConverter.GetBytes(LoginId1))
                .Concat(BitConverter.GetBytes(clienttime))
                .Concat(BitConverter.GetBytes(sex));

            stream.Write(bytes.ToArray(), 0, SIZE);
            stream.Flush();

            //base.Send(w);
            //w.Write(AccountId);
            //w.Write(CharacterId);
            //w.Write(LoginId1);
            //w.Write(clienttime);
            //w.Write(sex);
            //w.Flush();

            return true;
        }
    }
}

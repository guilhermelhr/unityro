using System.IO;
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
            base.Send(writer);

            writer.Write(AccountId);
            writer.Write(CharacterId);
            writer.Write(LoginId1);
            writer.Write(clienttime);
            writer.Write(sex);
            writer.Flush();

            Debug.Log(Marshal.SizeOf((ushort)HEADER) + Marshal.SizeOf(AccountId) + Marshal.SizeOf(CharacterId) + Marshal.SizeOf(LoginId1) + Marshal.SizeOf(clienttime) + Marshal.SizeOf(sex));

            return true;
        }
    }
}

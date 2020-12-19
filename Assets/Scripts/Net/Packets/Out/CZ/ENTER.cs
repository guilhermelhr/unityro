using System.IO;

public partial class CZ {
    public class ENTER : OutPacket {

        public const PacketHeader HEADER = PacketHeader.CZ_ENTER2;
        public const int SIZE = 19;

        private int AccountId, CharacterId, LoginId1, clienttime;
        private byte sex;

        public ENTER(int AccountId, int CharacterId, int LoginId1, int clienttime, byte sex) : base(HEADER, SIZE) {
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

            return true;
        }
    }
}

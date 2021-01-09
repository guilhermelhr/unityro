public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_STANDENTRY11")]
    public class NOTIFY_STANDENTRY11 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_STANDENTRY11;

        public EntityData entityData;

        public bool Read(BinaryReader br) {
            entityData = new EntityData();

            var type = (EntityType)br.ReadUByte();

            var GID = br.ReadULong();
            var AID = br.ReadULong();

            var speed = br.ReadShort();
            var opt1 = br.ReadShort();
            var opt2 = br.ReadShort();

            var option = br.ReadLong();

            var job = br.ReadShort();
            var hairStyle = br.ReadShort();
            var weapon = br.ReadShort();
            var shield = br.ReadShort();

            /**
             * might represent emblem/guild_id1/guild_id0
             * rA clif.cpp #1102
             */
            var accessory = br.ReadShort();
            var accessory2 = br.ReadShort();
            var accessory3 = br.ReadShort();

            var hairColor = br.ReadShort();
            var clothColor = br.ReadShort();
            var headDir = br.ReadShort();
            var robe = br.ReadShort();
            var GUID = br.ReadULong();
            var guildEmblem = br.ReadShort();
            var manner = br.ReadShort();

            var opt3 = br.ReadLong();

            var karma = br.ReadUByte();
            var sex = br.ReadUByte();

            var PosDir = br.ReadPos();

            var xSize = br.ReadUByte();
            var ySize = br.ReadUByte();
            var deadSit = br.ReadUByte();

            var level = br.ReadShort();
            var font = br.ReadShort();

            var maxhp = br.ReadLong();
            var hp = br.ReadLong();

            var isBoss = br.ReadUByte() == 1;

            var body = br.ReadShort();
            var name = br.ReadBinaryString(br.Length - br.Position);

            return true;
        }
    }
}

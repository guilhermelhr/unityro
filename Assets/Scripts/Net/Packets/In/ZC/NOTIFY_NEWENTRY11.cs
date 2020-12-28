public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_NEWENTRY11")]
    public class NOTIFY_NEWENTRY11 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_NEWENTRY11;

        public EntityData entityData;

        public bool Read(BinaryReader br) {
            entityData = new EntityData() {
                type = (EntityType)br.ReadUByte(),

                GID = br.ReadULong(),
                AID = br.ReadULong(),

                speed = br.ReadShort(),
                opt1 = br.ReadShort(),
                opt2 = br.ReadShort(),

                option = br.ReadLong(),

                job = br.ReadShort(),
                hairStyle = br.ReadShort(),
                weapon = br.ReadShort(),
                shield = br.ReadShort(),

                /**
                 * might represent emblem/guild_id1/guild_id0
                 * rA clif.cpp #1102
                 */
                headBottom = br.ReadShort(),
                headTop = br.ReadShort(),
                headMid = br.ReadShort(),
                hairColor = br.ReadShort(),
                clothColor = br.ReadShort(),
                headDir = br.ReadShort(),
                robe = br.ReadShort(),

                GUID = br.ReadULong(),

                guildEmblem = br.ReadShort(),
                manner = br.ReadShort(),

                opt3 = br.ReadLong(),

                karma = br.ReadUByte(),
                sex = br.ReadUByte(),

                PosDir = br.ReadPos(),

                xSize = br.ReadUByte(),
                ySize = br.ReadUByte(),
                deadSit = br.ReadUByte(),

                level = br.ReadShort(),
                font = br.ReadShort(),

                maxhp = br.ReadLong(),
                hp = br.ReadLong(),

                isBoss = br.ReadUByte() == 1,

                body = br.ReadShort(),
                name = br.ReadBinaryString(br.Length - br.Position)
            };

            return true;
        }
    }
}

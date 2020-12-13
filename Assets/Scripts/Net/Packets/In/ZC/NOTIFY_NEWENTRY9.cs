public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_NEWENTRY9")]
    public class NOTIFY_NEWENTRY9 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_NEWENTRY9;

        public EntityData entityData;

        public bool Read(BinaryReader br) {
            entityData = new EntityData() {
                type = (EntityType)br.ReadUByte(),

                id = br.ReadULong(),
                GID = br.ReadULong(),

                speed = br.ReadShort(),
                opt1 = br.ReadShort(),
                opt2 = br.ReadShort(),

                optionVal = br.ReadLong(),

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
                Robe = br.ReadShort(),
                GUID = br.ReadULong(),
                GEmblemVer = br.ReadShort(),
                manner = br.ReadShort(),

                opt3 = br.ReadLong(),

                karma = br.ReadUByte(),
                sex = br.ReadUByte(),

                PosDir = br.ReadPos(),

                xSize = br.ReadUByte(),
                ySize = br.ReadUByte(),
                deadSit = br.ReadUByte(),

                clevel = br.ReadShort(),
                font = br.ReadShort(),

                hp = br.ReadLong(),
                maxhp = br.ReadLong(),

                isBoss = br.ReadUByte(),

                body = br.ReadShort(),
                name = br.ReadBinaryString((int)(br.Length - br.Position))
            };

            return true;
        }
    }
}

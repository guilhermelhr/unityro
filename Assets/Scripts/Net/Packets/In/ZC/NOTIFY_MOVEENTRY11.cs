using System;
using System.IO;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_MOVEENTRY11")]
    public class NOTIFY_MOVEENTRY11 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_MOVEENTRY11;

        public EntityData entityData;

        public bool Read(BinaryReader fp) {
            entityData = new EntityData {
                type = (EntityType)fp.ReadUByte(),

                GID = fp.ReadULong(),
                AID = fp.ReadULong(),

                speed = fp.ReadShort(),
                opt1 = fp.ReadShort(),
                opt2 = fp.ReadShort(),

                option = fp.ReadLong(),

                job = fp.ReadShort(),
                hairStyle = fp.ReadShort(),
                weapon = fp.ReadShort(),
                shield = fp.ReadShort(),
                headBottom = fp.ReadShort(),

                moveStartTime = fp.ReadLong(),

                headTop = fp.ReadShort(),
                headMid = fp.ReadShort(),
                hairColor = fp.ReadShort(),
                clothColor = fp.ReadShort(),
                headDir = fp.ReadShort(),
                robe = fp.ReadShort(),

                GUID = fp.ReadULong(),

                guildEmblem = fp.ReadShort(),
                manner = fp.ReadShort(),

                opt3 = fp.ReadLong(),

                karma = fp.ReadUByte(),
                sex = fp.ReadUByte(),
                PosDir = fp.ReadPos2(),

                xSize = fp.ReadUByte(),
                ySize = fp.ReadUByte(),

                level = fp.ReadShort(),
                font = fp.ReadShort(),

                maxhp = fp.ReadLong(),
                hp = fp.ReadLong(),

                isBoss = fp.ReadUByte() == 1,

                body = fp.ReadShort(),
                name = fp.ReadBinaryString(fp.Length - fp.Position)
            };

            return true;
        }
    }
}

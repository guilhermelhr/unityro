using System;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_MOVEENTRY9")]
    public class NOTIFY_MOVEENTRY9 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_MOVEENTRY9;

        public EntityData entityData;

        public bool Read(BinaryReader fp) {

            var objecttype = (EntityType)fp.ReadUByte();
            var GID = fp.ReadULong();
            var AID = fp.ReadULong();
            var speed = fp.ReadShort();
            var bodyState = fp.ReadShort();
            var healthState = fp.ReadShort();
            var effectState = fp.ReadLong();
            var job = fp.ReadShort();
            var head = fp.ReadShort();
            var weapon = fp.ReadLong();
            var shield = fp.ReadShort();
            var moveStartTime = fp.ReadULong();
            var accessory2 = fp.ReadShort();
            var accessory3 = fp.ReadShort();
            var headpalette = fp.ReadShort();
            var bodypalette = fp.ReadShort();
            var headDir = fp.ReadShort();
            var Robe = fp.ReadShort();
            var GUID = fp.ReadULong();
            var GEmblemVer = fp.ReadShort();
            var honor = fp.ReadShort();
            var virtue = fp.ReadLong();
            var isPKModeON = fp.ReadUByte();
            var sex = fp.ReadUByte();
            var MoveData = fp.ReadPos2();
            var xSize = fp.ReadUByte();
            var ySize = fp.ReadUByte();
            var clevel = fp.ReadShort();
            var font = fp.ReadShort();
            var hp = fp.ReadLong();
            var maxhp = fp.ReadLong();
            var isBoss = fp.ReadUByte();
            var body = fp.ReadShort();
            var name = fp.ReadBinaryString(24);

            entityData = new EntityData() {
                type = objecttype,
                GID = GID,
                AID = AID,
                speed = speed,
                opt1 = bodyState,
                opt2 = healthState,
                optionVal = effectState,
                job = job,
                hairStyle = head,
                weapon = weapon,
                shield = shield,
                headTop = accessory2,
                headMid = accessory3,
                hairColor = headpalette,
                clothColor = bodypalette,
                headDir = headDir,
                Robe = Robe,
                GUID = GUID,
                GEmblemVer = GEmblemVer,
                manner = honor,
                opt3 = virtue,
                karma = isPKModeON,
                sex = sex,
                PosDir = MoveData,
                xSize = xSize,
                ySize = ySize,
                clevel = clevel,
                font = font,
                hp = hp,
                maxhp = maxhp,
                isBoss = isBoss,
                body = body,
                name = name,
                moveStartTime = moveStartTime
            };

            return true;
        }
    }
}

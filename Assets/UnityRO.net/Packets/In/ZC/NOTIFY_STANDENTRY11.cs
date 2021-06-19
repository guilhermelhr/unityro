using ROIO.Utils;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_STANDENTRY11")]
    public class NOTIFY_STANDENTRY11 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_STANDENTRY11;

        public EntityData entityData;

        public void Read(MemoryStreamReader br, int size) {
            entityData = new EntityData();

            entityData.objecttype = (EntityType)br.ReadByte();

            entityData.AID = br.ReadUInt();
            entityData.GID = br.ReadUInt();

            entityData.speed = br.ReadShort();
            entityData.bodyState = br.ReadShort();
            entityData.healthState = br.ReadShort();

            entityData.effectState = br.ReadInt();

            entityData.job = br.ReadShort();

            entityData.head = br.ReadUShort();

            entityData.weapon = br.ReadUInt();
            entityData.shield = br.ReadUInt();

            /**
             * might represent emblem/guild_id1/guild_id0
             * rA clif.cpp #1102
             */
            entityData.accessory = br.ReadUShort();
            entityData.accessory2 = br.ReadUShort();
            entityData.accessory3 = br.ReadUShort();

            entityData.headpalette = br.ReadShort();
            entityData.bodypalette = br.ReadShort();
            entityData.headDir = br.ReadShort();

            entityData.robe = br.ReadUShort();

            entityData.GUID = br.ReadUInt();

            entityData.GEmblemVer = br.ReadShort();
            entityData.honor = br.ReadShort();

            entityData.virtue = br.ReadInt();

            entityData.isPKModeON = br.ReadByte();
            entityData.sex = br.ReadByte();

            entityData.PosDir = br.ReadPos();

            entityData.xSize = br.ReadByte();
            entityData.ySize = br.ReadByte();
            entityData.state = br.ReadByte();

            entityData.clevel = br.ReadShort();
            entityData.font = br.ReadShort();

            entityData.maxHP = br.ReadInt();
            entityData.HP = br.ReadInt();

            entityData.isBoss = br.ReadByte();

            entityData.body = br.ReadUShort();
            entityData.name = br.ReadBinaryString(24);
        }
    }
}

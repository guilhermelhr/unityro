using System;
using System.IO;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_MOVEENTRY11")]
    public class NOTIFY_MOVEENTRY11 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_MOVEENTRY11;

        public EntityData entityData;

        public void Read(BinaryReader br, int size) {
            entityData = new EntityData();

            entityData.objecttype = (EntityType)br.ReadUByte();

            entityData.AID = br.ReadULong();
            entityData.GID = br.ReadULong();

            entityData.speed = br.ReadShort();
            entityData.bodyState = br.ReadShort();
            entityData.healthState = br.ReadShort();

            entityData.effectState = br.ReadLong();

            entityData.job = br.ReadShort();

            entityData.head = br.ReadUShort();

            entityData.weapon = br.ReadULong();
            entityData.accessory = br.ReadUShort();
            entityData.moveStartTime = br.ReadULong();
            entityData.shield = br.ReadULong();

            /**
             * might represent emblem/guild_id1/guild_id0
             * rA clif.cpp #1102
             */
            entityData.accessory2 = br.ReadUShort();
            entityData.accessory3 = br.ReadUShort();

            entityData.headpalette = br.ReadShort();
            entityData.bodypalette = br.ReadShort();
            entityData.headDir = br.ReadShort();

            entityData.robe = br.ReadUShort();

            entityData.GUID = br.ReadULong();

            entityData.GEmblemVer = br.ReadShort();
            entityData.honor = br.ReadShort();

            entityData.virtue = br.ReadLong();

            entityData.isPKModeON = br.ReadUByte();
            entityData.sex = br.ReadUByte();

            entityData.PosDir = br.ReadPos2();

            entityData.xSize = br.ReadUByte();
            entityData.ySize = br.ReadUByte();
            entityData.state = br.ReadUByte();

            entityData.clevel = br.ReadShort();
            entityData.font = br.ReadShort();

            entityData.maxHP = br.ReadLong();
            entityData.HP = br.ReadLong();

            entityData.isBoss = br.ReadUByte();

            entityData.body = br.ReadUShort();
            entityData.name = br.ReadBinaryString(24);
        }
    }
}

using ROIO.Utils;
using System;
using System.IO;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_NOTIFY_MOVEENTRY11")]
    public class NOTIFY_MOVEENTRY11 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_NOTIFY_MOVEENTRY11;

        public EntitySpawnData entityData;

        public void Read(MemoryStreamReader br, int size) {
            entityData = new EntitySpawnData();

            entityData.objecttype = br.ReadByte().GetEntityType();

            entityData.AID = br.ReadUInt();
            entityData.GID = br.ReadUInt();

            entityData.speed = br.ReadShort();
            entityData.bodyState = br.ReadShort();
            entityData.healthState = br.ReadShort();

            entityData.effectState = br.ReadInt();

            entityData.job = br.ReadShort();

            entityData.head = br.ReadUShort();

            entityData.Weapon = br.ReadUInt();
            entityData.Accessory = br.ReadUShort();
            entityData.moveStartTime = br.ReadUInt();
            entityData.Shield = br.ReadUInt();

            /**
             * might represent emblem/guild_id1/guild_id0
             * rA clif.cpp #1102
             */
            entityData.Accessory2 = br.ReadUShort();
            entityData.Accessory3 = br.ReadUShort();

            entityData.HairColor = br.ReadShort();
            entityData.ClothesColor = br.ReadShort();
            entityData.headDir = br.ReadShort();

            entityData.Robe = br.ReadUShort();

            entityData.GUID = br.ReadUInt();

            entityData.GEmblemVer = br.ReadShort();
            entityData.honor = br.ReadShort();

            entityData.virtue = br.ReadInt();

            entityData.isPKModeON = (byte)br.ReadByte();
            entityData.sex = (byte)br.ReadByte();

            entityData.PosDir = br.ReadPos2();

            entityData.xSize = (byte)br.ReadByte();
            entityData.ySize = (byte)br.ReadByte();

            // This needs checking... struct packet_unit_walking does not seem to have state
            //entityData.state = (byte)br.ReadByte();
            entityData.state = EntitySpawnData.EntitySpawnState.Stand;

            entityData.clevel = br.ReadShort();
            entityData.font = br.ReadShort();

            entityData.MaxHP = br.ReadInt();
            entityData.HP = br.ReadInt();

            entityData.isBoss = (byte)br.ReadByte();

            entityData.body = br.ReadUShort();
            entityData.name = br.ReadBinaryString(24);
        }
    }
}

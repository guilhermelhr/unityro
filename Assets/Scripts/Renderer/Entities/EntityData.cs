using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

unsafe public struct packet_idle_unit {
	public short PacketType;
	public short PacketLength;

	public EntityType objecttype;

	public uint AID;
	public uint GID;

	public short speed;
	public short bodyState;
	public short healthState;

	public int effectState;

	public short job;

	public ushort head;

	public uint weapon;
	public uint shield;

	public ushort accessory;
	public ushort accessory2;
	public ushort accessory3;

	public short headpalette;
	public short bodypalette;
	public short headDir;

	public ushort robe;

	public uint GUID;

	public short GEmblemVer;
	public short honor;

	public int virtue;

	public byte isPKModeON;
	public byte sex;

	public int[] PosDir;

	public byte xSize;
	public byte ySize;
	public byte state;

	public short clevel;
	public short font;

	public int maxHP;
	public int HP;

	public byte isBoss;

	public ushort body;
	/* Might be earlier, this is when the named item bug began */
	public string name;
}

public class EntityData {
	public EntityType objecttype;

	public uint AID;
	public uint GID;

	public short speed;
	public short bodyState;
	public short healthState;

	public int effectState;

	public short job;

	public ushort head;

	public uint weapon;
	public uint shield;

	public ushort accessory;
	public ushort accessory2;
	public ushort accessory3;

	public short headpalette;
	public short bodypalette;
	public short headDir;

	public ushort robe;

	public uint GUID;

	public short GEmblemVer;
	public short honor;

	public int virtue;

	public byte isPKModeON;
	public byte sex;

	public int[] PosDir;

	public byte xSize;
	public byte ySize;
	public byte state;

	public short clevel;
	public short font;

	public int maxHP;
	public int HP;

	public byte isBoss;

	public ushort body;
	/* Might be earlier, this is when the named item bug began */
	public string name;
    public uint moveStartTime;
}


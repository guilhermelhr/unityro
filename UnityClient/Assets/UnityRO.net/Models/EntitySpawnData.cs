public class EntitySpawnData {
	public EntityType objecttype;

	public uint AID;
	public uint GID;
	public uint GuildID;

	public short speed;
	public short bodyState;
	public short healthState;

	public int effectState;

	public short job;

	public ushort head;

	public uint Weapon;
	public uint Shield;
	public ushort Accessory;
	public ushort Accessory2;
	public ushort Accessory3;
	public ushort Robe;

	public short HairColor;
	public short ClothesColor;

	public short headDir;

	public short GEmblemVer;
	public short honor;

	public int virtue;

	public byte isPKModeON;
	public byte sex;

	public int[] PosDir;

	public byte xSize;
	public byte ySize;
	public EntitySpawnState state;

	public short clevel;
	public short font;

	public int MaxHP;
	public int HP;

	public byte isBoss;

	public ushort body;
	/* Might be earlier, this is when the named item bug began */
	public string name;
    public uint moveStartTime;

    public enum EntitySpawnState : byte {
        Stand = 0,
        Dead = 1,
        Sit = 2
    }
}


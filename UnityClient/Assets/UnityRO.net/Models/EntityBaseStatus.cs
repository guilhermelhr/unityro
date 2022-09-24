using System;

[Serializable]
public class EntityBaseStatus {
	public ulong char_id;
	public ulong account_id;
	public ulong partner_id;
	public ulong father;
	public ulong mother;
	public ulong child;
    public short walkSpeed;
    public uint base_exp, job_exp;
	public int zeny;
	public int attackRange;
	public ushort attackSpeed;

	public short jobId; ///< Player's JobID
	public uint StatusPoints, SkillPoints;
	public long hp, max_hp, sp, max_sp;
	public uint option;
	public short manner; // Defines how many minutes a char will be muted, each negative point is equivalent to a minute.
	public byte karma;
	public short hair, hair_color, clothes_color, body;
	public int party_id, guild_id, pet_id, hom_id, mer_id, ele_id, clan_id;
	public int fame;

	// Mercenary Guilds Rank
	public int arch_faith, arch_calls;
	public int spear_faith, spear_calls;
	public int sword_faith, sword_calls;

	public string name;
	public uint base_level, job_level;
	public ushort str, agi, vit, int_, dex, luk;
	public byte slot, sex;

	public ulong mapip;
	public ushort mapport;
    public int next_base_exp, next_job_exp;

    //	struct point last_point,save_point,memo_point[MAX_MEMOPOINTS];
    //	struct s_skill skill[MAX_SKILL];

    //	struct s_friend friends[MAX_FRIENDS]; //New friend system [Skotlex]
    //#ifdef HOTKEY_SAVING
    //	struct hotkey hotkeys[MAX_HOTKEYS];
    //#endif
    //	bool show_equip, allow_party;
    //	short rename;

    //	time_t delete_date;
    //	time_t unban_time;

    //	// Char server addon system
    //	uint character_moves;

    //	unsigned char font;

    //	bool cashshop_sent; // Whether the player has received the CashShop list

    //	ulong uniqueitem_counter;

    //	unsigned char hotkey_rowshift;
    //	unsigned long title_id;
}
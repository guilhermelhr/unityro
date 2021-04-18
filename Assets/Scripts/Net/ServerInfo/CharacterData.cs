using System.IO;

public class CharacterData {
    public int GID;
    public int Exp;
    public int Zeny;
    public int JobExp;
    public int JobLevel;
    public int BodyState;
    public int HealthState;
    public int Option;
    public int Karma;
    public int Manner;
    public short StatusPoint;
    public int HP;
    public int MaxHP;
    public short SP;
    public short MaxSP;
    public short Speed;
    public short Job;
    public short Hair;
    public short Weapon;
    public short BaseLevel;
    public short SkillPoint;
    public short Accessory;
    public short Shield;
    public short Accessory2;
    public short Accessory3;
    public short HairColor;
    public short ClothesColor;
    public string Name;
    public byte Str;
    public byte Agi;
    public byte Vit;
    public byte Int;
    public byte Dex;
    public byte Luk;
    public short Slot;
    public short Rename;
    public string MapName;
    public int DeleteDate;
    public int Robe;
    public int Body;
    public int Moves;
    public int AddOns;
    public int Sex;

    public static CharacterData parse(BinaryReader br) {
        CharacterData cd = new CharacterData();

        cd.GID = br.ReadLong();
        cd.Exp = br.ReadLong();
        br.Seek(4, SeekOrigin.Current);
        cd.Zeny = br.ReadLong();
        cd.JobExp = br.ReadLong();
        br.Seek(4, SeekOrigin.Current);
        cd.JobLevel = br.ReadLong();
        cd.BodyState = br.ReadLong();
        cd.HealthState = br.ReadLong();
        cd.Option = br.ReadLong();
        cd.Karma = br.ReadLong();
        cd.Manner = br.ReadLong();

        cd.StatusPoint = br.ReadShort();

        cd.HP = br.ReadLong();
        cd.MaxHP = br.ReadLong();

        cd.SP = br.ReadShort();
        cd.MaxSP = br.ReadShort();
        cd.Speed = br.ReadShort();
        cd.Job = br.ReadShort();
        cd.Hair = br.ReadShort();
        cd.Body = br.ReadShort();
        cd.Weapon = br.ReadShort();
        cd.BaseLevel = br.ReadShort();
        cd.SkillPoint = br.ReadShort();
        cd.Accessory = br.ReadShort();
        cd.Shield = br.ReadShort();
        cd.Accessory2 = br.ReadShort();
        cd.Accessory3 = br.ReadShort();
        cd.HairColor = br.ReadShort();
        cd.ClothesColor = br.ReadShort();

        cd.Name = br.ReadBinaryString(24);

        cd.Str = br.ReadUByte();
        cd.Agi = br.ReadUByte();
        cd.Vit = br.ReadUByte();
        cd.Int = br.ReadUByte();
        cd.Dex = br.ReadUByte();
        cd.Luk = br.ReadUByte();

        cd.Slot = br.ReadShort();
        cd.Rename = br.ReadShort();

        cd.MapName = br.ReadBinaryString(16);

        cd.DeleteDate = br.ReadLong();
        cd.Robe = br.ReadLong();
        cd.Moves = br.ReadLong();
        cd.AddOns = br.ReadLong();

        cd.Sex = br.ReadByte();

        return cd;
    }
}
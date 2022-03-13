using ROIO.Utils;
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
    public short BaseLevel;
    public short SkillPoint;

    public short Weapon;
    public short Shield;
    public short HeadBottom;
    public short HeadTop;
    public short HeadMid;
    public int Garment;

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

    public int Body;
    public int Moves;
    public int AddOns;
    public int Sex;

    public static CharacterData parse(MemoryStreamReader br) {
        CharacterData cd = new CharacterData();

        cd.GID = br.ReadInt();
        cd.Exp = br.ReadInt();
        br.Seek(4, SeekOrigin.Current);
        cd.Zeny = br.ReadInt();
        cd.JobExp = br.ReadInt();
        br.Seek(4, SeekOrigin.Current);
        cd.JobLevel = br.ReadInt();
        cd.BodyState = br.ReadInt();
        cd.HealthState = br.ReadInt();
        cd.Option = br.ReadInt();
        cd.Karma = br.ReadInt();
        cd.Manner = br.ReadInt();

        cd.StatusPoint = br.ReadShort();

        cd.HP = br.ReadInt();
        cd.MaxHP = br.ReadInt();

        cd.SP = br.ReadShort();
        cd.MaxSP = br.ReadShort();
        cd.Speed = br.ReadShort();
        cd.Job = br.ReadShort();
        cd.Hair = br.ReadShort();
        cd.Body = br.ReadShort();
        cd.Weapon = br.ReadShort();
        cd.BaseLevel = br.ReadShort();
        cd.SkillPoint = br.ReadShort();
        cd.HeadBottom = br.ReadShort();
        cd.Shield = br.ReadShort();
        cd.HeadTop = br.ReadShort();
        cd.HeadMid = br.ReadShort();
        cd.HairColor = br.ReadShort();
        cd.ClothesColor = br.ReadShort();

        cd.Name = br.ReadBinaryString(24);

        cd.Str = (byte)br.ReadByte();
        cd.Agi = (byte)br.ReadByte();
        cd.Vit = (byte)br.ReadByte();
        cd.Int = (byte)br.ReadByte();
        cd.Dex = (byte)br.ReadByte();
        cd.Luk = (byte)br.ReadByte();

        cd.Slot = br.ReadShort();
        cd.Rename = br.ReadShort();

        cd.MapName = br.ReadBinaryString(16);

        cd.DeleteDate = br.ReadInt();
        cd.Garment = br.ReadInt();
        cd.Moves = br.ReadInt();
        cd.AddOns = br.ReadInt();

        cd.Sex = br.ReadByte();

        return cd;
    }
}
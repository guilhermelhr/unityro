using ROIO.Utils;
using System.IO;

public class CharacterData {
    public const int BLOCK_SIZE = 175;

    public int GID;
    public long Exp;
    public int Money;
    public long JobExp;
    public int JobLevel;
    public int BodyState;
    public int HealthState;
    public int EffectState;
    public int Virtue;
    public int Honor;
    public short JobPoint;

    public long HP;
    public long MaxHP;
    public long SP;
    public long MaxSP;

    public short Speed;
    public short Job;
    public short Head;
    public short Level;
    public short SPPoint;

    public short Weapon;
    public short Shield;
    public short Accessory;
    public short Accessory2;
    public short Accessory3;
    public int chr_slot_changeCnt;

    public short HeadPalette;
    public short BodyPalette;

    public string Name;
    public byte Str;
    public byte Agi;
    public byte Vit;
    public byte Int;
    public byte Dex;
    public byte Luk;

    public short CharNum;
    public short bIsChangedCharName;
    public string MapName;
    public int DelRevDate;

    public int Body;
    public int chr_name_changeCnt;
    public int AddOns;
    public int Sex;

    public static CharacterData parse(MemoryStreamReader br) {
        CharacterData cd = new CharacterData();

        cd.GID = br.ReadInt();
        cd.Exp = br.ReadLong();
        //br.Seek(4, SeekOrigin.Current);
        cd.Money = br.ReadInt();
        cd.JobExp = br.ReadLong();
        //br.Seek(4, SeekOrigin.Current);
        cd.JobLevel = br.ReadInt();
        cd.BodyState = br.ReadInt();
        cd.HealthState = br.ReadInt();
        cd.EffectState = br.ReadInt();
        cd.Virtue = br.ReadInt();
        cd.Honor = br.ReadInt();

        cd.JobPoint = br.ReadShort();

        cd.HP = br.ReadLong();
        cd.MaxHP = br.ReadLong();
        cd.SP = br.ReadLong();
        cd.MaxSP = br.ReadLong();

        cd.Speed = br.ReadShort();
        cd.Job = br.ReadShort();
        cd.Head = br.ReadShort();
        cd.Body = br.ReadShort();
        cd.Weapon = br.ReadShort();
        cd.Level = br.ReadShort();
        cd.SPPoint = br.ReadShort();
        cd.Accessory = br.ReadShort();
        cd.Shield = br.ReadShort();
        cd.Accessory2 = br.ReadShort();
        cd.Accessory3 = br.ReadShort();
        cd.HeadPalette = br.ReadShort();
        cd.BodyPalette = br.ReadShort();

        cd.Name = br.ReadBinaryString(24);

        cd.Str = (byte)br.ReadByte();
        cd.Agi = (byte)br.ReadByte();
        cd.Vit = (byte)br.ReadByte();
        cd.Int = (byte)br.ReadByte();
        cd.Dex = (byte)br.ReadByte();
        cd.Luk = (byte) br.ReadByte();
        cd.CharNum = (byte)br.ReadByte();
        var HairColor = (byte) br.ReadByte();
        cd.bIsChangedCharName = br.ReadShort();

        cd.MapName = br.ReadBinaryString(16);

        cd.DelRevDate = br.ReadInt();
        var robePalette = br.ReadInt();
        cd.chr_slot_changeCnt = br.ReadInt();
        cd.chr_name_changeCnt = br.ReadInt();

        cd.Sex = br.ReadByte();

        return cd;
    }
}
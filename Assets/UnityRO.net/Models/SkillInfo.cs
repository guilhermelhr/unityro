public class SkillInfo {
    public short SkillID;
    public int SkillType;
    public short Level;
    public short SpCost;
    public short AttackRange;
    public string SkillName;
    public bool CanUpgrade;
}

public enum SkillTargetType {
    Enemy = 1,
    Place = 2,
    Self = 4,
    Friend = 16,
    Trap = 32,
    Target = 1 | 2 | 16 | 32,
    Pet = 64
}
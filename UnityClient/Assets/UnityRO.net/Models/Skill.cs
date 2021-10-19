using System.Collections.Generic;

public class Skill {
    public short SkillId;
    public string SkillName;
    public int MaxLv;
    public bool CanSelectLevel;
    public List<int> SpAmount;
    public List<int> AttackRange;
    public List<int[]> SkillScale;
    public Dictionary<int, int> NeededSkillList;
    public Dictionary<short, Dictionary<int, int>> SexNeededSkillList;
    public string SkillTag;
}

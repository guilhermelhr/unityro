using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Skill {
    public short SkillId;
    public string SkillName;
    public int MaxLv;
    public bool bSeparateLv;
    public List<int> SpAmount;
    public List<int> AttackRange;
    public List<int[]> SkillScale;
    public Dictionary<int, int> NeededSkillList;
    public Dictionary<short, Dictionary<int, int>> SexNeededSkillList;
}

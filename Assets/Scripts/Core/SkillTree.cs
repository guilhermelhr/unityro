using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SkillTree {

    public List<SkillInfo> SkillInfos = new List<SkillInfo>();
    public List<Dictionary<int, int>> Tree = new List<Dictionary<int, int>>();

    public void Init(short job, List<SkillInfo> skills) {
        SkillInfos.AddRange(skills);
        var tree = SkillTable.GetSkillTree(job);
        if (tree != null) {
            Tree = tree;
        }
    }
}
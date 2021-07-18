using System.Collections.Generic;

public class SkillTree {

    public List<SkillInfo> OwnedSkillsInfos = new List<SkillInfo>();
    public List<Dictionary<int, SkillInfo>> OwnedTree = new List<Dictionary<int, SkillInfo>>();
    public Dictionary<int, Dictionary<int, Skill>> ClassTree = new Dictionary<int, Dictionary<int, Skill>>();

    public void Init(short job, List<SkillInfo> skills) {
        OwnedSkillsInfos.AddRange(skills);
        var tree = SkillTable.GetSkillTree(job);

        if(tree != null) {
            //OwnedTree = tree
            //    .Select(j => j
            //        .Select(tr => new KeyValuePair<int, SkillInfo>(tr.Key, OwnedSkillsInfos.Find(a => a.SkillID == tr.Value))
            //    ).ToDictionary(t => t.Key, t => t.Value))
            //    .ToList();
            ClassTree = tree;
        }

        MapUiController.Instance.SkillWindow.UpdateSkills();
    }
}
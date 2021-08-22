using System;
using System.Collections.Generic;

public class SkillTree {

    public List<SkillInfo> OwnedSkillsInfos = new List<SkillInfo>();
    public Dictionary<int, Dictionary<int, Skill>> ClassTree = new Dictionary<int, Dictionary<int, Skill>>();

    public void Init(short job, List<SkillInfo> skills) {
        OwnedSkillsInfos.Clear();
        OwnedSkillsInfos.AddRange(skills);
        var tree = SkillTable.GetSkillTree(job);

        if(tree != null) {
            ClassTree = tree;
        }

        MapUiController.Instance.SkillWindow.UpdateSkills();
    }

    public void UpdateSkill(SkillInfo skill) {
        var info = OwnedSkillsInfos.Find(it => it.SkillID == skill.SkillID);
        if (info == null) {
            OwnedSkillsInfos.Add(info);
        } else {
            info.Level = skill.Level;
            info.AttackRange = skill.AttackRange;
            info.CanUpgrade = skill.CanUpgrade;
            info.SpCost = skill.SpCost;
        }

        MapUiController.Instance.SkillWindow.UpdateSkills();
    }

    public bool HasSkill(short skillId, short level) {
        return OwnedSkillsInfos.Find(t => t.SkillID == skillId && t.Level == level) != null;
    }
}
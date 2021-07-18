using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;

public class SkillTable {

    public static Dictionary<short, Skill> Skills = new Dictionary<short, Skill>();

    public static Dictionary<short, Dictionary<int, Skill>> SkillTree = new Dictionary<short, Dictionary<int, Skill>>();

    private static Func<TablePair, KeyValuePair<int, int>> TablePairToKeyValueTransform = t => new KeyValuePair<int, int>(int.Parse(t.Key.ToString()), int.Parse(t.Value.ToString()));

    public static void LoadSkillData() {
        
        var skillTree = LuaInterface.GetTable("SKILL_TREEVIEW_FOR_JOB");
        var skillInfo = LuaInterface.GetTable("SKILL_INFO_LIST");

        foreach(var key in skillInfo.Keys) {
            var skid = short.Parse(key.ToString());
            var dict = skillInfo[key] as Table;
            var tree = dict.Pairs.ToList();
            var skill = new Skill {
                SkillId = skid,
                SkillTag = tree.Last().Value.ToString().Replace("\"",""),
                SkillName = dict["SkillName"].ToString(),
                MaxLv = int.Parse(dict["MaxLv"].ToString()),
                SpAmount = (dict["SpAmount"] as Table).Pairs.Select(t => int.Parse(t.Value.ToString())).ToList(),
                bSeparateLv = dict["bSeperateLv"] != null ? bool.Parse(dict["bSeperateLv"].ToString()) : true,
                AttackRange = (dict["AttackRange"] as Table).Pairs.Select(t => int.Parse(t.Value.ToString())).ToList()
            };

            var skillScale = dict["SkillScale"] as Table;
            skill.SkillScale = new List<int[]>();
            if(skillScale != null) {
                foreach(var pair in skillScale.Pairs.ToList()) {
                    var lvl = (pair.Value.Table);
                    var s = new int[] { int.Parse(lvl["x"].ToString()), int.Parse(lvl["y"].ToString()) };
                    skill.SkillScale.Add(s);
                }
            }

            var needed = dict["_NeedSkillList"] as Table;
            if(needed != null) {
                skill.NeededSkillList = needed
                    .Pairs
                    .ToList()
                    .ToDictionary(
                        t => int.Parse(t.Value.Table.Values.ToList()[0].ToString()),
                        t => t.Value.Table.Values.ToList().Count > 1 ? int.Parse(t.Value.Table.Values.ToList()[1].ToString()) : 1
                    );
            }

            var sexNeeded = dict["NeedSkillList"] as Table;
            if(sexNeeded != null) {
                var _dict = new Dictionary<short, Dictionary<int, int>>();
                foreach(var pair in sexNeeded.Pairs) {
                    var job = short.Parse(pair.Key.ToString());
                    var skills = pair.Value.Table.Pairs
                    .ToList()
                    .ToDictionary(
                        t => int.Parse(t.Value.Table.Values.ToList()[0].ToString()),
                        t => t.Value.Table.Values.ToList().Count > 1 ? int.Parse(t.Value.Table.Values.ToList()[1].ToString()) : 1
                    );
                    _dict.Add(job, skills);
                }
                skill.SexNeededSkillList = _dict;
            }

            Skills.Add(skid, skill);
        }

        foreach(var key in skillTree.Keys) {
            var job = short.Parse(key.ToString());
            var dict = skillTree[key] as Table;
            var tree = dict
                .Pairs
                .ToList()
                .Select(TablePairToKeyValueTransform)
                .ToDictionary(it => it.Key, it => Skills[(short)it.Value]);
            SkillTree.Add(job, tree);
        }
    }

    public static Dictionary<int, Dictionary<int, Skill>> GetSkillTree(short jobID) {
        var tree = GetInheritance(jobID);
        tree.Sort((a, b) => a.CompareTo(b));
        var completeTree = new Dictionary<int, Dictionary<int, Skill>>();
        foreach(var job in tree) {
            if (!SkillTree.ContainsKey(job)) {
                continue;
            }

            completeTree.Add(job, SkillTree[job]);
        }

        return completeTree;
    }

    private static List<short> GetInheritance(short jobID) {
        var tree = new List<object> {
            jobID
        };

        var thirdJob = LuaInterface.GetTable("JOB_INHERIT_LIST2")[jobID];
        if (thirdJob != null) {
            tree.Add(thirdJob);
        }

        var secondJob = LuaInterface.GetTable("JOB_INHERIT_LIST")[thirdJob ?? jobID];
        if (secondJob != null) {
            tree.Add(secondJob);
        }

        var firstJob = LuaInterface.GetTable("JOB_INHERIT_LIST")[secondJob ?? jobID];
        if (firstJob != null) {
            tree.Add(firstJob);
        }

        tree.Add(0); //novice

        return tree.Select(t => short.Parse(t.ToString())).Distinct().ToList();
    }
}

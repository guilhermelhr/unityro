using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;

public class SkillTable {

    public static Dictionary<short, Skill> Skills = new Dictionary<short, Skill>();

    public static Dictionary<Job, Dictionary<int, int>> SkillTree = new Dictionary<Job, Dictionary<int, int>>();

    private static Func<TablePair, KeyValuePair<int, int>> TablePairToKeyValueTransform = t => new KeyValuePair<int, int>(int.Parse(t.Key.ToString()), int.Parse(t.Value.ToString()));

    public static void LoadSkillData() {
        Script script = new Script();
        script.Globals["rag"] = "";
        script.DoStream(FileManager.ReadSync("data/luafiles514/lua files/skillinfoz/jobinheritlist.lub"));
        script.DoStream(FileManager.ReadSync("data/luafiles514/lua files/skillinfoz/skillid.lub"));
        script.DoStream(FileManager.ReadSync("data/luafiles514/lua files/skillinfoz/skilldescript.lub"));
        script.DoStream(FileManager.ReadSync("data/luafiles514/lua files/skillinfoz/skillinfolist.lub"));
        script.DoStream(FileManager.ReadSync("data/luafiles514/lua files/skillinfoz/skilltreeview.lub"));

        var skillTree = script.Globals["SKILL_TREEVIEW_FOR_JOB"] as Table;
        var skillInfo = script.Globals["SKILL_INFO_LIST"] as Table;

        foreach(var key in skillTree.Keys) {
            var job = (Job)int.Parse(key.ToString());
            var dict = skillTree[key] as Table;
            var tree = dict.Pairs.ToList().Select(TablePairToKeyValueTransform).ToDictionary(it => it.Key, it => it.Value);
            SkillTree.Add(job, tree);
        }

        foreach(var key in skillInfo.Keys) {
            var skid = short.Parse(key.ToString());
            var dict = skillInfo[key] as Table;
            var tree = dict.Pairs.ToList();
            var skill = new Skill {
                SkillId = skid,
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
                var _dict = new Dictionary<Job, Dictionary<int, int>>();
                foreach(var pair in sexNeeded.Pairs) {
                    var job = (Job)int.Parse(pair.Key.ToString());
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
    }
}

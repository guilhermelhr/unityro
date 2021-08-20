using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillWindowController : MonoBehaviour, ISkillWindowController {

    private class SkillAllocation {
        public short skillID;
        public int level;
    }

    [SerializeField] private GridLayoutGroup GridLayout;
    [SerializeField] private ToggleGroup tabLayout;
    [SerializeField] private Toggle tabPrefab;
    [SerializeField] private TextMeshProUGUI skillPointsText;

    [SerializeField] private Material unownedSkillShader;
    [SerializeField] private Shader ownedSkillShader;

    private List<UISkill> UISkillArray;
    private List<Toggle> tabs = new List<Toggle>();
    private int AvailableSkillPoints;
    private List<KeyValuePair<Skill, int>> NeededSkills = new List<KeyValuePair<Skill, int>>();
    private List<SkillAllocation> AllocatedSkillPoints = new List<SkillAllocation>();

    // Start is called before the first frame update
    void Start() {
        InitGrid();
    }

    private void InitGrid() {
        if (UISkillArray == null) {
            UISkillArray = GridLayout.GetComponentsInChildren<UISkill>().ToList();
        };
    }

    private void ResetGrid() {
        foreach (var uis in UISkillArray) {
            uis.SetSkill(null);
            uis.SetShaders(unownedSkillShader, ownedSkillShader);
            uis.SetWindowController(this);
        }
    }

    public void UpdateSkills() {
        InitGrid();
        ResetGrid();
        tabs = new List<Toggle>();
        var entity = Session.CurrentSession.Entity as Entity;
        var skillTree = entity.SkillTree;

        foreach (var job in skillTree.ClassTree) {
            var tab = Instantiate(tabPrefab);
            tab.onValueChanged.AddListener(delegate {
                OnTabChanged(tab, job);
            });
            tab
                .GetComponent<Tab>()
                .SetLabel(JobHelper.GetJobName(job.Key, Session.CurrentSession.Entity.GetBaseStatus().sex));
            tab.group = tabLayout;
            tab.transform.SetParent(tabLayout.transform);
            tabLayout.RegisterToggle(tab);
            tabs.Add(tab);
        }
        tabs[0].isOn = true;
    }

    public void UpdateSkillPoints() {
        var entity = Session.CurrentSession.Entity as Entity;
        AvailableSkillPoints = (int) entity.GetBaseStatus().SkillPoints;

        skillPointsText.text = $"Skill Points: {AvailableSkillPoints}";
    }

    private void OnTabChanged(Toggle tab, KeyValuePair<int, Dictionary<int, Skill>> tree) {
        ResetGrid();
        foreach (var position in tree.Value.Keys) {
            UISkillArray[position]
                .SetSkill(tree.Value[position]);
        }
    }

    public void HighlightSkill(short skillID, int level) {
        UISkillArray
            .Find(it => it.GetSkillID() == skillID)
            .Highlight(level);
    }

    public bool HasRequiredSkill(short skillID, short level) {
        var entity = Session.CurrentSession.Entity as Entity;
        return entity.SkillTree.HasSkill(skillID, level);
    }

    public void CheckSkillRequirements(short skillID, bool isTraversing = false) {
        if (!isTraversing) {
            NeededSkills = new List<KeyValuePair<Skill, int>>();
        }

        Skill skill = SkillTable.Skills[skillID];

        if (skill.NeededSkillList == null) {
            if (NeededSkills.Count > 0) {
                NeededSkills.Reverse();
            }
            return;
        }

        foreach (var neededSkillDict in skill.NeededSkillList) {
            // Do we have the needed skill?
            if (HasRequiredSkill((short) neededSkillDict.Key, (short) neededSkillDict.Value)) {
                // Yes, just highlight it
                HighlightSkill(skillID, 0);
            } else {
                // No, recursevely lookup all the needed skills to acquire this one
                var neededSkill = SkillTable.Skills[(short) neededSkillDict.Key];
                Debug.Log($"Skill {neededSkill.SkillName}({neededSkillDict.Value}) is needed");
                HighlightSkill(neededSkill.SkillId, neededSkillDict.Value);
                CheckSkillRequirements(neededSkill.SkillId, true);
                NeededSkills.Add(new KeyValuePair<Skill, int>(neededSkill, neededSkillDict.Value));
            }
        }
    }

    public void ResetSkillRequirements() {
        NeededSkills = new List<KeyValuePair<Skill, int>>();
        UISkillArray
            .FindAll(it => it.IsHighlighted)
            .ForEach(it => it.UnHighlight());
    }

    public void ApplyAllocatedPoints() {
        var packets = new List<CZ.UPGRADE_SKILLLEVEL>();

        foreach(var allocation in AllocatedSkillPoints) {
            for (int i = 0; i < allocation.level; i++) {
                packets.Add(new CZ.UPGRADE_SKILLLEVEL {
                    SkillID = allocation.skillID
                });
            }
        }

        packets.ForEach(it => it.Send());
    }

    public void ResetAllocatedPoints() {
        AllocatedSkillPoints = new List<SkillAllocation>();
    }

    public void AllocateSkillPoints(short skillID) {
        var targetUiSkill = UISkillArray.Find(it => it.GetSkillID() == skillID);

        foreach (var skill in NeededSkills) {
            if (AllocatedSkillPoints.Any(it => it.skillID == skill.Key.SkillId)) {
                continue;
            }
            var uiSkill = UISkillArray.Find(it => it.GetSkillID() == skill.Key.SkillId);

            if (AvailableSkillPoints >= skill.Value) {
                uiSkill.AddPoints(skill.Value);

                AllocatedSkillPoints.Add(new SkillAllocation {
                    skillID = skill.Key.SkillId,
                    level = skill.Value
                });
                
                UpdateSkillPoints(skill.Value);
            } else {
                uiSkill.AddPoints(AvailableSkillPoints);

                AllocatedSkillPoints.Add(new SkillAllocation {
                    skillID = skill.Key.SkillId,
                    level = AvailableSkillPoints
                });

                UpdateSkillPoints(AvailableSkillPoints);
            }
        }

        if (AvailableSkillPoints > 0 && targetUiSkill.CurrentPoints < targetUiSkill.Skill.MaxLv) {
            targetUiSkill.AddPoints(1);
            UpdateSkillPoints(1);


            SkillAllocation allocation = AllocatedSkillPoints.Find(it => it.skillID == targetUiSkill.Skill.SkillId);
            if (allocation == null) {
                AllocatedSkillPoints.Add(new SkillAllocation {
                    skillID = targetUiSkill.Skill.SkillId,
                    level = 1
                });
            } else {
                allocation.level += 1;
            }
        }
    }

    private void UpdateSkillPoints(int value) {
        var entity = Session.CurrentSession.Entity as Entity;
        entity.GetBaseStatus().SkillPoints -= (uint) value;
        UpdateSkillPoints();
    }
}

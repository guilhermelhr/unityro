using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillWindowController : MonoBehaviour, ISkillWindowController {

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

    public void AllocateSkillPoints(short skillID) {
        var points = new List<short>();
        foreach (var skill in NeededSkills) {
            for (int i = 0; i < skill.Value; i++) {
                points.Add(skill.Key.SkillId);
            }
        }
        points.Add(skillID);

        if (AvailableSkillPoints >= points.Count) {

        }
    }
}

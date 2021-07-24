using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillWindowController : MonoBehaviour {

    [SerializeField] private GridLayoutGroup GridLayout;
    [SerializeField] private ToggleGroup tabLayout;
    [SerializeField] private Toggle tabPrefab;
    [SerializeField] private TextMeshProUGUI skillPointsText;


    private UISkill[] UISkillArray;
    private List<Toggle> tabs = new List<Toggle>();
    private int SkillPoints;

    // Start is called before the first frame update
    void Start() {
        InitGrid();
    }

    private void InitGrid() {
        if (UISkillArray == null) {
            UISkillArray = GridLayout.GetComponentsInChildren<UISkill>();
        };
    }

    private void ResetGrid() {
        foreach (var uis in UISkillArray) {
            uis.SetSkill(null);
        }
    }

    public void UpdateSkills() {
        InitGrid();
        ResetGrid();
        var entity = Session.CurrentSession.Entity as Entity;
        var skillTree = entity.SkillTree;

        foreach (var job in skillTree.ClassTree) {
            var tab = Instantiate(tabPrefab);
            tab.onValueChanged.AddListener(delegate {
                OnTabChanged(tab, job);
            });
            tab.GetComponent<Tab>().SetLabel(JobHelper.GetJobName(job.Key, Session.CurrentSession.Entity.GetBaseStatus().sex));
            tab.group = tabLayout;
            tab.transform.SetParent(tabLayout.transform);
            tabLayout.RegisterToggle(tab);
            tabs.Add(tab);
        }
        tabs[0].isOn = true;
    }

    public void UpdateSkillPoints() {
        var entity = Session.CurrentSession.Entity as Entity;
        SkillPoints = (int) entity.GetBaseStatus().SkillPoints;

        skillPointsText.text = $"Skill Points: {SkillPoints}";
    }

    private void OnTabChanged(Toggle tab, KeyValuePair<int, Dictionary<int, Skill>> tree) {
        ResetGrid();
        foreach (var position in tree.Value.Keys) {
            UISkillArray[position].SetSkill(tree.Value[position]);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillWindowController : MonoBehaviour {

    [SerializeField]
    private GridLayoutGroup GridLayout;

    private UISkill[] UISkillArray;

    // Start is called before the first frame update
    void Start() {
        InitGrid();
    }

    private void InitGrid() {
        if(UISkillArray != null) return;
        UISkillArray = GridLayout.GetComponentsInChildren<UISkill>();
        foreach(var uis in UISkillArray) {
            uis.SetSkill(null);
        }
    }

    public void UpdateSkills() {
        InitGrid();
        var skillTree = Core.Session.Entity.SkillTree;
        var firstJob = skillTree.ClassTree[1];
        foreach(var position in firstJob.Keys) {
            UISkillArray[position].SetSkill(firstJob[position]);
        }
    }
}

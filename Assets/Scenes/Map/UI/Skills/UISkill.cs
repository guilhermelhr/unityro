using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UISkill : MonoBehaviour {

    private const string NO_SKILL_IMAGE = "basic_interface/no_skill.bmp";
    private Texture2D NO_SKILL_TEXTURE;

    [SerializeField]
    private RawImage skillImage;

    [SerializeField]
    private TextMeshProUGUI skillName;

    private Skill Skill;


    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void SetSkill(Skill skill) {
        if(skill != null) {
            this.Skill = skill;
            try {
                var texture = FileManager.Load($"{DBManager.INTERFACE_PATH}item/{skill.SkillTag.ToLower()}.bmp") as Texture2D;
                skillImage.texture = texture;
            } catch {

            }
            skillName.text = skill.SkillName;
        } else {
            if(NO_SKILL_TEXTURE == null)
                NO_SKILL_TEXTURE = FileManager.Load(DBManager.INTERFACE_PATH + NO_SKILL_IMAGE) as Texture2D;

            skillImage.texture = NO_SKILL_TEXTURE;
        }
    }
}

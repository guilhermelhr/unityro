using ROIO;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {

    private const string NO_SKILL_IMAGE = "basic_interface/no_skill.bmp";
    private Texture2D NO_SKILL_TEXTURE;

    [SerializeField]
    private RawImage skillImage;

    [SerializeField]
    private TextMeshProUGUI skillName;

    private Material unownedSkillShader;
    private Shader ownedSkillShader;

    private Skill Skill;
    private ISkillWindowController skillWindowController;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }

    public void SetSkill(Skill skill) {
        Skill = skill;

        if (skill != null) {
            try {
                var texture = FileManager.Load($"{DBManager.INTERFACE_PATH}item/{skill.SkillTag.ToLower()}.bmp") as Texture2D;
                skillImage.texture = texture;
                skillImage.material = unownedSkillShader;
            } catch {

            }
            skillName.text = skill.SkillName;
        } else {
            if (NO_SKILL_TEXTURE == null)
                NO_SKILL_TEXTURE = FileManager.Load(DBManager.INTERFACE_PATH + NO_SKILL_IMAGE) as Texture2D;

            skillImage.texture = NO_SKILL_TEXTURE;
            skillName.text = null;
        }
    }

    internal void SetWindowController(ISkillWindowController skillWindowController) {
        this.skillWindowController = skillWindowController;
    }

    internal void SetShaders(Material unownedSkillShader, Shader ownedSkillShader) {
        this.unownedSkillShader = unownedSkillShader;
        this.ownedSkillShader = ownedSkillShader;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (Skill != null) {
            skillWindowController.CheckSkillRequirements(Skill.SkillId);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (Skill != null) {
            throw new System.NotImplementedException();
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (Skill != null) {
            throw new System.NotImplementedException();
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (Skill != null) {
            throw new System.NotImplementedException();
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (Skill != null) {
            throw new System.NotImplementedException();
        }
    }

    public short GetSkillID() {
        return Skill?.SkillId ?? -1;
    }

    public void Highlight(int level) {
        Debug.Log($"Highlight {Skill.SkillName} at level {level}");
    }
}

using ROIO;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {

    private const string NO_SKILL_IMAGE = "basic_interface/no_skill.bmp";
    private Texture2D NO_SKILL_TEXTURE;

    [SerializeField] private RawImage skillContainer;
    [SerializeField] private RawImage skillImage;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI neededLevel;
    [SerializeField] private TextMeshProUGUI currentLevelLabel;
    [SerializeField] private TextMeshProUGUI allocatedPointsLabel;
    [SerializeField] private CustomButton increaseLevelButton;
    [SerializeField] private CustomButton decreaseLevelButton;
    [SerializeField] private Color highlightedColor;

    private Material unownedSkillShader;
    private Material ownedSkillShader;

    private ISkillWindowController skillWindowController;
    public Skill Skill { get; private set; }
    public SkillInfo SkillInfo { get; private set; }
    public bool IsHighlighted { get; private set; }
    public int AllocatedPoints { get; private set; }
    public int SelectedLevel { get; private set; }

    internal void SetSkill(Skill skill) {
        Skill = skill;

        if (skill != null) {
            try {
                var texture = FileManager.Load($"{DBManager.INTERFACE_PATH}item/{skill.SkillTag.ToLower()}.bmp") as Texture2D;
                skillImage.texture = texture;
                skillImage.material = unownedSkillShader;
            } catch { }

            skillName.text = skill.SkillName;
        } else {
            if (NO_SKILL_TEXTURE == null)
                NO_SKILL_TEXTURE = FileManager.Load(DBManager.INTERFACE_PATH + NO_SKILL_IMAGE) as Texture2D;

            skillImage.texture = NO_SKILL_TEXTURE;
            skillImage.material = unownedSkillShader;
            skillName.text = null;
        }
    }

    internal void Reset() {
        SetSkill(null);
        SetSkillInfo(null);
        allocatedPointsLabel.text = null;
        increaseLevelButton.gameObject.SetActive(false);
        decreaseLevelButton.gameObject.SetActive(false);
        AllocatedPoints = 0;
        SelectedLevel = 0;
    }

    internal void SetWindowController(ISkillWindowController skillWindowController) {
        this.skillWindowController = skillWindowController;
    }

    internal void SetShaders(Material unownedSkillShader, Material ownedSkillShader) {
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
            skillWindowController.ResetSkillRequirements();
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (Skill == null) {
            return;
        }

        switch (eventData.clickCount) {
            case 1:
                skillWindowController.AllocateSkillPoints(Skill.SkillId);
                break;
            case 2:
                skillWindowController.UseSkill(Skill.SkillId, (short) SelectedLevel, SkillInfo.SkillType);
                break;
            default:
                return;
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

    internal short GetSkillID() => Skill?.SkillId ?? -1;

    internal void SetSkillInfo(SkillInfo skillInfo) {
        SkillInfo = skillInfo;

        if (skillInfo != null && skillInfo.Level > 0) {
            skillImage.material = ownedSkillShader;
            SelectedLevel = skillInfo.Level;

            SetCurrentLevelLabelText();
        } else {
            skillImage.material = unownedSkillShader;
            currentLevelLabel.text = null;
        }
    }

    private void SetCurrentLevelLabelText() {
        if (Skill.CanSelectLevel) {
            currentLevelLabel.text = $"{SelectedLevel}/{SkillInfo.Level}";
            increaseLevelButton.gameObject.SetActive(true);
            decreaseLevelButton.gameObject.SetActive(true);
        } else {
            increaseLevelButton.gameObject.SetActive(false);
            decreaseLevelButton.gameObject.SetActive(false);
            currentLevelLabel.text = $"{SelectedLevel}";
        }
    }

    internal void Highlight(int level) {
        IsHighlighted = true;
        skillContainer.color = highlightedColor;

        if (level > 0) {
            neededLevel.text = $"{level}";
        }
    }

    internal void UnHighlight() {
        IsHighlighted = false;
        neededLevel.text = null;
        skillContainer.color = Color.white;
    }

    internal void AddPoints(int value) {
        AllocatedPoints += value;
        allocatedPointsLabel.text = $"+{AllocatedPoints}";
    }

    internal int GetCurrentLevel() {
        return SkillInfo?.Level ?? 0;
    }

    internal bool CanUpgradeCurrentSkill() => GetCurrentLevel() + AllocatedPoints < Skill.MaxLv;

    public void IncreaseCurrentLevel() {
        if (Skill.CanSelectLevel && SelectedLevel < SkillInfo.Level) {
            SelectedLevel++;
        }
        SetCurrentLevelLabelText();
    }

    public void DecreaseCurrentLevel() {
        if (Skill.CanSelectLevel && SelectedLevel > 1) {
            SelectedLevel--;
        }
        SetCurrentLevelLabelText();
    }
}

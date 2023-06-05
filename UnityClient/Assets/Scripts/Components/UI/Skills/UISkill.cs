using ROIO;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityRO.Net;

public class UISkill : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler,
    IUsable,
    IBeginDragHandler,
    IEndDragHandler,
    IDragHandler {

    private const string NO_SKILL_IMAGE = "basic_interface/no_skill.png";
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
    private Canvas Canvas;
    private RectTransform ItemDragImageTransform;

    private ISkillWindowController skillWindowController;
    public Skill Skill { get; private set; }
    public SkillInfo SkillInfo { get; private set; }
    public bool IsHighlighted { get; private set; }
    public int AllocatedPoints { get; private set; }
    public int SelectedLevel { get; private set; }

    private void Awake() {
        Canvas = Canvas.FindMainCanvas();
    }

    internal async void SetSkill(Skill skill) {
        Skill = skill;

        if (skill != null) {
            try {
                var texture = await Addressables.LoadAssetAsync<Texture2D>($"{DBManager.INTERFACE_PATH}item/{skill.SkillTag.ToLower()}.png").Task;
                skillImage.texture = texture;
                skillImage.material = unownedSkillShader;
            } catch { }

            skillName.text = skill.SkillName;
        } else {
            if (NO_SKILL_TEXTURE == null)
                NO_SKILL_TEXTURE = await Addressables.LoadAssetAsync<Texture2D>(DBManager.INTERFACE_PATH + NO_SKILL_IMAGE).Task;

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
                UseSkill();
                break;
            default:
                return;
        }
    }

    private void UseSkill() {
        skillWindowController.UseSkill(SkillInfo, (short) SelectedLevel);
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

    public string GetDisplayName() {
        return Skill.SkillName;
    }

    public int GetDisplayNumber() {
        return SelectedLevel;
    }

    public Texture2D GetTexture() {
        return skillImage.texture as Texture2D;
    }

    public void OnUse() {
        UseSkill();
    }

    public void OnRightClick() {
        throw new NotImplementedException();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (Skill == null) {
            return;
        }

        var ItemDragImage = new GameObject("ShopItemDrag");
        ItemDragImage.transform.SetParent(Canvas.transform, false);
        ItemDragImage.transform.SetAsLastSibling();

        var image = ItemDragImage.AddComponent<RawImage>();
        image.texture = skillImage.texture as Texture2D;
        image.SetNativeSize();

        CanvasGroup canvasGroup = ItemDragImage.gameObject.AddComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;

        ItemDragImageTransform = ItemDragImage.GetComponent<RectTransform>();
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(eventData.pointerEnter.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out var globalMousePos)
        ) {
            ItemDragImageTransform.position = globalMousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        Destroy(ItemDragImageTransform.gameObject);
    }

    public void OnDrag(PointerEventData eventData) {
        ItemDragImageTransform.anchoredPosition += eventData.delta / Canvas.scaleFactor;
    }
}

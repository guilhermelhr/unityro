using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharCreationController : MonoBehaviour {

    private const int HUMAN_MAX_HAIRSTYLE = 23;
    private const int DORAM_MAX_HAIRTYLE = 6;

    public Entity StyleEntity;
    public Entity HumanSelectionEntity;
    public Entity DoramSelecionEntity;

    public GridLayoutGroup GridLayout;
    public ToggleGroup HairToggleGroup;
    public ToggleGroup SexToggleGroup;

    private bool IsDirty = false;
    private List<ToggleImage> HairToggleList;

    private int SelectedSex = 1;
    private bool IsHumanSelected = true;
    private int SelectedHair = 0;
    private int SelectedHairColor = 0;

    void Start() {
        InitEntity(StyleEntity);
        InitEntity(HumanSelectionEntity);
        InitEntity(DoramSelecionEntity);

        HairToggleList = GridLayout.GetComponentsInChildren<ToggleImage>().ToList();
    }

    void LateUpdate() {
        if (!IsDirty) {
            HumanSelectionEntity.ChangeMotion(new EntityViewer.MotionRequest { Motion = SpriteMotion.Walk });
            HairToggleList[0].Toggle.isOn = true;
            SexToggleGroup.ActiveToggles().First().isOn = true;
            SetHairstyles();

            IsDirty = true;
        }
    }

    public void SetRace(bool isHuman) {
        IsHumanSelected = isHuman;
        if (isHuman) {
            DoramSelecionEntity.ChangeMotion(new EntityViewer.MotionRequest { Motion = SpriteMotion.Idle });
            HumanSelectionEntity.ChangeMotion(new EntityViewer.MotionRequest { Motion = SpriteMotion.Walk });
        } else {
            DoramSelecionEntity.ChangeMotion(new EntityViewer.MotionRequest { Motion = SpriteMotion.Walk });
            HumanSelectionEntity.ChangeMotion(new EntityViewer.MotionRequest { Motion = SpriteMotion.Idle });
        }

        SetHairstyles();
    }

    public void SetSex(int sex) {
        SelectedSex = sex;
        UpdateEntity(StyleEntity, sex);
        SetHairstyles();
    }

    private void InitEntity(Entity entity, int sex = 1) {
        entity.Init(new CharacterData() { Sex = sex, Job = 0, Name = "Player", GID = 20001, Weapon = 1, Speed = 150 }, LayerMask.NameToLayer("Characters"), true);
        entity.SortingGroup.sortingOrder = 3;
        entity.SetReady(true, true);
    }

    private void UpdateEntity(Entity entity, int sex = 1, int hair = 0, int color = 0) {
        entity.Status.sex = (byte) sex;
        entity.Status.hair = (short) hair;
        entity.Status.hair_color = (short) color;
        entity.UpdateSprites();
    }

    private void SetHairstyles() {
        var count = IsHumanSelected ? HUMAN_MAX_HAIRSTYLE : DORAM_MAX_HAIRTYLE;
        for (int i = 1; i <= count; i++) {
            var hairstylePath = "make_character_ver2/img_hairstyle";
            if (IsHumanSelected) {
                var sexPath = SelectedSex == 1 ? "" : "_girl";
                hairstylePath += $"{sexPath}{i.ToString("D2")}";
            } else {
                var sexPath = SelectedSex == 1 ? "_doramboy" : "_doramgirl";
                hairstylePath += $"{sexPath}{i.ToString("D2")}";
            }

            var index = i - 1;
            var toggle = HairToggleList[index];
            toggle.SetImage(hairstylePath + ".bmp", index);
            toggle.onValueChanged.AddListener(OnHairToggleChanged);
        }
    }

    private void OnHairToggleChanged(int index) {
        SelectedHair = index;
        UpdateEntity(StyleEntity, SelectedSex, SelectedHair, SelectedHairColor);
    }
}

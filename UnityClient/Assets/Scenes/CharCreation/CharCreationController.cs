using ROIO;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CharCreationController : MonoBehaviour {

    private const int HUMAN_MAX_HAIRSTYLE = 23;
    private const int DORAM_MAX_HAIRTYLE = 6;

    public Entity StyleEntity;
    public Entity HumanSelectionEntity;
    public Entity DoramSelecionEntity;
    public TMP_InputField CharacterName;

    public GridLayoutGroup GridLayout;
    public ToggleGroup HairToggleGroup;
    public ToggleGroup SexToggleGroup;

    public RawImage background;

    private bool IsDirty = false;
    private List<ToggleImage> HairToggleList;
    private NetworkClient NetworkClient;

    private int SelectedSex = 1;
    private bool IsHumanSelected = true;
    private int SelectedHair = 1;
    private int SelectedHairColor = 0;

    void Start() {
        background.SetLoginBackground();
        NetworkClient = FindObjectOfType<NetworkClient>();

        InitEntity(StyleEntity);
        InitEntity(HumanSelectionEntity, sex: SelectedSex, job: 0);
        InitEntity(DoramSelecionEntity, sex: SelectedSex, job: 4218);

        HairToggleList = GridLayout.GetComponentsInChildren<ToggleImage>().ToList();
    }

    void Update() {
        if (!IsDirty) {
            HumanSelectionEntity.ChangeMotion(new MotionRequest { Motion = SpriteMotion.Walk });
            HairToggleList[0].Toggle.isOn = true;
            SexToggleGroup.ActiveToggles().First().isOn = true;
            SetHairstyles();

            IsDirty = true;
        }
    }

    public void SetRace(bool isHuman) {
        IsHumanSelected = isHuman;
        if (isHuman) {
            DoramSelecionEntity.ChangeMotion(new MotionRequest { Motion = SpriteMotion.Idle });
            HumanSelectionEntity.ChangeMotion(new MotionRequest { Motion = SpriteMotion.Walk });
            InitEntity(StyleEntity, sex: SelectedSex, job: 0);
        } else {
            DoramSelecionEntity.ChangeMotion(new MotionRequest { Motion = SpriteMotion.Walk });
            HumanSelectionEntity.ChangeMotion(new MotionRequest { Motion = SpriteMotion.Idle });
            InitEntity(StyleEntity, sex: SelectedSex, job: 4218);
        }

        SetHairstyles();
    }

    public void SetSex(int sex) {
        SelectedSex = sex;
        SetHairstyles();
        UpdateEntity(StyleEntity, sex);
        UpdateEntity(HumanSelectionEntity, sex);
        UpdateEntity(DoramSelecionEntity, sex);
    }

    public void CreateCharacter() {
        var name = CharacterName.text;
        if (name.Length < 4) {
            return;
        }

        new CH.MAKE_CHAR2() {
            Name = name,
            CharNum = (byte) NetworkClient.State.CurrentCharactersInfo.Chars.Count,
            Sex = (byte) SelectedSex,
            Head = (ushort) SelectedHair,
            HeadPal = (ushort) SelectedHairColor,
            StartJob = IsHumanSelected ? 0 : 4218
        }.Send();
    }

    public void CloseWindow() {
        SceneManager.UnloadSceneAsync(6);
    }

    private void InitEntity(Entity entity, int sex = 1, int job = 0) {
        entity.Init(new CharacterData() { Sex = sex, Job = (short) job, Name = "Player", GID = 20001, Weapon = 1, Speed = 150, Head = 1 }, LayerMask.NameToLayer("Characters"), null, true);
        entity.SortingGroup.sortingOrder = 3;
        entity.SetReady(true, true);
    }

    private void UpdateEntity(Entity entity, int sex = 1, int hair = 1, int color = 1) {
        entity.Status.sex = (byte) sex;
        entity.Status.hair = (short) hair;
        entity.Status.hair_color = (short) color;
        entity.UpdateSprites();
    }

    private void SetHairstyles() {
        if (HairToggleList == null) {
            return;
        }
        var count = IsHumanSelected ? HUMAN_MAX_HAIRSTYLE : DORAM_MAX_HAIRTYLE;
        HairToggleList.ForEach(it => it.SetImage(null, -1));
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
            toggle.SetImage(hairstylePath + ".png", index);
            toggle.onValueChanged.AddListener(OnHairToggleChanged);
        }
    }

    private void OnHairToggleChanged(int index) {
        SelectedHair = index + 1;
        UpdateEntity(StyleEntity, SelectedSex, SelectedHair, SelectedHairColor);
    }
}

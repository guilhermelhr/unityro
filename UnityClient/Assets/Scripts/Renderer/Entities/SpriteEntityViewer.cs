using Assets.Scripts.Renderer.Sprite;
using ROIO.Models.FileTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;

public class SpriteEntityViewer : GameEntityViewer {

    public Entity Entity;
    public SpriteEntityViewer Parent;
    public ViewerType ViewerType;

    public int HeadDirection;

    public List<SpriteEntityViewer> Children = new List<SpriteEntityViewer>();
    private Dictionary<ACT.Frame, Mesh> ColliderCache = new Dictionary<ACT.Frame, Mesh>();
    private Dictionary<ACT.Frame, Mesh> MeshCache = new Dictionary<ACT.Frame, Mesh>();
    private GameObject ShadowObject;

    private PaletteData CurrentPaletteData;
    private Sprite[] Sprites;
    private ACT CurrentACT;
    private ACT.Action CurrentAction;
    private int CurrentViewID;
    private int CurrentFrameIndex = 0;
    private int ActionId = 0;

    private MeshCollider MeshCollider;
    private MeshFilter MeshFilter;
    private MeshRenderer MeshRenderer;
    private Material SpriteMaterial;
    private SortingGroup SortingGroup;
    private Texture2D PaletteTexture;

    private IFramePaceCalculator FramePaceCalculator;
    private GameObject Mesh3D;

    private bool IsReady = false;

    private bool IsHead => ViewerType == ViewerType.HEAD ||
        ViewerType == ViewerType.HEAD_BOTTOM ||
        ViewerType == ViewerType.HEAD_TOP ||
        ViewerType == ViewerType.HEAD_MID;

    public void Start() {
        SpriteMaterial = Resources.Load("Materials/Sprites/SpriteMaterial") as Material;

        InitFramePaceCalculator();

        Init();
        InitShadow();
        IsReady = true;
    }

    private void InitFramePaceCalculator() {
        FramePaceCalculator = Entity.CurrentFramePaceAlgorithm switch {
            Entity.FramePaceAlgorithm.RoBrowser => gameObject.GetOrAddComponent<RoBrowserFramePaceCalculator>(),
            Entity.FramePaceAlgorithm.UnityRO => gameObject.GetOrAddComponent<UnityROFramePaceCalculator>(),
            Entity.FramePaceAlgorithm.DHXJ => throw new NotImplementedException(),
            _ => throw new NotImplementedException(),
        };
    }

    public override void Init(SpriteData spriteData, Texture2D atlas) {
        CurrentACT = spriteData.act;
        Sprites = spriteData.GetSprites(atlas);

        if (FramePaceCalculator == null) {
            InitFramePaceCalculator();
        }

        FramePaceCalculator.Init(Entity, ViewerType, CurrentACT);
    }

    public override void Init(bool reloadSprites = false) {
        CurrentPaletteData = new PaletteData {
            hairColor = Entity.Status.hair_color,
            hair = Entity.Status.hair,
            clothesColor = Entity.Status.clothes_color
        };

        InitRenderers();

        if (Entity.Type == EntityType.WARP) {
            return;
        }

        if (Entity.Type == EntityType.ITEM) {
            MeshRenderer.material = SpriteMaterial;
            MeshRenderer.material.mainTexture = Sprites[0].texture;
        }

        if (Sprites == null || reloadSprites) {
            string path = "";
            string palettePath = "";

            switch (ViewerType) {
                case ViewerType.BODY:
                    path = DBManager.GetBodyPath(Entity.Status.jobId, Entity.Status.sex);
                    if (Entity.Status.clothes_color > 0) {
                        palettePath = DBManager.GetBodyPalPath(Entity.Status.jobId, Entity.Status.clothes_color, Entity.Status.sex);
                    }
                    break;
                case ViewerType.HEAD:
                    path = DBManager.GetHeadPath(Entity.Status.jobId, Entity.Status.hair, Entity.Status.sex);
                    if (Entity.Status.hair_color > 0) {
                        palettePath = DBManager.GetHeadPalPath(Entity.Status.hair, Entity.Status.hair_color, Entity.Status.sex);
                    }
                    break;
                case ViewerType.WEAPON:
                    CurrentViewID = Entity.EquipInfo.Weapon;
                    path = DBManager.GetWeaponPath(CurrentViewID, Entity.Status.jobId, Entity.Status.sex);
                    break;
                case ViewerType.SHIELD:
                    CurrentViewID = Entity.EquipInfo.Shield;
                    path = DBManager.GetShieldPath(CurrentViewID, Entity.Status.jobId, Entity.Status.sex);
                    break;
                case ViewerType.HEAD_TOP:
                    CurrentViewID = Entity.EquipInfo.HeadTop;
                    path = DBManager.GetHatPath(CurrentViewID, Entity.Status.sex);
                    break;
                case ViewerType.HEAD_MID:
                    CurrentViewID = Entity.EquipInfo.HeadMid;
                    path = DBManager.GetHatPath(CurrentViewID, Entity.Status.sex);
                    break;
                case ViewerType.HEAD_BOTTOM:
                    CurrentViewID = Entity.EquipInfo.HeadBottom;
                    path = DBManager.GetHatPath(CurrentViewID, Entity.Status.sex);
                    break;
            }

            if (ViewerType != ViewerType.BODY && ViewerType != ViewerType.HEAD && CurrentViewID <= 0) {
                CurrentACT = null;
                Sprites = null;
                ColliderCache.Clear();
                MeshCache.Clear();

                return;
            }

            try {
                var spriteData = Addressables.LoadAssetAsync<SpriteData>(path + ".asset").WaitForCompletion();
                var atlas = Addressables.LoadAssetAsync<Texture2D>(path + ".png").WaitForCompletion();

                Sprites = spriteData.GetSprites(atlas);
                CurrentACT = spriteData.act;

                FramePaceCalculator.Init(Entity, ViewerType, CurrentACT);

                if (SpriteMaterial == null) {
                    SpriteMaterial = Resources.Load("Materials/Sprites/SpriteMaterial") as Material;
                }

                MeshRenderer.material = SpriteMaterial;
                MeshRenderer.material.mainTexture = atlas;
                MeshRenderer.material.renderQueue -= 2;

                if (palettePath.Length == 0) {
                    palettePath = path + "_pal";
                }

                var palette = Addressables.LoadAssetAsync<Texture2D>(palettePath + ".png").WaitForCompletion();
                if (palette != null) {
                    MeshRenderer.material.SetTexture("_PaletteTex", palette);
                } else {
                    // selected palette doesn't exist, fallback to original
                    palettePath = path + "_pal";
                    palette = Addressables.LoadAssetAsync<Texture2D>(palettePath + ".png").WaitForCompletion();
                    MeshRenderer.material.SetTexture("_PaletteTex", palette);
                }
            } catch (Exception e) {
                Debug.LogError($"Could not load sprites for: {path}");
                Debug.LogException(e);
                CurrentACT = null;
            }
        }

        foreach (var child in Children) {
            child.Start();
            child.Init(reloadSprites);
        }
    }

    private void InitRenderers() {
        MeshCollider = gameObject.GetOrAddComponent<MeshCollider>();
        MeshFilter = gameObject.GetOrAddComponent<MeshFilter>();
        MeshRenderer = gameObject.GetOrAddComponent<MeshRenderer>();
        SortingGroup = gameObject.GetOrAddComponent<SortingGroup>();
    }

    void FixedUpdate() {
        if (CurrentACT == null || (!Entity.IsReady && !IsReady))
            return;

        if (ActionId == -1) {
            ChangeMotion(new MotionRequest { Motion = SpriteMotion.Idle });
        }

        if (ViewerType == ViewerType.MESH) {

        } else {
            if (CheckForEntityViewsUpdates()) {
                Init(reloadSprites: true);
                return;
            }

            ACT.Frame frame = UpdateFrame();

            if (CurrentAction == null) {
                return;
            }

            UpdateMesh(frame);
            PlaySound(frame);
            UpdateAnchorPoints();
        }
    }

    private ACT.Frame UpdateFrame() {
        CurrentAction = CurrentACT.actions[FramePaceCalculator.GetActionIndex()];
        CurrentFrameIndex = FramePaceCalculator.GetCurrentFrame();
        var frame = CurrentAction.frames[CurrentFrameIndex];
        return frame;
    }

    private bool CheckForEntityViewsUpdates() {
        if (ViewerType != ViewerType.BODY && ViewerType != ViewerType.HEAD) {
            return FindCurrentViewID() != CurrentViewID;
        }

        if (Entity.Status.hair != CurrentPaletteData.hair ||
            Entity.Status.hair_color != CurrentPaletteData.hairColor ||
            Entity.Status.clothes_color != CurrentPaletteData.clothesColor) {
            return true;
        }

        return false;
    }

    private int FindCurrentViewID() {
        return ViewerType switch {
            ViewerType.WEAPON => Entity.EquipInfo.Weapon,
            ViewerType.SHIELD => Entity.EquipInfo.Shield,
            ViewerType.HEAD_TOP => Entity.EquipInfo.HeadTop,
            ViewerType.HEAD_MID => Entity.EquipInfo.HeadMid,
            ViewerType.HEAD_BOTTOM => Entity.EquipInfo.HeadBottom,
            _ => -1,
        };
    }

    private void UpdateAnchorPoints() {
        if (Parent != null && ViewerType != ViewerType.WEAPON) {
            var parentAnchor = Parent.GetAnimationAnchor();
            var ourAnchor = GetAnimationAnchor();

            var diff = parentAnchor - ourAnchor;

            transform.localPosition = new Vector3(diff.x, -diff.y, 0f) / SPR.PIXELS_PER_UNIT;
        }
    }

    private void PlaySound(ACT.Frame frame) {
        if (frame.soundId > -1 && frame.soundId < CurrentACT.sounds.Length) {
            var clipName = CurrentACT.sounds[frame.soundId];
            if (clipName == "atk")
                return;

            Entity.PlayAudio($"data/wav/{clipName}");
        }
    }

    private void UpdateMesh(ACT.Frame frame) {
        // We need this mesh collider in order to have the raycast to hit the sprite
        ColliderCache.TryGetValue(frame, out Mesh colliderMesh);
        if (colliderMesh == null) {
            colliderMesh = SpriteMeshBuilder.BuildColliderMesh(frame, Sprites);
            ColliderCache.Add(frame, colliderMesh);
        }

        MeshCache.TryGetValue(frame, out Mesh rendererMesh);
        if (rendererMesh == null) {
            rendererMesh = SpriteMeshBuilder.BuildSpriteMesh(frame, Sprites);
            MeshCache.Add(frame, rendererMesh);
        }

        foreach (var layer in frame.layers) {
            MeshRenderer.material.SetFloat("_Alpha", layer.color.a);
        }

        MeshFilter.sharedMesh = null;
        MeshFilter.sharedMesh = rendererMesh;
        MeshCollider.sharedMesh = colliderMesh;
    }

    public override void ChangeMotion(MotionRequest motion, MotionRequest? nextMotion = null) {
        if (!IsReady)
            return;

        State = motion.Motion switch {
            SpriteMotion.Dead => SpriteState.Dead,
            SpriteMotion.Sit => SpriteState.Sit,
            SpriteMotion.Idle => SpriteState.Idle,
            SpriteMotion.Walk => SpriteState.Walking,
            _ => SpriteState.Alive,
        };

        int newAction;
        if (motion.Motion == SpriteMotion.Attack) {
            var attackActions = new SpriteMotion[] { SpriteMotion.Attack1, SpriteMotion.Attack2, SpriteMotion.Attack3 };
            var action = DBManager.GetWeaponAction((Job) Entity.Status.jobId, Entity.Status.sex, Entity.EquipInfo.Weapon, Entity.EquipInfo.Shield);
            newAction = AnimationHelper.GetMotionIdForSprite(Entity.Type, attackActions[action]);

            /**
             * Seems like og client makes entity look diagonally up
             * when attacking from the sides
             */
            if (Entity.Direction == Direction.East) {
                Entity.Direction = Direction.NorthEast;
            } else if (Entity.Direction == Direction.West) {
                Entity.Direction = Direction.NorthWest;
            }
        } else {
            newAction = AnimationHelper.GetMotionIdForSprite(Entity.Type, motion.Motion);
        }

        if (newAction == ActionId) {
            return;
        }

        if (Parent == null && ShadowObject != null && State == SpriteState.Dead) {
            ShadowObject.SetActive(false);
        }

        Entity.Action = newAction;
        ActionId = newAction;
        CurrentFrameIndex = 0;

        FramePaceCalculator.OnMotionChanged(motion, nextMotion, newAction);

        foreach (var child in Children) {
            child.ChangeMotion(motion, nextMotion);
        }
    }

    private void InitShadow() {
        if (ViewerType != ViewerType.BODY)
            return;

        ShadowObject = new GameObject("Shadow");
        ShadowObject.layer = LayerMask.NameToLayer("Characters");
        ShadowObject.transform.SetParent(transform, false);
        ShadowObject.transform.localPosition = Vector3.zero;
        ShadowObject.transform.localScale = new Vector3(Entity.ShadowSize, Entity.ShadowSize, Entity.ShadowSize);
        var sortingGroup = ShadowObject.AddComponent<SortingGroup>();
        sortingGroup.sortingOrder = -20001;

        var spriteData = Addressables.LoadAssetAsync<SpriteData>("data/sprite/shadow.asset").WaitForCompletion();
        var atlas = Addressables.LoadAssetAsync<Texture2D>("data/sprite/shadow.png").WaitForCompletion();

        var spriteRenderer = ShadowObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteData.GetSprites(atlas)[0];
        spriteRenderer.sortingOrder = -1;
        spriteRenderer.material.renderQueue -= 2;
        spriteRenderer.material.color = new Color(1, 1, 1, 0.4f);
    }

    public Vector2 GetAnimationAnchor() {
        if (CurrentAction == null) {
            return Vector2.zero;
        }

        var frame = CurrentAction.frames[CurrentFrameIndex];
        if (frame.pos.Length > 0)
            return frame.pos[0];
        if (ViewerType == ViewerType.HEAD && (State == SpriteState.Idle || State == SpriteState.Sit))
            return frame.pos[CurrentFrameIndex];
        return Vector2.zero;
    }

    public override IEnumerator FadeOut() {
        var currentAlpha = MeshRenderer.material.GetFloat("_Alpha");
        while (currentAlpha > 0f) {
            currentAlpha -= Time.deltaTime * 5f;
            MeshRenderer.material.SetFloat("_Alpha", currentAlpha);
            yield return new WaitForEndOfFrame();
        }
    }


}

using ROIO;
using ROIO.Models.FileTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityRO.GameCamera;

public class OfflineUtility : MonoBehaviour {

    private GameManager GameManager;
    private EntityManager EntityManager;

    public string MapName = "prontera";
    public List<long> MapLoadingTimes;
    public List<string> MapNames;

    private Entity offlineEntity;

    private async void Awake() {
        GameManager = FindObjectOfType<GameManager>();
        EntityManager = FindObjectOfType<EntityManager>();

        await Addressables.InitializeAsync().Task;
    }

    void Start() {
        MapLoadingTimes = new List<long>();
        MapNames = new List<string>();

        SpawnCharacter();
        //SpawnMob();
        //GameManager.BeginMapLoading(MapName);

        var descriptors = FileManager.GetFileDescriptors();
        foreach (var key in descriptors.Keys) {
            if (Path.GetExtension(key.ToString()) == ".rsw") {
                MapNames.Add(key.ToString().Replace("data/", "").Replace(".rsw", ""));
            }
        }

        MapNames.Sort();
    }

    async void Update() {
        if (Input.GetKeyUp(KeyCode.F4)) {
            await LoadEffect();
        }
    }

    private async Task LoadEffect() {
        var str = await Addressables.LoadAssetAsync<STR>("data/texture/effect/magnificat.str").Task;
        if (str != null) {
            var renderer = new GameObject().AddComponent<StrEffectRenderer>();
            renderer.transform.SetParent(offlineEntity.transform, false);
            renderer.Initialize(str);
        }
    }

    internal void SelectNextMap() {
        MapName = MapNames.Last();
        MapNames.Remove(MapName);
    }

    void SpawnCharacter() {
        offlineEntity = EntityManager.SpawnPlayer(new CharacterData() {
            Sex = 1,
            Job = 12,
            Name = "Player",
            GID = 20001,
            Weapon = 1,
            Speed = 150,
            Head = 1,
            MaxHP = 100,
            HP = 100,
            MaxSP = 50,
            SP = 50,
            BodyPalette = 2,
        });
        offlineEntity.transform.position = new Vector3(150, 16, 150);
        Session.StartSession(new Session(offlineEntity, 0));

        CharacterCamera charCam = FindObjectOfType<CharacterCamera>();
        charCam.SetTarget(offlineEntity.EntityViewer.transform);

        offlineEntity.SetReady(true);
    }

    private void SpawnMob() {
        var mob = EntityManager.Spawn(new EntitySpawnData() { job = 1002, name = "Poring", GID = 20001, speed = 697, PosDir = new int[] { 0, 0, 0 }, objecttype = EntityType.MOB });
        mob.transform.position = new Vector3(150, 0, 155);
        mob.SetReady(true);
    }

    public async Task LoadMap() {
        await GameManager.BeginMapLoading(MapName);
    }
}

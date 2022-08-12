using ROIO;
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

    private async void Awake() {
        GameManager = FindObjectOfType<GameManager>();
        EntityManager = FindObjectOfType<EntityManager>();

        await Addressables.InitializeAsync();
    }

    void Start() {
        MapLoadingTimes = new List<long>();
        MapNames = new List<string>();

        SpawnCharacter();
        //GameManager.BeginMapLoading(MapName);
        
        var descriptors = FileManager.GetFileDescriptors();
        foreach (var key in descriptors.Keys) {
            if (Path.GetExtension(key.ToString()) == ".rsw") {
                MapNames.Add(key.ToString().Replace("data/", "").Replace(".rsw", ""));
            }
        }

        MapNames.Sort();
    }

    internal void SelectNextMap() {
        MapName = MapNames.Last();
        MapNames.Remove(MapName);
    }

    void SpawnCharacter() {
        var entity = EntityManager.SpawnPlayer(new CharacterData() {
            Sex = 1,
            Job = 0,
            Name = "Player",
            GID = 20001,
            Weapon = 1,
            Speed = 150,
            Hair = 1,
            MaxHP = 100,
            HP = 100,
            MaxSP = 50,
            SP = 50,
            ClothesColor = 1,
        });
        entity.transform.position = new Vector3(150, 16, 150);
        entity.SetAttackSpeed(135);
        Session.StartSession(new Session(entity, 0));

        CharacterCamera charCam = FindObjectOfType<CharacterCamera>();
        charCam.SetTarget(entity.EntityViewer.transform);

        entity.SetReady(true);
        //var mob = EntityManager.Spawn(new EntityData() { job = 1002, name = "Poring", GID = 20001, speed = 697, PosDir = new int[] { 0, 0, 0 }, objecttype = EntityType.MOB });
        //mob.transform.position = new Vector3(150, 0, 155);
        //mob.SetReady(true);
    }

    public async Task LoadMap() {
        var time = await GameManager.BenchmarkMapLoading(MapName);
        MapLoadingTimes.Add(time);
        Debug.Log($"Average map loading times {MapLoadingTimes.Average() / 1000f}");
    }
}

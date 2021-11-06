using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityRO.GameCamera;

public class OfflineUtility : MonoBehaviour {

    private GameManager GameManager;
    private EntityManager EntityManager;

    public string MapName = "prontera";
    public List<long> MapLoadingTimes;

    private void Awake() {
        GameManager = FindObjectOfType<GameManager>();
        EntityManager = FindObjectOfType<EntityManager>();
    }

    void Start() {
        GameManager.BeginMapLoading(MapName);
        SpawnCharacter();
        MapLoadingTimes = new List<long>();
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
            SP = 50
        });
        entity.transform.position = new Vector3(150, 0, 150);
        entity.SetAttackSpeed(135);
        Session.StartSession(new Session(entity, 0));

        CharacterCamera charCam = FindObjectOfType<CharacterCamera>();
        charCam.SetTarget(entity.EntityViewer.transform);

        entity.SetReady(true);
        //var mob = EntityManager.Spawn(new EntityData() { job = 1002, name = "Poring", GID = 20001, speed = 697, PosDir = new int[] { 0, 0, 0 }, objecttype = EntityType.MOB });
        //mob.transform.position = new Vector3(150, 0, 155);
        //mob.SetReady(true);
    }

    public async void LoadMap() {
        var time = await GameManager.BenchmarkMapLoading(MapName);
        MapLoadingTimes.Add(time);
        Debug.Log($"Average map loading times {MapLoadingTimes.Average()}");
    }
}

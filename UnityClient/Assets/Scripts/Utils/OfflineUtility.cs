using UnityEngine;
using UnityRO.GameCamera;

public class OfflineUtility : MonoBehaviour {

    private GameManager GameManager;
    private EntityManager EntityManager;

    private void Awake() {
        GameManager = FindObjectOfType<GameManager>();
        EntityManager = FindObjectOfType<EntityManager>();
    }

    void Start() {
        GameManager.BeginMapLoading("prontera");
        SpawnCharacter();
    }

    void Update() {

    }

    void SpawnCharacter() {
        var entity = EntityManager.SpawnPlayer(new CharacterData() { 
            Sex = 1,
            Job = 0,
            Name = "Player",
            GID = 20001,
            Weapon = 1,
            Speed = 150,
            Hair = 1 
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
}

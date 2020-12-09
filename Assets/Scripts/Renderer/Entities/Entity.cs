using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Entity : MonoBehaviour {

    //private EntityWalk _EntityWalk;

    // Picking Priority
    // TODO

    public EntityType Type = EntityType.UNKNOWN;
    public EntityViewer EntityViewer;
    public Vector3 Position = Vector3.zero;
    public int Direction = 0;
    public float ShadowSize;
    public CharacterData Data;
    public int Action = 0;

    //internal object weapon;
    //internal Job _job = Job.NOVICE;
    //internal int _sex;

    //public long GID;
    //public long GUID;
    //public int xSize = 5;
    //public int ySize = 5;

    //public Matrix4x4 matrix = Matrix4x4.zero;
    //public int headDir = 0;
    //public int direction = 0;
    //public Vector3 position = Vector3.zero;

    //public int attackRange = 0;
    //public int attackSpeed = 300;

    private void Awake() {
        //_EntityViewer = gameObject.AddComponent<EntityViewer>();

        //var character = Core.NetworkClient.State.SelectedCharacter;
        //this.GID = character.GID;
        //this._job = (Job) character.Job;
        //this._sex = character.Sex;


    }

    private void Update() {
        //if (Input.GetMouseButtonDown(0)) {
        //    var ray = Core.MainCamera.ScreenPointToRay(Input.mousePosition);
        //    if (Physics.Raycast(ray, out var hit, 150)) {
        //        _EntityWalk.RequestMove(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.z), 0);
        //    }
        //}
    }

    public void Configure() {
        EntityViewer.UpdateBody((Job) Data.Job, Data.Sex);
    }
}

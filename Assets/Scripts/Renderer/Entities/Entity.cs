using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Entity : MonoBehaviour {

    private EntityWalk _EntityWalk;

    // Picking Priority
    // TODO

    public EntityType Type = EntityType.UNKNOWN;
    public EntityViewer EntityViewer;
    public Vector3 Position = Vector3.zero;
    public int Direction = 0;
    public float ShadowSize;
    public int Action = 0;
    public SpriteAction ActionTable;
    public Animation Animation;
    public int HeadDir;

    public bool IsReady = false;

    public short Job { get; private set; }
    public byte Sex { get; private set; }
    public uint GID { get; private set; }
    public short Hair { get; private set; }

    public void SetReady(bool ready) {
        IsReady = ready;

        if(HasAuthority())
            _EntityWalk = gameObject.AddComponent<EntityWalk>();
    }

    public void Init(EntityData data) {
        Job = data.job;
        Sex = data.sex;
        GID = data.GID;
        Hair = data.hairStyle;
        Type = data.type;

        gameObject.transform.position = new Vector3(data.PosDir[0], 2f, data.PosDir[1]);
    }

    public void Init(CharacterData data) {
        Job = data.Job;
        Sex = (byte)data.Sex;
        GID = (uint)data.GID;
        Hair = data.Hair;
        Type = EntityType.PC;
    }
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

    }

    public bool HasAuthority() {
        return GID == Core.Session.Entity.GID;
    }
}

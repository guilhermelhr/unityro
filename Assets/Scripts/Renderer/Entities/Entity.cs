using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Entity : MonoBehaviour {

    private EntityWalk _EntityWalk;

    // Picking Priority
    // TODO

    public EntityType Type = EntityType.UNKNOWN;
    public EntityViewer EntityViewer;
    public Direction Direction = 0;
    public float ShadowSize;
    public int Action = 0;
    public int HeadDir;

    public bool IsReady = false;
    public bool HasAuthority => GID == Core.Session.Entity.GID;

    public short Job { get; private set; }
    public byte Sex { get; private set; }
    public uint GID { get; private set; }
    public short Hair { get; private set; }

    public void SetReady(bool ready) {
        IsReady = ready;
        _EntityWalk = gameObject.AddComponent<EntityWalk>();
    }

    public void StartMoving(int startX, int startY, int endX, int endY) {
        _EntityWalk.StartMoving(startX, startY, endX, endY);
    }

    public void Init(EntityData data) {
        Job = data.job;
        Sex = data.sex;
        GID = data.GID;
        Hair = data.hairStyle;
        Type = data.type;

        gameObject.transform.position = new Vector3(data.PosDir[0], Core.PathFinding.GetCellHeight(data.PosDir[0],data.PosDir[1]), data.PosDir[1]);
    }

    public void SetAction(SpriteMotion motion) {
        EntityViewer.ChangeMotion(motion);
        EntityViewer.State = AnimationHelper.GetStateForMotion(motion);
    }

    public void Init(CharacterData data) {
        Job = data.Job;
        Sex = (byte)data.Sex;
        GID = (uint)data.GID;
        Hair = data.Hair;
        Type = EntityType.PC;
    }

    private void Awake() {

    }

    private void Update() {

    }
}

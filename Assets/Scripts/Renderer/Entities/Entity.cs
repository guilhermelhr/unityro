using System;
using UnityEngine;

public class Entity : MonoBehaviour {

    private EntityWalk _EntityWalk;

    public const int TYPE_EFFECT = -3;
    public const int TYPE_UNKNOWN = -2;
    public const int TYPE_WARP = -1;
    public const int TYPE_PC = 0;
    public const int TYPE_DISGUISED = 1;
    public const int TYPE_MOB = 5;
    public const int TYPE_NPC = 6;
    public const int TYPE_PET = 7;
    public const int TYPE_HOM = 8;
    public const int TYPE_MERC = 9;
    public const int TYPE_ELEM = 10;
    public const int TYPE_ITEM = 11;

    // Picking Priority
    // TODO

    public int type = TYPE_UNKNOWN;
    internal object weapon;
    internal object _job;
    internal object _sex;

    public long GID;
    public long GUID;
    public int xSize = 5;
    public int ySize = 5;

    public Matrix4x4 matrix = Matrix4x4.zero;
    public int headDir = 0;
    public int direction = 0;
    public Vector3 position = Vector3.zero;

    public int attackRange = 0;
    public int attackSpeed = 300;


    private void Awake() {
        Core.OnMouseActionClick += this.OnMouseActionClick;

        _EntityWalk = gameObject.AddComponent<EntityWalk>();


        _EntityWalk.Init(gameObject.transform);
    }

    private void OnDestroy() {
        Core.OnMouseActionClick -= this.OnMouseActionClick;
    }

    private void OnMouseActionClick(Vector3 targetPosition) {
        _EntityWalk.WalkTo(targetPosition, gameObject.transform.position);
    }
}

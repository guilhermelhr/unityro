using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Entity : MonoBehaviour {

    private EntityWalk _EntityWalk;
    private EntityViewer _EntityViewer;

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
    internal Job _job = Job.NOVICE;
    internal int _sex;

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
        _EntityViewer = gameObject.AddComponent<EntityViewer>();

        //var character = Core.NetworkClient.State.SelectedCharacter;
        //this.GID = character.GID;
        //this._job = (Job) character.Job;
        //this._sex = character.Sex;

        //_EntityViewer.UpdateBody(this._job, this._sex);
        //var go = new GameObject("name");
        //go.layer = LayerMask.NameToLayer("Characters");
        //go.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        //var control = go.AddComponent<EntityWalk>();
        //go.AddComponent<Billboard>();

        //var body = new GameObject("Sprite");
        //body.layer = LayerMask.NameToLayer("Characters");
        //body.transform.SetParent(go.transform, false);
        //body.transform.localPosition = Vector3.zero;
        //body.AddComponent<SortingGroup>();
        //body.AddComponent<SPRRenderer>();

        //var head = new GameObject("Head");
        //head.layer = LayerMask.NameToLayer("Characters");
        //head.transform.SetParent(body.transform, false);
        //head.transform.localPosition = Vector3.zero;


    }

    private void Update() {
        //if (Input.GetMouseButtonDown(0)) {
        //    var ray = Core.MainCamera.ScreenPointToRay(Input.mousePosition);
        //    if (Physics.Raycast(ray, out var hit, 150)) {
        //        _EntityWalk.RequestMove(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.z), 0);
        //    }
        //}
    }
}

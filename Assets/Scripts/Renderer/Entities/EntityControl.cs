using UnityEngine;
using System.Collections;
using System;

public class EntityControl : MonoBehaviour {

    public Entity Entity;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        var ray = Core.MainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 150, LayerMask.GetMask("NPC", "Monsters", "Items"))) {
            hit.collider.gameObject.TryGetComponent<EntityViewer>(out var target);

            if (target == null) return;

            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                ProcessClickAction(target.Entity);
            }
        }
    }

    private void ProcessClickAction(Entity target) {
        switch (target.Type) {
            case EntityType.NPC:
                new CZ.CONTACTNPC() {
                    NAID = target.AID,
                    Type = 1
                }.Send();
                break;
            case EntityType.ITEM:
                Core.CursorRenderer.SetAction(CursorAction.PICK, false, 2);

                OutPacket pickPacket = new CZ.ITEM_PICKUP2() { ID = (int)target.GID };
                if (Vector3.Distance(transform.position, target.transform.position) > 2) {
                    Entity.AfterMoveAction = pickPacket;

                    new CZ.REQUEST_MOVE2() {
                        x = (short)target.transform.position.x,
                        y = (short)target.transform.position.z,
                        dir = 0
                    }.Send();

                    break;
                }

                pickPacket.Send();
                Entity.LookTo(target.transform.position);
                break;
            case EntityType.MOB:
                // TODO render lock arrow
                var path = Core.PathFinding.GetPath(Entity.transform.position, target.transform.position, Entity.AttackRange + 1);

                if (path.Count == 0) {
                    return;
                }

                OutPacket packet = new CZ.REQUEST_ACT2() {
                    TargetID = target.AID,
                    action = EntityActionType.CONTINUOUS_ATTACK
                };

                PathNode endNode;
                if (path.Count < 2) {
                    packet.Send();
                    endNode = path[path.Count - 1];
                } else {
                    endNode = path[path.Count - 2];
                }

                Entity.AfterMoveAction = packet;

                new CZ.REQUEST_MOVE2() {
                    x = (short)endNode.x,
                    y = (short)endNode.y,
                    dir = (byte)Entity.Direction
                }.Send();

                break;
            case EntityType.WARP:
                break;
        }

    }
}

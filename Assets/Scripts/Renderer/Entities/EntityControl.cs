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
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            ProcessClickAction();
        }
    }

    private void ProcessClickAction() {
        var ray = Core.MainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 150, LayerMask.GetMask("NPC", "Monsters"))) {
            hit.collider.gameObject.TryGetComponent<EntityViewer>(out var target);

            if (target != null) {
                switch (target.Entity.Type) {
                    case EntityType.NPC:
                        new CZ.CONTACTNPC() {
                            NAID = target.Entity.GID,
                            Type = 1
                        }.Send();
                        break;
                    case EntityType.ITEM:
                        break;
                    case EntityType.MOB:
                        // TODO render lock arrow
                        var path = Core.PathFinding.GetPath(Entity.transform.position, target.transform.position, Entity.AttackRange + 1);

                        if (path.Count == 0) {
                            return;
                        }

                        OutPacket packet = new CZ.REQUEST_ACT2() {
                            TargetGID = target.Entity.GID,
                            action = 0
                        };

                        PathNode endNode;
                        if (path.Count < 2) {
                            packet.Send();
                            endNode = path[path.Count - 1];
                        } else {
                            endNode = path[path.Count - 2];
                        }

                        //TODO figure out what this is
                        Entity.MoveAction = packet;

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
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class EntityWalk : MonoBehaviour {

    private Entity Entity;

    private long _tick = 0;
    private int nodeIndex = 0;
    private List<Vector3> nodes;
    private bool isWalking = false;
    private ushort lastSpeed = 150;
    private Vector3 lastPosition;

    private void Awake() {
        Entity = GetComponent<Entity>();

        if(Entity.HasAuthority) {
            Core.NetworkClient.HookPacket(ZC.NOTIFY_PLAYERMOVE.HEADER, OnPlayerMovement); //Our movement
        }
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0) && Entity.HasAuthority && !EventSystem.current.IsPointerOverGameObject()) {
            var ray = Core.MainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out var hit, 150)) {
                RequestMove(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.z), 0);
            }
        }

        if(isWalking && !nodes.IsEmpty()) {
            bool isEnd = false;
            while(_tick <= Core.Tick) {
                var current = nodes[nodeIndex];
                if(nodeIndex == nodes.Count - 1) {
                    isEnd = true;
                    break;
                }
                var next = nodes[nodeIndex + 1];
                var isDiagonal = PathFindingManager.IsDiagonal(next, current);
                lastSpeed = (ushort)(isDiagonal ? Entity.WalkSpeed * 14 / 10 : Entity.WalkSpeed); //Diagonal walking is slower
                _tick += lastSpeed;
                nodeIndex++;
            }

            var currentNode = nodeIndex == 0 ? lastPosition : nodes[nodeIndex - 1];
            var nextNode = nodes[nodeIndex];
            var direction = nextNode - currentNode;
            float timeDelta = 1 - Math.Max(_tick - Core.Tick, 0f) / lastSpeed;

            transform.position = currentNode + direction * timeDelta;
            Entity.Direction = PathFindingManager.GetDirectionForOffset(nextNode, currentNode);

            if(isEnd) {
                isWalking = false;
                nodes.Clear();

                StartCoroutine(OnWalkEnd());
            }
        }
    }

    /**
     * Server has acknowledged our request and set data back to us
     */
    private void OnPlayerMovement(ushort cmd, int size, InPacket packet) {
        if(!Entity) return;
        if(packet is ZC.NOTIFY_PLAYERMOVE) {
            var pkt = packet as ZC.NOTIFY_PLAYERMOVE;
            Entity.ChangeMotion(SpriteMotion.Walk);

            StartMoving(pkt.startPosition[0], pkt.startPosition[1], pkt.endPosition[0], pkt.endPosition[1]);
        }
    }

    public void StartMoving(int startX, int startY, int endX, int endY) {
        _tick = Core.Tick;
        nodeIndex = 0;
        nodes = Core.PathFinding.GetPath(startX, startY, endX, endY).Select(node => new Vector3(node.x, (float)node.y, node.z)).ToList();
        isWalking = true;
        lastPosition = transform.position;
    }

    public void StopMoving() {
        Entity.ChangeMotion(SpriteMotion.Idle);
    }

    private IEnumerator OnWalkEnd() {
        Entity.ChangeMotion(SpriteMotion.Idle);
        yield return new WaitForEndOfFrame();
        Entity.AfterMoveAction?.Send();
        Entity.AfterMoveAction = null;

        yield return null;
    }

    public void RequestMove(int x, int y, int dir) {
        /**
         * Validate things such as if entity is sit, whatever
         */
        if(Core.Instance.Offline) {
            Entity.ChangeMotion(SpriteMotion.Walk);
            StartMoving((int)transform.position.x, (int)transform.position.z, x, y);
        } else {
            new CZ.REQUEST_MOVE2(x, y, dir).Send();
        }
    }
}
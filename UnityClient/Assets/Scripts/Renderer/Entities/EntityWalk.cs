using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityWalk : MonoBehaviour {

    private Entity Entity;

    private long _tick = 0;
    private int nodeIndex = 0;
    private List<Vector3> nodes;
    private bool isWalking = false;
    private ushort lastSpeed = 150;
    private Vector3 lastPosition;

    private NetworkClient NetworkClient;
    private GameManager GameManager;
    private PathFinder PathFinder;

    private void Awake() {
        NetworkClient = FindObjectOfType<NetworkClient>();
        GameManager = FindObjectOfType<GameManager>();
        PathFinder = FindObjectOfType<PathFinder>();
        Entity = GetComponent<Entity>();

        if (Entity.HasAuthority) {
            NetworkClient.HookPacket(ZC.NOTIFY_PLAYERMOVE.HEADER, OnPlayerMovement); //Our movement
        }
    }

    private void Update() {
        if (PathFinder == null) {
            PathFinder = FindObjectOfType<PathFinder>();
            return;
        }
        if (isWalking && !nodes.IsEmpty()) {
            bool isEnd = false;
            while (_tick <= GameManager.Tick) {
                var current = nodes[nodeIndex];
                if (nodeIndex == nodes.Count - 1) {
                    isEnd = true;
                    break;
                }
                var next = nodes[nodeIndex + 1];
                var isDiagonal = PathFinder.IsDiagonal(next, current);
                lastSpeed = (ushort) (isDiagonal ? Entity.GetBaseStatus().walkSpeed * 14 / 10 : Entity.GetBaseStatus().walkSpeed); //Diagonal walking is slower
                _tick += lastSpeed;
                nodeIndex++;
            }

            var currentNode = nodeIndex == 0 ? lastPosition : nodes[nodeIndex - 1];
            var nextNode = nodes[nodeIndex];
            var direction = nextNode - currentNode;
            float timeDelta = 1 - Math.Max(_tick - GameManager.Tick, 0f) / lastSpeed;

            transform.position = currentNode + direction * timeDelta;
            Entity.Direction = PathFinder.GetDirectionForOffset(nextNode, currentNode);

            if (isEnd) {
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
        if (!Entity)
            return;
        if (packet is ZC.NOTIFY_PLAYERMOVE) {
            var pkt = packet as ZC.NOTIFY_PLAYERMOVE;

            StartMoving(pkt.StartPosition[0], pkt.StartPosition[1], pkt.EndPosition[0], pkt.EndPosition[1]);
        }
    }

    public void StartMoving(int startX, int startY, int endX, int endY) {
        if (PathFinder == null) {
            PathFinder = FindObjectOfType<PathFinder>();
        }

        _tick = GameManager.Tick;
        nodeIndex = 0;
        nodes = PathFinder.GetPath(startX, startY, endX, endY).Select(node => new Vector3(node.x, (float) node.y, node.z)).ToList();

        if (!nodes.IsEmpty()) {
            Entity.ChangeMotion(new MotionRequest { Motion = SpriteMotion.Walk });
            isWalking = true;
        }

        lastPosition = transform.position;
    }

    public void StopMoving() {
        Entity.ChangeMotion(new MotionRequest { Motion = SpriteMotion.Idle });
        isWalking = false;
        nodes?.Clear();
    }

    private IEnumerator OnWalkEnd() {
        StopMoving();
        yield return new WaitForEndOfFrame();
        Entity.AfterMoveAction?.Invoke();
        Entity.AfterMoveAction = null;

        yield return null;
    }

    public void RequestMove(int x, int y, int dir) {
        /**
         * Validate things such as if entity is sit, whatever
         */
        if (GameManager.OfflineOnly) {
            StartMoving((int) transform.position.x, (int) transform.position.z, x, y);
        } else {
            new CZ.REQUEST_MOVE2(x, y, dir).Send();
        }
    }
}
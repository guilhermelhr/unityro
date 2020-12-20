using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityWalk : MonoBehaviour {

    private Entity Entity;
    private Coroutine MoveIE, MoveToIE;

    private void Awake() {
        Entity = GetComponent<Entity>();

        if (Entity.HasAuthority) {
            Core.NetworkClient.HookPacket(ZC.NOTIFY_PLAYERMOVE.HEADER, OnPlayerMovement); //Our movement
            Core.NetworkClient.HookPacket(ZC.STOPMOVE.HEADER, OnPlayerMovement);
        }
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && Entity.HasAuthority) {
            var ray = Core.MainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 150)) {
                RequestMove(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.z), 0);
            }
        }
    }

    private void OnDisable() {
        if (MoveIE != null) {
            StopCoroutine(MoveIE);
        }
        if (MoveToIE != null) {
            StopCoroutine(MoveToIE);
        }
    }

    /**
     * Server has acknowledged our request and set data back to us
     */
    private void OnPlayerMovement(ushort cmd, int size, InPacket packet) {
        if (!Entity) return;
        if (packet is ZC.NOTIFY_PLAYERMOVE) {
            Entity.ChangeMotion(SpriteMotion.Walk);
            var pkt = packet as ZC.NOTIFY_PLAYERMOVE;

            StartMoving(pkt.startPosition[0], pkt.startPosition[1], pkt.endPosition[0], pkt.endPosition[1]);
        } else if (packet is ZC.STOPMOVE) {
            Entity.ChangeMotion(SpriteMotion.Walk);
            var pkt = packet as ZC.STOPMOVE;
            StartMoving((int)transform.position.x, (int)transform.position.z, pkt.PosX, pkt.PosY);
        }
    }

    public void StartMoving(int startX, int startY, int endX, int endY) {
        var path = Core.PathFinding.GetPath(startX, startY, endX, endY);

        if (MoveIE != null) {
            StopCoroutine(MoveIE);
        }
        if (MoveToIE != null) {
            StopCoroutine(MoveToIE);
        }
        MoveIE = StartCoroutine(Move(path));
    }

    public void StopMoving() {
        if (MoveIE != null) {
            StopCoroutine(MoveIE);
        }
        if (MoveToIE != null) {
            StopCoroutine(MoveToIE);
        }
        Entity.ChangeMotion(SpriteMotion.Idle);
    }

    private IEnumerator OnWalkEnd() {
        Entity.ChangeMotion(SpriteMotion.Idle);
        yield return new WaitForEndOfFrame();
        Entity.MoveAction?.Send();
        Entity.MoveAction = null;

        yield return null;
    }

    IEnumerator Move(List<PathNode> path) {
        var linkedList = new LinkedList<PathNode>(path);

        foreach (var node in linkedList) {
            var next = linkedList.Find(node).Next?.Value;
            if (next != null) {
                var offset = new Vector2Int(next.x, next.z) - new Vector2Int(node.x, node.z);
                Entity.Direction = PathFindingManager.GetDirectionForOffset(offset);
            }

            MoveToIE = StartCoroutine(MoveTo(node));
            yield return MoveToIE;
        }

        yield return OnWalkEnd();
    }

    IEnumerator MoveTo(PathNode node) {
        var destination = new Vector3(node.x, (float)node.y, node.z);
        while (transform.position != destination) {
            transform.position = Vector3.MoveTowards(transform.position, destination, Entity.WalkSpeed * Time.deltaTime / 20);
            yield return null;
        }
    }

    public void RequestMove(int x, int y, int dir) {
        /**
         * Validate things such as if entity is sit, whatever
         */
        if (Core.Instance.Offline) {
            Entity.ChangeMotion(SpriteMotion.Walk);
            StartMoving((int)transform.position.x, (int)transform.position.z, x, y);
        } else {
            new CZ.REQUEST_MOVE2(x, y, dir).Send();
        }
    }
}
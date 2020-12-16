using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityWalk : MonoBehaviour {

    private Entity Entity;
    private Coroutine MoveIE, MoveToIE;
    private int speed = 150;

    private void Awake() {
        Entity = GetComponent<Entity>();

        if(Entity.HasAuthority)
            Core.NetworkClient.HookPacket(ZC.NOTIFY_PLAYERMOVE.HEADER, OnPlayerMovement); //Our movement
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0) && Entity.HasAuthority) {
            var ray = Core.MainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out var hit, 150)) {
                RequestMove(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.z), 0);
            }
        }
    }

    /**
     * Server has acknowledged our request and set data back to us
     */
    private void OnPlayerMovement(ushort cmd, int size, InPacket packet) {
        if(!Entity) return;
        if(packet is ZC.NOTIFY_PLAYERMOVE) {
            Entity.SetAction(SpriteMotion.Walk);
            var pkt = packet as ZC.NOTIFY_PLAYERMOVE;

            StartMoving(pkt.startPosition[0], pkt.startPosition[1], pkt.endPosition[0], pkt.endPosition[1]);
        }
    }

    public void StartMoving(int startX, int startY, int endX, int endY) {
        var path = Core.PathFinding.GetPath(startX, startY, endX, endY);

        if(MoveIE != null) {
            Core.Instance.StopCoroutine(MoveIE);
        }
        if(MoveToIE != null) {
            Core.Instance.StopCoroutine(MoveToIE);
        }
        MoveIE = Core.Instance.StartCoroutine(Move(path));
    }

    IEnumerator Move(List<PathNode> path) {
        var linkedList = new LinkedList<PathNode>(path);

       foreach(var node in linkedList) {
            var next = linkedList.Find(node).Next?.Value;
            if (next != null) {
                var offset = new Vector2Int(next.x, next.z) - new Vector2Int(node.x, node.z);
                Entity.Direction = GetDirectionForOffset(offset);
            }

            MoveToIE = Core.Instance.StartCoroutine(MoveTo(node));
            yield return MoveToIE;
        }

        Entity.SetAction(SpriteMotion.Idle);
    }

    IEnumerator MoveTo(PathNode node) {
        var destination = new Vector3(node.x, (float)node.y, node.z);
        while(transform.position != destination) {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime / 20);
            yield return null;
        }
    }

    public void RequestMove(int x, int y, int dir) {
        /**
         * Validate things such as if entity is sit, whatever
         */
        new CZ.REQUEST_MOVE2(x, y, dir).Send();
    }

    private bool IsNeighbor(Vector2Int pos1, Vector2Int pos2) {
        var x = Mathf.Abs(pos1.x - pos2.x);
        var y = Mathf.Abs(pos1.y - pos2.y);

        if(x <= 1 && y <= 1)
            return true;
        return false;
    }

    private Direction GetDirectionForOffset(Vector2Int offset) {

        if(offset.x == -1 && offset.y == -1) return Direction.SouthWest;
        if(offset.x == -1 && offset.y == 0) return Direction.West;
        if(offset.x == -1 && offset.y == 1) return Direction.NorthWest;
        if(offset.x == 0 && offset.y == 1) return Direction.North;
        if(offset.x == 1 && offset.y == 1) return Direction.NorthEast;
        if(offset.x == 1 && offset.y == 0) return Direction.East;
        if(offset.x == 1 && offset.y == -1) return Direction.SouthEast;
        if(offset.x == 0 && offset.y == -1) return Direction.South;

        return Direction.South;
    }

    private bool IsDiagonal(Direction dir) {
        if(dir == Direction.NorthEast || dir == Direction.NorthWest ||
            dir == Direction.SouthEast || dir == Direction.SouthWest)
            return true;
        return false;
    }
}
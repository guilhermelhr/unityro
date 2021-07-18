using ROIO.Models.FileTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingManager {

    public struct PathRequest {
        public Vector2Int from;
        public Vector2Int to;
    }

    enum eDirection {
        DIR_NORTH = 1,
        DIR_WEST = 2,
        DIR_SOUTH = 4,
        DIR_EAST = 8
    }

    private const int COST_STRAIGHT_MOVE = 10;
    private const int COST_DIAGONAL_MOVE = 14;
    private const int QUADS_FOR_PATH = 32 * 32;

    public Altitude Altitude { get; private set; }

    private List<PathNode> mapNodes;
    private List<PathNode> openSet = new List<PathNode>();
    private HashSet<PathNode> closedSet = new HashSet<PathNode>();
    private List<PathNode> finalPath = new List<PathNode>();

    private int gridX => (int)Altitude.getWidth();
    private int gridY => (int)Altitude.getHeight();

    public bool LoadMap(Altitude altitude) {
        if (altitude != null && altitude != this.Altitude) {
            this.Altitude = altitude;
            this.mapNodes = this.Altitude.GetNodes();
            return true;
        } else if (altitude == this.Altitude) {
            return true;
        } else {
            return false;
        }
    }

    public List<PathNode> GetPath(Vector3 startPosition, Vector3 endPosition, int attackRange = 0) {
        var newRequest = new PathRequest() {
            from = new Vector2Int((int)startPosition.x, (int)startPosition.z),
            to = new Vector2Int((int)endPosition.x, (int)endPosition.z)
        };

        List<PathNode> path = FindPath(newRequest, attackRange);
        return path;
    }

    public float GetCellHeight(int x, int y) {
        return (float)Altitude.GetCellHeight(x, y);
    }

    public List<PathNode> GetPath(int startX, int startY, int endX, int endY, int range = 0) {
        if (startX == endX && startY == endY) {
            return new List<PathNode>();
        }

        var newRequest = new PathRequest() {
            from = new Vector2Int(startX, startY),
            to = new Vector2Int(endX, endY)
        };

        List<PathNode> path = FindPath(newRequest, range);
        return path;
    }

    public void DebugNodes() {
        if (this.mapNodes == null) return;
        foreach (var node in this.mapNodes) {
            var origin = new Vector3(node.x, (float)Altitude.GetCellHeight(node.x, node.z), node.z);
            Gizmos.color = node.walkable ? Color.green : Color.black;
            Gizmos.DrawCube(origin, new Vector3(1, 1, 1));
        }

    }

    public bool IsWalkable(float x, float y) {
        return Altitude.IsCellWalkable((int)Math.Floor(x), (int)Math.Floor(y));
    }

    public GAT.Cell GetCell(float x, float y) {
        return Altitude.GetCell(x, y);
    }

    private List<PathNode> FindPath(PathRequest pr, int range = 0) {
        finalPath.Clear();
        openSet.Clear();
        closedSet.Clear();

        var startNode = mapNodes[pr.from.x + (pr.from.y * gridX)];
        var endNode = mapNodes[pr.to.x + (pr.to.y * gridX)];

        List<PathNode> newPath = new List<PathNode>();

        /**
         * Don't spend resources if either start or end
         * nodes are not walkable.
         */
        if (!endNode.walkable || !startNode.walkable) {
            return newPath;
        }

        openSet.Add(startNode);

        int x, y, xs, ys;
        xs = gridX - 1;
        ys = gridY - 1;

        while (openSet.Count > 0) {
            var currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++) {
                var node = openSet[i];
                if (node.fCost < currentNode.fCost || node.fCost == currentNode.fCost && node.hCost < currentNode.hCost) {
                    currentNode = node;
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endNode) {
                newPath = ReversePath(startNode, endNode);
                int idx = -1;
                if (range > 0) {
                    for (int i = 0; i < newPath.Count; i++) {
                        if (Math.Abs(newPath[i].x - endNode.x) <= range && Math.Abs(newPath[i].z - endNode.z) <= range) {
                            idx = i;
                            break;
                        }
                    }
                }

                if (idx != -1) {
                    newPath.RemoveRange(idx + 1, newPath.Count - (idx + 1));
                }

                return newPath;
            }

            x = currentNode.x;
            y = currentNode.z;
            int gCost = currentNode.gCost;

            // rAthena neighbour
            int allowed_directions = 0;

            if (y < ys && Altitude.IsCellWalkable(x, y + 1)) allowed_directions |= (int)eDirection.DIR_NORTH;
            if (y > 0 && Altitude.IsCellWalkable(x, y - 1)) allowed_directions |= (int)eDirection.DIR_SOUTH;
            if (x < xs && Altitude.IsCellWalkable(x + 1, y)) allowed_directions |= (int)eDirection.DIR_EAST;
            if (x > 0 && Altitude.IsCellWalkable(x - 1, y)) allowed_directions |= (int)eDirection.DIR_WEST;

            if (CheckDirection(allowed_directions, ((int)eDirection.DIR_SOUTH | (int)eDirection.DIR_WEST)) && Altitude.IsCellWalkable(x - 1, y - 1))
                ProcessNode(x - 1, y - 1, gCost + COST_DIAGONAL_MOVE, currentNode, endNode);

            if (CheckDirection(allowed_directions, ((int)eDirection.DIR_WEST)))
                ProcessNode(x - 1, y, gCost + COST_STRAIGHT_MOVE, currentNode, endNode);

            if (CheckDirection(allowed_directions, ((int)eDirection.DIR_NORTH | (int)eDirection.DIR_WEST)) && Altitude.IsCellWalkable(x - 1, y + 1))
                ProcessNode(x - 1, y + 1, gCost + COST_DIAGONAL_MOVE, currentNode, endNode);

            if (CheckDirection(allowed_directions, ((int)eDirection.DIR_NORTH)))
                ProcessNode(x, y + 1, gCost + COST_STRAIGHT_MOVE, currentNode, endNode);

            if (CheckDirection(allowed_directions, ((int)eDirection.DIR_NORTH | (int)eDirection.DIR_EAST)) && Altitude.IsCellWalkable(x + 1, y + 1))
                ProcessNode(x + 1, y + 1, gCost + COST_DIAGONAL_MOVE, currentNode, endNode);

            if (CheckDirection(allowed_directions, ((int)eDirection.DIR_EAST)))
                ProcessNode(x + 1, y, gCost + COST_STRAIGHT_MOVE, currentNode, endNode);

            if (CheckDirection(allowed_directions, ((int)eDirection.DIR_SOUTH | (int)eDirection.DIR_EAST)) && Altitude.IsCellWalkable(x + 1, y - 1))
                ProcessNode(x + 1, y - 1, gCost + COST_DIAGONAL_MOVE, currentNode, endNode);

            if (CheckDirection(allowed_directions, ((int)eDirection.DIR_SOUTH)))
                ProcessNode(x, y - 1, gCost + COST_STRAIGHT_MOVE, currentNode, endNode);
        }

        return newPath;
    }

    private void ProcessNode(int x, int y, int g_cost, PathNode parentNode, PathNode endNode) {
        int idx = x + (y * gridX);
        PathNode currentNode = mapNodes[idx];
        int h_cost = GetHeuristic(x, y, endNode);

        if (closedSet.Contains(currentNode)) return;
        if (openSet.Contains(currentNode)) {
            if (g_cost < currentNode.gCost) {
                currentNode.gCost = g_cost;
                currentNode.hCost = h_cost;
                currentNode.parentNode = parentNode;
                openSet.Add(currentNode);
            }

            return;
        }

        currentNode.gCost = g_cost;
        currentNode.hCost = h_cost;
        currentNode.parentNode = parentNode;
        openSet.Add(currentNode);
    }

    private bool CheckDirection(int dir, int bitmask) => (dir & bitmask) == bitmask;

    private int GetHeuristic(int x0, int y0, PathNode endNode) => COST_STRAIGHT_MOVE * (Mathf.Abs(x0 - endNode.x) + Mathf.Abs(y0 - endNode.z));

    private List<PathNode> ReversePath(PathNode startNode, PathNode endNode) {
        var path = new List<PathNode>();
        var currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }

        path.Add(startNode);
        path.Reverse();

        return path;
    }

    public static bool IsNeighbor(Vector2Int pos1, Vector2Int pos2) {
        var x = Mathf.Abs(pos1.x - pos2.x);
        var y = Mathf.Abs(pos1.y - pos2.y);

        if (x <= 1 && y <= 1)
            return true;
        return false;
    }

    public static Direction GetDirectionForOffset(Vector3 v1, Vector3 v2) {
        return GetDirectionForOffset(new Vector2Int((int)v1.x, (int)v1.z) - new Vector2Int((int)v2.x, (int)v2.z));
    }

    public static Direction GetDirectionForOffset(Vector2Int offset) {

        if (offset.x == -1 && offset.y == -1) return Direction.SouthWest;
        if (offset.x == -1 && offset.y == 0) return Direction.West;
        if (offset.x == -1 && offset.y == 1) return Direction.NorthWest;
        if (offset.x == 0 && offset.y == 1) return Direction.North;
        if (offset.x == 1 && offset.y == 1) return Direction.NorthEast;
        if (offset.x == 1 && offset.y == 0) return Direction.East;
        if (offset.x == 1 && offset.y == -1) return Direction.SouthEast;
        if (offset.x == 0 && offset.y == -1) return Direction.South;

        return Direction.South;
    }

    public static bool IsDiagonal(Vector3 v1, Vector3 v2) {
        return IsDiagonal(GetDirectionForOffset(v1, v2));
    }

    public static bool IsDiagonal(Direction dir) {
        if (dir == Direction.NorthEast || dir == Direction.NorthWest ||
            dir == Direction.SouthEast || dir == Direction.SouthWest)
            return true;
        return false;
    }
}
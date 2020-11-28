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

    public List<PathNode> GetPath(int x0, int y0, int x1, int y1, int range = 0) {
        var newRequest = new PathRequest() {
            from = new Vector2Int(x0, y0),
            to = new Vector2Int(x1, y1)
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

            if (y < ys && CheckWalkable(x, y + 1)) allowed_directions |= (int)eDirection.DIR_NORTH;
            if (y > 0 && CheckWalkable(x, y - 1)) allowed_directions |= (int)eDirection.DIR_SOUTH;
            if (x < xs && CheckWalkable(x + 1, y)) allowed_directions |= (int)eDirection.DIR_EAST;
            if (x > 0 && CheckWalkable(x - 1, y)) allowed_directions |= (int)eDirection.DIR_WEST;

            if (CheckDirection(allowed_directions, ((int)eDirection.DIR_SOUTH | (int)eDirection.DIR_WEST)) && CheckWalkable(x - 1, y - 1))
                ProcessNode(x - 1, y - 1, gCost + COST_DIAGONAL_MOVE, currentNode, endNode);

            if (CheckDirection(allowed_directions, ((int)eDirection.DIR_WEST)))
                ProcessNode(x - 1, y, gCost + COST_STRAIGHT_MOVE, currentNode, endNode);

            if (CheckDirection(allowed_directions, ((int)eDirection.DIR_NORTH | (int)eDirection.DIR_WEST)) && CheckWalkable(x - 1, y + 1))
                ProcessNode(x - 1, y + 1, gCost + COST_DIAGONAL_MOVE, currentNode, endNode);

            if (CheckDirection(allowed_directions, ((int)eDirection.DIR_NORTH)))
                ProcessNode(x, y + 1, gCost + COST_STRAIGHT_MOVE, currentNode, endNode);

            if (CheckDirection(allowed_directions, ((int)eDirection.DIR_NORTH | (int)eDirection.DIR_EAST)) && CheckWalkable(x + 1, y + 1))
                ProcessNode(x + 1, y + 1, gCost + COST_DIAGONAL_MOVE, currentNode, endNode);

            if (CheckDirection(allowed_directions, ((int)eDirection.DIR_EAST)))
                ProcessNode(x + 1, y, gCost + COST_STRAIGHT_MOVE, currentNode, endNode);

            if (CheckDirection(allowed_directions, ((int)eDirection.DIR_SOUTH | (int)eDirection.DIR_EAST)) && CheckWalkable(x + 1, y - 1))
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

    private bool CheckWalkable(int x, int y) {
        int idx = x + (y * gridX);
        if (idx > mapNodes.Count - 1) {
            Debug.LogWarning("Break here");
        }

        return mapNodes[idx].walkable;
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
}
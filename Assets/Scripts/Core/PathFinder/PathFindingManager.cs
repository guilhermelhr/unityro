using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingManager {

    public struct PathRequest {
        public int GID;
        public Vector2Int from;
        public Vector2Int to;
        public Action<int, List<PathNode>> callback;
    }

    enum eDirection {
        DIR_NORTH = 1,
        DIR_WEST = 2,
        DIR_SOUTH = 4,
        DIR_EAST = 8
    }

    const int MOVE_COST = 10;
    const int MOVE_DIAGONAL = 14;
    const int QUADS_FOR_PATH = 32 * 32;

    private GAT map;
    private List<PathNode> mapNodes;
    private List<PathNode> openSet = new List<PathNode>();
    private HashSet<PathNode> closedSet = new HashSet<PathNode>();
    private List<PathNode> finalPath = new List<PathNode>();

    private Queue<PathRequest> requestQueue = new Queue<PathRequest>();

    private bool readyToLoad = false;
    private bool coroutineRunning = false;
    private float nextCheck = 0.0f;
    private int gridX => (int)map.width;
    private int gridY => (int)map.height;

    public void Tick(float dt) {
        if ((nextCheck -= dt) < 0) {
            if (requestQueue.Count > 0 && !coroutineRunning) {
                PathRequest temp = requestQueue.Dequeue();
                Core.Instance.StartCoroutine(FindPath(temp));
            }
        }
    }

    public bool LoadMap(GAT map) {
        if (map != null && map != this.map) {
            this.map = map;
            GenerateNodes();
            readyToLoad = true;
            return true;
        } else if (map == this.map) {
            return true;
        } else {
            readyToLoad = false;
            return false;
        }
    }

    private void GenerateNodes() {
        mapNodes = new List<PathNode>();

        for (int y = 0; y < gridY; y++) {
            for (int x = 0; x < gridX; x++) {
                var newNode = new PathNode() {
                    x = x,
                    y = y,
                    walkable = map.cells[x + (y * gridX)].type == (byte)GAT.TYPE.WALKABLE ? true : false
                };
                mapNodes.Add(newNode);
            }
        }
    }

    public List<PathNode> GetPath(int GID, int x0, int y0, int x1, int y1, int range = 0) {
        var newRequest = new PathRequest() {
            GID = GID,
            from = new Vector2Int(x0, y0),
            to = new Vector2Int(x1, y1)
        };

        List<PathNode> path = NonCRFindPath(newRequest, range);
        return path;
    }

    public void RequestPath(int GID, int x0, int y0, int x1, int y1, Action<int, List<PathNode>> callback, int range = 0) {
        if (GID < 0) {
            Debug.LogError("PathfindingManager: Something went wrong with the RequestPath function");
            return;
        }

        if (!readyToLoad || mapNodes == null) {
            Debug.LogError("PathfindingManager: The map is null, cannot request a path");
            return;
        }

        var newRequest = new PathRequest() {
            GID = GID,
            from = new Vector2Int(x0, y0),
            to = new Vector2Int(x1, y1),
            callback = callback,
        };

        requestQueue.Enqueue(newRequest);
    }

    private List<Vector2Int> RequestPath(int x0, int y0, int x1, int y1) {
        var newPath = new List<Vector2Int>();

        if (mapNodes == null) {
            Debug.LogError("PathfindingManager: The map is null, cannot request a path");
            return null;
        }


        return newPath;
    }

    private List<PathNode> NonCRFindPath(PathRequest pr, int range = 0) {
        finalPath.Clear();
        openSet.Clear();
        closedSet.Clear();

        var startNode = mapNodes[pr.from.x + (pr.from.y * gridX)];
        var endNode = mapNodes[pr.to.x + (pr.to.y * gridX)];

        List<PathNode> newPath = new List<PathNode>();

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
                newPath = ReTracePath(startNode, endNode);
                int idx = -1;
                if (range > 0) {
                    for (int i = 0; i < newPath.Count; i++) {
                        if (Math.Abs(newPath[i].x - endNode.x) <= range && Math.Abs(newPath[i].y - endNode.y) <= range) {
                            idx = i;
                            break;
                        }
                    }
                }

                if (idx != -1) {
                    //Debug.Log("Break here");
                    newPath.RemoveRange(idx + 1, newPath.Count - (idx + 1));
                }

                // do the callback i guess
                //pr.callback(pr.GID, new_path);
                //cr_running = false;
                return newPath;
            }

            x = currentNode.x;
            y = currentNode.y;
            int gCost = currentNode.gCost;

            // do the reathena neighbour shit in here :>
            int allowed_directions = 0;

            if (y < ys && CheckWalkable(x, y + 1)) allowed_directions |= (int)eDirection.DIR_NORTH;
            if (y > 0 && CheckWalkable(x, y - 1)) allowed_directions |= (int)eDirection.DIR_SOUTH;
            if (x < xs && CheckWalkable(x + 1, y)) allowed_directions |= (int)eDirection.DIR_EAST;
            if (x > 0 && CheckWalkable(x - 1, y)) allowed_directions |= (int)eDirection.DIR_WEST;

            if (CheckDirection(allowed_directions, ((int)eDirection.DIR_SOUTH | (int)eDirection.DIR_WEST)) && CheckWalkable(x - 1, y - 1))
                ProcessNode(openSet, x - 1, y - 1, gCost + MOVE_DIAGONAL, currentNode, GetHeuristic(x - 1, y - 1, endNode.x, endNode.y));

            if (CheckDirection(allowed_directions, ((int)eDirection.DIR_WEST)))
                ProcessNode(openSet, x - 1, y, gCost + MOVE_COST, currentNode, GetHeuristic(x - 1, y, endNode.x, endNode.y));

            if (CheckDirection(allowed_directions, ((int)eDirection.DIR_NORTH | (int)eDirection.DIR_WEST)) && CheckWalkable(x - 1, y + 1))
                ProcessNode(openSet, x - 1, y + 1, gCost + MOVE_DIAGONAL, currentNode, GetHeuristic(x - 1, y + 1, endNode.x, endNode.y));

            if (CheckDirection(allowed_directions, ((int)eDirection.DIR_NORTH)))
                ProcessNode(openSet, x, y + 1, gCost + MOVE_COST, currentNode, GetHeuristic(x, y + 1, endNode.x, endNode.y));

            if (CheckDirection(allowed_directions, ((int)eDirection.DIR_NORTH | (int)eDirection.DIR_EAST)) && CheckWalkable(x + 1, y + 1))
                ProcessNode(openSet, x + 1, y + 1, gCost + MOVE_DIAGONAL, currentNode, GetHeuristic(x + 1, y + 1, endNode.x, endNode.y));

            if (CheckDirection(allowed_directions, ((int)eDirection.DIR_EAST)))
                ProcessNode(openSet, x + 1, y, gCost + MOVE_COST, currentNode, GetHeuristic(x + 1, y, endNode.x, endNode.y));

            if (CheckDirection(allowed_directions, ((int)eDirection.DIR_SOUTH | (int)eDirection.DIR_EAST)) && CheckWalkable(x + 1, y - 1))
                ProcessNode(openSet, x + 1, y - 1, gCost + MOVE_DIAGONAL, currentNode, GetHeuristic(x + 1, y - 1, endNode.x, endNode.y));

            if (CheckDirection(allowed_directions, ((int)eDirection.DIR_SOUTH)))
                ProcessNode(openSet, x, y - 1, gCost + MOVE_COST, currentNode, GetHeuristic(x, y - 1, endNode.x, endNode.y));
        }

        return newPath;
    }

    private void ProcessNode(List<PathNode> openSet, int x, int y, int g_cost, PathNode parentNode, int h_cost) {
        int idx = x + (y * gridX);
        PathNode currentNode = mapNodes[idx];

        if (closedSet.Contains(currentNode)) return;
        if (openSet.Contains(currentNode)) {
            if (g_cost < currentNode.gCost) {
                currentNode.gCost = g_cost;
                currentNode.hCost = h_cost;
                currentNode.parent = parentNode;
                openSet.Add(currentNode);
            }

            return;
        }

        currentNode.gCost = g_cost;
        currentNode.hCost = h_cost;
        currentNode.parent = parentNode;
        openSet.Add(currentNode);
    }

    IEnumerator FindPatch(PathRequest pr) {
        Debug.Log("fock off");
        yield return new WaitForEndOfFrame();
    }

    IEnumerator FindPath(PathRequest pr) {
        coroutineRunning = true;
        NonCRFindPath(pr);
        coroutineRunning = false;
        yield return null;
    }

    private bool CheckWalkable(int x, int y) {
        int idx = x + (y * gridX);
        if (idx > mapNodes.Count - 1) {
            Debug.LogWarning("Break here");
        }

        return mapNodes[idx].walkable;
    }

    private bool CheckDirection(int dir, int bitmask) => (dir & bitmask) == bitmask;

    public int GetHeuristic(int x0, int y0, int x1, int y1) => MOVE_COST * (Mathf.Abs(x0 - x1) + Mathf.Abs(y0 - y1));

    private List<PathNode> ReTracePath(PathNode startNode, PathNode endNode) {
        var path = new List<PathNode>();
        var currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Add(startNode);
        path.Reverse();

        return path;
    }
}
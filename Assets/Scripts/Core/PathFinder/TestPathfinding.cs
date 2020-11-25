using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TestPathfinding {

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public static TestPathfinding Instance { get; private set; }

    private GAT grid;

    private List<PathNode> openList;
    private List<PathNode> closedList;

    public TestPathfinding() {
        Instance = this;
    }

    public void LoadMap(GAT gat) {
        this.grid = gat;
    }

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition) {
        //grid.GetXY(startWorldPosition, out int startX, out int startY);
        //grid.GetXY(endWorldPosition, out int endX, out int endY);

        //var path = FindPath(startX, startY, endX, endY);
        //if (path != null) {
        //    return null;
        //}

        //var vectorPath = new List<Vector3>();
        //foreach (var pathNode in path) {
        //    vectorPath.Add(new Vector3(pathNode.x, pathNode.y) * grid.get);
        //}

        //return vectorPath;
        return null;
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY) {
        var startNode = grid.GetPathNode(startX, startY);
        var endNode = grid.GetPathNode(endX, endY);

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for (int x = 0; x < grid.width; x++) {
            for (int y = 0; y < grid.height; y++) {
                var pathNode = grid.GetPathNode(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.parentNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);

        while (openList.Count > 0) {
            var currentNode = GetLowestFCostNode(openList);

            if (currentNode == endNode) {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (var neighbourNode in GetNeighbourList(currentNode)) {
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.walkable) {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost) {
                    neighbourNode.parentNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);

                    if (!openList.Contains(neighbourNode)) {
                        openList.Add(neighbourNode);
                    }
                }
            }

            return null;
        }

        return null;
    }

    private List<PathNode> CalculatePath(PathNode endNode) {
        var path = new List<PathNode>();
        path.Add(endNode);
        var currentNode = endNode;

        while(currentNode.parentNode != null) {
            path.Add(currentNode.parentNode);
            currentNode = currentNode.parentNode;
        }

        path.Reverse();

        return path;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode) {
        var neighbourList = new List<PathNode>();

        if (currentNode.x - 1 >= 0) {
            // Left
            neighbourList.Add(grid.GetPathNode(currentNode.x - 1, currentNode.y));
            // Left Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(grid.GetPathNode(currentNode.x - 1, currentNode.y - 1));
            // Left Up
            if (currentNode.y + 1 < grid.height) neighbourList.Add(grid.GetPathNode(currentNode.x - 1, currentNode.y + 1));
        }

        if (currentNode.x + 1 < grid.width) {
            // Right
            neighbourList.Add(grid.GetPathNode(currentNode.x + 1, currentNode.y));
            // Right Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(grid.GetPathNode(currentNode.x + 1, currentNode.y - 1));
            // Right Up
            if (currentNode.y + 1 < grid.height) neighbourList.Add(grid.GetPathNode(currentNode.x + 1, currentNode.y + 1));
        }

        // Down
        if (currentNode.y - 1 >= 0) neighbourList.Add(grid.GetPathNode(currentNode.x, currentNode.y - 1));
        // Up
        if (currentNode.y + 1 < grid.height) neighbourList.Add(grid.GetPathNode(currentNode.x, currentNode.y + 1));

        return neighbourList;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b) {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList) {
        var lowestFCostNode = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++) {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost) {
                lowestFCostNode = pathNodeList[i];
            }
        }

        return lowestFCostNode;
    }

}
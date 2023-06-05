using System;
using System.Collections.Generic;
using UnityRO.Core.Extensions;
using Priority_Queue;
using ROIO.Models.FileTypes;
using UnityEngine;

namespace Core.Path {
    public class PathFinder : MonoBehaviour {
        const int MAX_PATHNODE = 150;

        private int m_destX;
        private int m_destY;

        private int m_poolCount;
        private PathNode[] m_nodePool = new PathNode[MAX_PATHNODE];

        private Altitude? Altitude;

        private Dictionary<int, PathNode> m_masterNodes = new(); //	hash value = int:x*width+y

        private SimplePriorityQueue<PathNode> m_openNodes = new();

        public PathFinder() {
            for (var i = 0; i < MAX_PATHNODE; i++) {
                m_nodePool[i] = new PathNode();
            }
        }

        public int Width => (int)(Altitude?.getWidth() ?? -1);
        public int Height => (int)(Altitude?.getHeight() ?? -1);

        private void Reset() {
            m_masterNodes.Clear();
            m_openNodes.Clear();
        }

        public void SetMap(Altitude newMap) {
            Altitude = newMap;
        }

        private int GetMapWidth() {
            return (int)Altitude.getWidth();
        }

        private int GetMapHeight() {
            return (int)Altitude.getHeight();
        }

        private PathNode? FindNode(int x, int y) {
            m_masterNodes.TryGetValue(x + y * GetMapWidth(), out var node);
            return node;
        }

        private int GetHeuristicCost(int sx, int sy) {
            return (Math.Abs(sx - m_destX) + Math.Abs(sy - m_destY)) * 10;
        }

        public bool FindPath(long startTime, int sx, int sy, int dx, int dy, int speedFactor, CPathInfo pathInfo) {
            //	Trace("find path start : (%d, %d  xM:%d ,  yM:%d) - (%d, %d)", sx, sy, xM, yM, dx, dy);
            m_destX = dx;
            m_destY = dy;
            if (sx == dx && sy == dy)
                return false;

            //	pretest using simpler method
            //	straight first test
            //	diagonal first test

            pathInfo.ResetStartCell(); /// if start to find path, then pathInfo.startCell is set to zero
            Reset();
            m_poolCount = 0;
            var startNode = GetNode(sx, sy);
            startNode!.Type = PathNode.PathStatus.OPEN;
            startNode.Parent = null;
            startNode.Cost = 0;
            startNode.Total = GetHeuristicCost(sx, sy);
            //m_openNodes.Push(startNode);
            m_openNodes.Enqueue(startNode, startNode.Total);

            //while (!m_openNodes.IsEmpty())
            while (m_openNodes.Count > 0) {
                //var bestNode = m_openNodes.Pop();
                var bestNode = m_openNodes.Dequeue();
                if (bestNode.X == dx && bestNode.Y == dy) {
                    //	goal reached
                    pathInfo.StartPointX = sx;
                    pathInfo.StartPointY = sy;
                    BuildResultPath(startTime, bestNode, speedFactor, pathInfo);
                    return true;
                }

                //	process successor nodes
                bool result;
                if (IsConnected(bestNode.X, bestNode.Y, bestNode.X + 1, bestNode.Y - 1)) {
                    result = ProcessNode(bestNode, 14, bestNode.X + 1, bestNode.Y - 1, 5);
                    if (!result) {
                        //Trace("find path aborted : buffer overflow");
                        return false;
                    }
                }

                if (IsConnected(bestNode.X, bestNode.Y, bestNode.X + 1, bestNode.Y)) {
                    result = ProcessNode(bestNode, 10, bestNode.X + 1, bestNode.Y, 6);
                    if (!result) {
                        //Trace("find path aborted : buffer overflow");
                        return false;
                    }
                }

                if (IsConnected(bestNode.X, bestNode.Y, bestNode.X + 1, bestNode.Y + 1)) {
                    result = ProcessNode(bestNode, 14, bestNode.X + 1, bestNode.Y + 1, 7);
                    if (!result) {
                        //Trace("find path aborted : buffer overflow");
                        return false;
                    }
                }

                if (IsConnected(bestNode.X, bestNode.Y, bestNode.X, bestNode.Y + 1)) {
                    result = ProcessNode(bestNode, 10, bestNode.X, bestNode.Y + 1, 0);
                    if (!result) {
                        //Trace("find path aborted : buffer overflow");
                        return false;
                    }
                }

                if (IsConnected(bestNode.X, bestNode.Y, bestNode.X - 1, bestNode.Y + 1)) {
                    result = ProcessNode(bestNode, 14, bestNode.X - 1, bestNode.Y + 1, 1);
                    if (!result) {
                        //Trace("find path aborted : buffer overflow");
                        return false;
                    }
                }

                if (IsConnected(bestNode.X, bestNode.Y, bestNode.X - 1, bestNode.Y)) {
                    result = ProcessNode(bestNode, 10, bestNode.X - 1, bestNode.Y, 2);
                    if (!result) {
                        //Trace("find path aborted : buffer overflow");
                        return false;
                    }
                }

                if (IsConnected(bestNode.X, bestNode.Y, bestNode.X - 1, bestNode.Y - 1)) {
                    result = ProcessNode(bestNode, 14, bestNode.X - 1, bestNode.Y - 1, 3);
                    if (!result) {
                        //Trace("find path aborted : buffer overflow");
                        return false;
                    }
                }

                if (IsConnected(bestNode.X, bestNode.Y, bestNode.X, bestNode.Y - 1)) {
                    result = ProcessNode(bestNode, 10, bestNode.X, bestNode.Y - 1, 4);
                    if (!result) {
                        //Trace("find path aborted : buffer overflow");
                        return false;
                    }
                }

                bestNode.Type = PathNode.PathStatus.CLOSED;
            }

            //Trace("find path failed : no path found");
            return false; //	no path found
        }

        private bool ProcessNode(PathNode parent, int traverseCost, int x, int y, int dir) {
            int newCost = parent.Cost + traverseCost;
            var node = GetNode(x, y);
            if (node == null)
                return false;

            if (node.Type != PathNode.PathStatus.UNEXPLORED) { //	node already exists
                if (node.Cost <= newCost) { //	ignore this node if shows no improvement
                    return true;
                }

                //	store the new or improved information
                node.Parent = parent;
                node.Cost = newCost;
                node.Total = newCost + GetHeuristicCost(x, y);
                node.Direction = dir;
                if (node.Type == PathNode.PathStatus.OPEN) {
                    //	already in openlist, just update priority
                    m_openNodes.UpdatePriority(node, node.Total);
                } else if (node.Type == PathNode.PathStatus.CLOSED) {
                    node.Type = PathNode.PathStatus.OPEN; //	new path is closer, move newnode from closed to open again
                }
            } else {
                //	unexplored node goes into open list
                node.Type = PathNode.PathStatus.OPEN;
                node.Parent = parent;
                node.Cost = newCost;
                node.Total = newCost + GetHeuristicCost(x, y);
                node.Direction = dir;
                m_openNodes.Enqueue(node, node.Total);
            }

            return true;
        }

        private PathNode? GetNewNode() {
            if (m_poolCount >= MAX_PATHNODE - 1) {
                return null;
            }

            return m_nodePool[m_poolCount++];
        }

        private PathNode? GetNode(int x, int y) {
            var node = FindNode(x, y);

            if (node != null) {
                return node;
            }

            node = GetNewNode();
            if (node == null) {
                return null; //	node pool exhausted
            }

            node.X = x;
            node.Y = y;
            node.Type = PathNode.PathStatus.UNEXPLORED;
            AddNode(node);
            return node;
        }

        private void AddNode(PathNode node) {
            m_masterNodes[node.X + node.Y * GetMapWidth()] = node;
        }

        private long GetSecondNodeArrivalTime(long startTime, CPathInfo path, int speedFactor) {
            var xlen = path.PathData[1].X - path.StartPointX;
            var ylen = path.PathData[1].Y - path.StartPointY;
            var distance = (float)Math.Sqrt(xlen * xlen + ylen * ylen);

            return startTime + (long)(distance * speedFactor);
        }

        private void BuildResultPath(long startTime, PathNode goalNode, int speedFactor, CPathInfo path) {
            //	count path size
            var len = 0;
            PathNode? tempnode = goalNode;

            while (tempnode != null) {
                len++;
                tempnode = tempnode.Parent;
            }

            //	set path size
            path.PathData.Resize(len, default!);
            if (len <= 1)
                return;

            //	fill path info
            tempnode = goalNode;
            int i;
            for (i = 0; i < len; i++) {
                if (path.PathData[len - i - 1] == default) {
                    path.PathData[len - i - 1] = new PathCell();
                }

                path.PathData[len - i - 1].X = tempnode!.X;
                path.PathData[len - i - 1].Y = tempnode.Y;
                path.PathData[len - i - 1].Direction = tempnode.Direction;
                tempnode = tempnode.Parent;
            }

            //	fill pos, dir info
            var diagonalMoveFactor = (int)(speedFactor * 1.414f);
            path.PathData[0].Time = startTime;
            path.PathData[1].Time = GetSecondNodeArrivalTime(startTime, path, speedFactor);

            for (i = 1; i < len - 1; i++) {
                if (path.PathData[i + 1].Direction % 2 == 1) { //	straight line move
                    path.PathData[i + 1].Time = path.PathData[i].Time + diagonalMoveFactor;
                } else { //	diagonal line move
                    path.PathData[i + 1].Time = path.PathData[i].Time + speedFactor;
                }
            }
        }

        private bool IsConnected(int sx, int sy, int dx, int dy) {
            if (dx < 0 || dy < 0 || dx >= GetMapWidth() || dy >= GetMapHeight()) {
                return false;
            }

            if (!Altitude.IsCellWalkable(dx, dy)) {
                return false;
            }

            if (sx == dx || sy == dy) return true;

            return Altitude.IsCellWalkable(sx, dy) && Altitude.IsCellWalkable(dx, sy);
        }

        public static Direction GetDirectionForOffset(Vector3 v1, Vector3 v2) {
            return GetDirectionForOffset(new Vector2Int((int)v1.x, (int)v1.z) - new Vector2Int((int)v2.x, (int)v2.z));
        }

        public static Direction GetDirectionForOffset(Vector2Int offset) {
            if (offset.x <= -1 && offset.y <= -1) return Direction.SouthWest;
            if (offset.x <= -1 && offset.y == 0) return Direction.West;
            if (offset.x <= -1 && offset.y <= 1) return Direction.NorthWest;
            if (offset.x == 0 && offset.y >= 1) return Direction.North;
            if (offset.x >= 1 && offset.y >= 1) return Direction.NorthEast;
            if (offset.x >= 1 && offset.y == 0) return Direction.East;
            if (offset.x >= 1 && offset.y <= -1) return Direction.SouthEast;
            if (offset.x == 0 && offset.y <= -1) return Direction.South;

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

        public float GetCellHeight(int pktPosX, int pktPosY) {
            return (float)Altitude.GetCellHeight(pktPosX, pktPosY);
        }

        public float GetCellHeight(Vector2 pos) {
            return (float)Altitude.GetCellHeight(pos.x, pos.y);
        }

        public GAT.Cell GetCell(Vector2Int tile) {
            return Altitude.GetCell(tile.x, tile.y);
        }
    }
}
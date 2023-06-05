using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Path {
    public class CPathInfo {
        public List<PathCell> PathData { get; set; } = new List<PathCell>();
        public int StartCell { get; set; }
        public float StartPointX { get; set; }
        public float StartPointY { get; set; }

        public void ResetStartCell() {
            StartCell = 0;
        }

        public bool IsInPath(int pos_x, int pos_y) {
            for (int i = 0; i < PathData.Count; i++) {
                if (PathData[i].X == pos_x && PathData[i].Y == pos_y) {
                    return true;
                }
            }

            return false;
        }

        public void SetNewPathInfo(PathCell[] pBuf, int count) {
            int cc = 3;

            if (count < cc) {
                cc = count;
            }

            bool clear_flag = false;

            int j = (int)PathData.Count - 1;
            for (int i = count - 1; (i >= cc && j >= cc); i--, j--) {
                if (pBuf[i].X != PathData[j].X || pBuf[i].Y != PathData[j].Y
                                               || Math.Abs((int)pBuf[j].Time - (int)PathData[j].Time) > 200) {
                    clear_flag = true;
                }
            }

            var clear_time = pBuf[count - cc].Time;

            if (clear_flag) {
                PathData.RemoveAll(x => x.Time > clear_time);

                //m_pathData.clear() ;
                for (int i = 0; i < count; i++) {
                    if (!IsInPath(pBuf[i].X, pBuf[i].Y)) {
                        PathCell temp = new PathCell();
                        temp.X = pBuf[i].X;
                        temp.Y = pBuf[i].Y;
                        temp.Direction = pBuf[i].Direction;
                        temp.Time = pBuf[i].Time;
                        PathData.Add(temp);
                    }
                }
            }
        }

        public void FixPathTime(long dest_arrived_time) {
            if (PathData.Count == 0)
                return;

            if (dest_arrived_time == 0)
                return;

            float sub = (float)((int)dest_arrived_time - (int)(PathData[PathData.Count - 1].Time));

            float sub_div = sub / (float)PathData.Count;
            if (sub_div != 0) {
                for (int i = 0; i < PathData.Count - 1; i++) {
                    PathData[i].Time += (uint)(sub_div * i);
                }
            }

            PathData.Last().Time = dest_arrived_time;
        }

        public bool IsPathWillEnd(long time, int delay) {
            if (PathData.Count == 0)
                return true;

            var sub = PathData[PathData.Count - 1].Time - time;
            if (sub < delay) {
                return true;
            } else {
                return false;
            }
        }

        public int GetPos(int speedFactor, int time, ref float xPos, ref float yPos, ref int dir) {
            if (PathData.Count == 0 || PathData[0].Time > time) {
                xPos = PathData[0].X;
                yPos = PathData[0].Y;
                dir = PathData[1].Direction;
                return 0;
            }

            int index;
            for (index = StartCell; index < PathData.Count - 1; index++) {
                if (PathData[index].Time <= time && time < PathData[index + 1].Time) {
                    var movingTime = time - PathData[index].Time;
                    float xlen, ylen;
                    if (index == 0) {
                        xlen = PathData[1].X - StartPointX;
                        ylen = PathData[1].Y - StartPointY;
                    } else {
                        xlen = PathData[index + 1].X - PathData[index].X;
                        ylen = PathData[index + 1].Y - PathData[index].Y;
                    }

                    float xMovingDistance = xlen * (float)(movingTime) /
                                            (float)(PathData[index + 1].Time - PathData[index].Time);

                    if (index == 0) {
                        xPos = StartPointX + xMovingDistance;
                    } else {
                        xPos = PathData[index].X + xMovingDistance;
                    }

                    float yMovingDistance = ylen * (float)(movingTime) /
                                            (float)(PathData[index + 1].Time - PathData[index].Time);

                    if (index == 0) {
                        yPos = StartPointY + yMovingDistance;
                    } else {
                        yPos = PathData[index].Y + yMovingDistance;
                    }

                    if (time < (PathData[index].Time + (PathData[index + 1].Time - PathData[index].Time) / 2)) {
                        dir = PathData[index].Direction;
                    } else {
                        dir = PathData[index + 1].Direction;
                    }

                    StartCell = index;
                    return index;
                }
            }

            dir = PathData[index].Direction;
            xPos = PathData[index].X;
            yPos = PathData[index].Y;
            return -1;
        }

        public int GetStartCellNumber() {
            return StartCell;
        }

        public void SetStartCellNumber(int num) {
            StartCell = num;
        }

        public float GetStartPointX() {
            return StartPointX;
        }

        float GetStartPointY() {
            return StartPointY;
        }

        public long GetTotalExpectedMovingTime() {
            int size = PathData.Count;
            return PathData[size - 1].Time - PathData[0].Time;
        }

        public void GetLastCellTime(ref long lastCellTime) {
            lastCellTime = PathData[PathData.Count - 1].Time;
        }

        public int GetPrevCellInfo(long currentTime,
            ref long time,
            ref Vector3 position,
            ref int direction,
            Func<Vector2, float> getCellHeight) {
            var x = 0;
            var y = 0;
            
            if (PathData.Count == 0) {
                direction = 0;
                return -1;
            }

            if (PathData[0].Time == 0) {
                direction = 0;
                return -1;
            }

            if (PathData[0].Time > currentTime && PathData.Count > 1) {
                time = PathData[0].Time;
                x = PathData[0].X;
                y = PathData[0].Y;
                position = new Vector3(x, getCellHeight(new Vector2(x, y)), y);
                direction = PathData[0].Direction;
                return 0;
            }

            int index = 0;
            int size = PathData.Count;
            for (; index < size - 1; index++) {
                if (PathData[index].Time <= currentTime && currentTime < PathData[index + 1].Time) {
                    time = PathData[index].Time;
                    x = PathData[index].X;
                    y = PathData[index].Y;
                    position = new Vector3(x, getCellHeight(new Vector2(x, y)), y);
                    direction = PathData[0].Direction;
                    return index;
                }
            }

            time = PathData[index].Time;
            x = PathData[index].X;
            y = PathData[index].Y;
            position = new Vector3(x, getCellHeight(new Vector2(x, y)), y);
            direction = PathData[index].Direction;
            //	Trace(" currentTime:%d, index3:%d, xPos:%d, yPos:%d",currentTime,index+1,x,y);
            return -1;
        }

        public int GetNextCellInfo(long currentTime,
            ref long time,
            ref Vector3 position,
            ref int direction,
            Func<Vector2, float> getCellHeight) {
            var x = 0;
            var y = 0;

            if (PathData.Count == 0) {
                direction = 0;
                return -1;
            }

            if (PathData[0].Time == 0) {
                direction = 0;
                return -1;
            }

            if (PathData[0].Time > currentTime && PathData.Count > 1) {
                x = PathData[1].X;
                y = PathData[1].Y;
                time = PathData[1].Time;
                position = new Vector3(x, getCellHeight(new Vector2(x, y)), y);
                direction = PathData[1].Direction;
                return 0;
            }

            var index = 0;
            var size = PathData.Count;
            for (; index < size - 1; index++) {
                if (PathData[index].Time <= currentTime && currentTime < PathData[index + 1].Time) {
                    time = PathData[index + 1].Time;
                    x = PathData[index + 1].X;
                    y = PathData[index + 1].Y;
                    position = new Vector3(x, getCellHeight(new Vector2(x, y)), y);
                    direction = 0;
                    return index;
                }
            }

            time = PathData[index].Time;
            x = PathData[index].X;
            y = PathData[index].Y;
            position = new Vector3(x, getCellHeight(new Vector2(x, y)), y);
            direction = 0;
            //	Trace(" currentTime:%d, index3:%d, xPos:%d, yPos:%d",currentTime,index+1,x,y);
            return -1;
        }
    }
}
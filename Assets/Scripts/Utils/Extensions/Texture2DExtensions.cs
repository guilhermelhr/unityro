using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Texture2DExtensions {
    public struct Point {
        public short x;
        public short y;
        public Point(short aX, short aY) { x = aX; y = aY; }
        public Point(int aX, int aY) : this((short)aX, (short)aY) { }
    }

    public static void FloodFillArea(this Texture2D aTex, int aX, int aY, Color aFillColor) {
        int w = aTex.width;
        int h = aTex.height;
        Color[] colors = aTex.GetPixels();
        Color refCol = colors[aX + aY * w];
        Queue<Point> nodes = new Queue<Point>();
        nodes.Enqueue(new Point(aX, aY));
        while(nodes.Count > 0) {
            Point current = nodes.Dequeue();
            for(int i = current.x; i < w; i++) {
                Color C = colors[i + current.y * w];
                if(C != refCol || C == aFillColor)
                    break;
                colors[i + current.y * w] = aFillColor;
                if(current.y + 1 < h) {
                    C = colors[i + current.y * w + w];
                    if(C == refCol && C != aFillColor)
                        nodes.Enqueue(new Point(i, current.y + 1));
                }
                if(current.y - 1 >= 0) {
                    C = colors[i + current.y * w - w];
                    if(C == refCol && C != aFillColor)
                        nodes.Enqueue(new Point(i, current.y - 1));
                }
            }
            for(int i = current.x - 1; i >= 0; i--) {
                Color C = colors[i + current.y * w];
                if(C != refCol || C == aFillColor)
                    break;
                colors[i + current.y * w] = aFillColor;
                if(current.y + 1 < h) {
                    C = colors[i + current.y * w + w];
                    if(C == refCol && C != aFillColor)
                        nodes.Enqueue(new Point(i, current.y + 1));
                }
                if(current.y - 1 >= 0) {
                    C = colors[i + current.y * w - w];
                    if(C == refCol && C != aFillColor)
                        nodes.Enqueue(new Point(i, current.y - 1));
                }
            }
        }
        aTex.SetPixels(colors);
    }

    public static void FloodFillBorder(this Texture2D aTex, int aX, int aY, Color aFillColor, Color aBorderColor) {
        int w = aTex.width;
        int h = aTex.height;
        Color[] colors = aTex.GetPixels();
        byte[] checkedPixels = new byte[colors.Length];
        Color refCol = aBorderColor;
        Queue<Point> nodes = new Queue<Point>();
        nodes.Enqueue(new Point(aX, aY));
        while(nodes.Count > 0) {
            Point current = nodes.Dequeue();

            for(int i = current.x; i < w; i++) {
                if(checkedPixels[i + current.y * w] > 0 || colors[i + current.y * w] == refCol)
                    break;
                colors[i + current.y * w] = aFillColor;
                checkedPixels[i + current.y * w] = 1;
                if(current.y + 1 < h) {
                    if(checkedPixels[i + current.y * w + w] == 0 && colors[i + current.y * w + w] != refCol)
                        nodes.Enqueue(new Point(i, current.y + 1));
                }
                if(current.y - 1 >= 0) {
                    if(checkedPixels[i + current.y * w - w] == 0 && colors[i + current.y * w - w] != refCol)
                        nodes.Enqueue(new Point(i, current.y - 1));
                }
            }
            for(int i = current.x - 1; i >= 0; i--) {
                if(checkedPixels[i + current.y * w] > 0 || colors[i + current.y * w] == refCol)
                    break;
                colors[i + current.y * w] = aFillColor;
                checkedPixels[i + current.y * w] = 1;
                if(current.y + 1 < h) {
                    if(checkedPixels[i + current.y * w + w] == 0 && colors[i + current.y * w + w] != refCol)
                        nodes.Enqueue(new Point(i, current.y + 1));
                }
                if(current.y - 1 >= 0) {
                    if(checkedPixels[i + current.y * w - w] == 0 && colors[i + current.y * w - w] != refCol)
                        nodes.Enqueue(new Point(i, current.y - 1));
                }
            }
        }
        aTex.SetPixels(colors);
    }

}

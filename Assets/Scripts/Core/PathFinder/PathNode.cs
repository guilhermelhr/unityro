using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PathNode {
    public PathNode parentNode;
    public int x;
    public float y;
    public int z;
    public int gCost;
    public int hCost;
    public int fCost => gCost + hCost;
    public int flag;
    public bool walkable = true;
}
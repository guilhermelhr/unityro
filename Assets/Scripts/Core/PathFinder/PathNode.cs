public class PathNode {
    public PathNode parentNode;
    public int x;
    public double y;
    public int z;
    public int gCost;
    public int hCost;
    public int fCost => gCost + hCost;
    public int flag;
    public bool walkable = true;
}
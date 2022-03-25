using System;
using UnityEngine;

[Serializable]
public class PathNode {
    [SerializeField] public PathNode parentNode;
    [SerializeField] public int x;
    [SerializeField] public double y;
    [SerializeField] public int z;
    [SerializeField] public int gCost;
    [SerializeField] public int hCost;
    [SerializeField] public int fCost => gCost + hCost;
    [SerializeField] public int flag;
    [SerializeField] public bool walkable = true;
}
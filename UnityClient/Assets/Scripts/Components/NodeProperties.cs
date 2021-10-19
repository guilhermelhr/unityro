using UnityEngine;
using System.Collections;

public class NodeProperties : MonoBehaviour
{
    //hierarchy
    public int nodeId;
    public string parentName;
    public string mainName;
    internal bool isChild {
        get { return !string.IsNullOrEmpty(parentName) && !parentName.Equals(mainName); }
    }
}

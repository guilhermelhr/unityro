using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityWalk : MonoBehaviour {

    /**
	 * Direction look up table
	 */
    private int[][] DIRECTION = new int[][] {
        new int[] { 1,2,3 },
        new int[] { 0,0,4 },
        new int[] { 7,6,5 }
    };

    //private class WalkStructure {
    int speed = 150;
    int tick = 0;
    List<PathNode> path = new List<PathNode>();
    Vector3 position = Vector3.zero;
    Action onEnd = null;
    int index = 0;
    int total = 0;
    //}

    private Coroutine MoveIE, MoveToIE;
    private Transform Transform;

    public void Init(Transform transform) {
        this.Transform = transform;
    }

    public void WalkTo(Vector3 targetPosition, Vector3 fromPosition) {
        var path = Core.PathFinding.GetPath((int)fromPosition.x, (int)fromPosition.z, (int)targetPosition.x, (int)targetPosition.z);

        if (MoveIE != null) {
            Core.Instance.StopCoroutine(MoveIE);
        }
        if (MoveToIE != null) {
            Core.Instance.StopCoroutine(MoveToIE);
        }
        MoveIE = Core.Instance.StartCoroutine(Move(path));
    }

    IEnumerator Move(List<PathNode> path) {
        foreach (var node in path) {
            MoveToIE = Core.Instance.StartCoroutine(MoveTo(node));
            yield return MoveToIE;
        }
    }

    IEnumerator MoveTo(PathNode node) {
        var destination = new Vector3(node.x, (float)node.y, node.z);
        while (Transform.position != destination) {
            Transform.position = Vector3.MoveTowards(Transform.position, destination, (speed / 10) * Time.deltaTime);
            yield return null;
        }
    }
}
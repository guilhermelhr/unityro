using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EntityWalk : MonoBehaviour {

    private Coroutine MoveIE, MoveToIE;
    private int speed = 150;

    public void WalkTo(Vector3 targetPosition) {
        var path = Core.PathFinding.GetPath((int)transform.position.x, (int)transform.position.z, (int)targetPosition.x, (int)targetPosition.z);

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
        while (transform.position != destination) {
            transform.position = Vector3.MoveTowards(transform.position, destination, (speed / 10) * Time.deltaTime);
            yield return null;
        }
    }
}
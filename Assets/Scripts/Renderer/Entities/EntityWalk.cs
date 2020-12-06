using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EntityWalk : MonoBehaviour {

    private Coroutine MoveIE, MoveToIE;
    private int speed = 150;

    private void Awake() {
        Core.NetworkClient.HookPacket(ZC.NOTIFY_PLAYERMOVE.HEADER, OnPlayerMovement);
    }

    /**
     * Server has acknowledged our request and set data back to us
     */
    private void OnPlayerMovement(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.NOTIFY_PLAYERMOVE) {
            var pkt = packet as ZC.NOTIFY_PLAYERMOVE;

            var path = Core.PathFinding.GetPath(pkt.startPosition[0], pkt.startPosition[1], pkt.endPosition[0], pkt.endPosition[1]);

            if(MoveIE != null) {
                Core.Instance.StopCoroutine(MoveIE);
            }
            if(MoveToIE != null) {
                Core.Instance.StopCoroutine(MoveToIE);
            }
            MoveIE = Core.Instance.StartCoroutine(Move(path));
        }
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

    public void RequestMove(int x, int y, int dir) {
        /**
         * Validate things such as if entity is sit, whatever
         */

        new CZ.REQUEST_MOVE2(x, y, dir).Send();
    }
}
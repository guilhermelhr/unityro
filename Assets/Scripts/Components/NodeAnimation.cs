using System.Collections.Generic;
using UnityEngine;

public class NodeAnimation : MonoBehaviour {
    public int nodeId;

    private SortedList<int, Quaternion> rotKeyframes;
    private SortedList<int, Vector3> posKeyframes;
    public long animLen;
    private Quaternion baseRotation;

    private int lastRotKeyframe;
    private int lastPosKeyframe;


    public void Initialize(Models.AnimProperties properties) {
        rotKeyframes = properties.rotKeyframes;
        posKeyframes = properties.posKeyframes;
        animLen = properties.animLen;
        baseRotation = properties.baseRotation;

        if(rotKeyframes.Count > 0) {
            lastRotKeyframe = rotKeyframes.Keys[rotKeyframes.Count - 1];
        }

        if(posKeyframes.Count > 0) {
            lastPosKeyframe = posKeyframes.Keys[posKeyframes.Count - 1];
        }
    }

    //this was based on Borf's BroLib https://github.com/Borf/browedit/blob/master/brolib/BroLib/Rsm.cpp#L134
    void Update () {
        if(rotKeyframes.Count > 0) {
            UpdateRotation();
        }

        if(posKeyframes.Count > 0) {
            UpdatePosition();
        }
    }

    private void UpdateRotation() {
        int tick = (int) (Time.realtimeSinceStartup * 1000) % lastRotKeyframe;

        int current = 0;
        for(int i = 0; i < rotKeyframes.Count; i++) {
            var key = rotKeyframes.Keys[i];
            if(key > tick) {
                current = Mathf.Max(i - 1, 0);
                break;
            }
        }

        int next = current + 1;
        if(next >= rotKeyframes.Count) {
            next = 0;
        }

        int currentTime = rotKeyframes.Keys[current];
        int nextTime = rotKeyframes.Keys[next];

        float interval = (tick - currentTime) / ((float) (nextTime - currentTime));

        Quaternion quat = Quaternion.Lerp(rotKeyframes[currentTime], rotKeyframes[nextTime], interval);

        transform.localRotation = baseRotation * quat;
    }

    private void UpdatePosition() {
        int tick = (int) (Time.realtimeSinceStartup * 1000) % lastPosKeyframe;

        int current = 0;
        for(int i = 0; i < posKeyframes.Count; i++) {
            var key = posKeyframes.Keys[i];
            if(key > tick) {
                current = Mathf.Max(i - 1, 0);
                break;
            }
        }

        int next = current + 1;
        if(next >= posKeyframes.Count) {
            next = 0;
        }

        int currentTime = posKeyframes.Keys[current];
        int nextTime = posKeyframes.Keys[next];

        float interval = (tick - currentTime) / ((float) (nextTime - currentTime));

        Vector3 position = Vector3.Lerp(posKeyframes[currentTime], posKeyframes[nextTime], interval);

        transform.localPosition = position;
    }
}

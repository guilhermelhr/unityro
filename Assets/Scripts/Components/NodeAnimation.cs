using System.Collections.Generic;
using UnityEngine;

public class NodeAnimation : MonoBehaviour {
    public int nodeId;

    private RSM.RotationKeyframe[] rotKeyframes;
    private RSM.PositionKeyframe[] posKeyframes;
    public long animLen;
    private Quaternion baseRotation;

    private int lastRotKeyframe;
    private int lastPosKeyframe;

    private bool isChild = false;

    public string parentName;
    public string mainName;

    public void Initialize(Models.AnimProperties properties) {
        rotKeyframes = properties.rotKeyframes;
        posKeyframes = properties.posKeyframes;
        animLen = properties.animLen;
        baseRotation = properties.baseRotation;

        if(rotKeyframes.Length > 0) {
            lastRotKeyframe = (int)rotKeyframes[rotKeyframes.Length - 1].frame;
        }

        if(posKeyframes.Length > 0) {
            lastPosKeyframe = (int)posKeyframes[posKeyframes.Length - 1].frame;
        }

        isChild = properties.isChild;
    }

    //this was based on Borf's BroLib https://github.com/Borf/browedit/blob/master/brolib/BroLib/Rsm.cpp#L134
    void Update () {
        if(rotKeyframes != null && rotKeyframes.Length > 0) {
            UpdateRotation();
        }

        if(posKeyframes != null && posKeyframes.Length > 0) {
            UpdatePosition();
        }
    }

    private void UpdateRotation() {
        int tick = (int) (Time.realtimeSinceStartup * 1000) % lastRotKeyframe;

        int current = 0;
        for(int i = 0; i < rotKeyframes.Length; i++) {
            var key = rotKeyframes[i].frame;
            if(key > tick) {
                current = Mathf.Max(i - 1, 0);
                break;
            }
        }

        int next = current + 1;
        if(next >= rotKeyframes.Length) {
            next = 0;
        }

        int currentTime = (int)rotKeyframes[current].frame;
        int nextTime = (int)rotKeyframes[next].frame;

        float interval = (tick - currentTime) / ((float) (nextTime - currentTime));

        Quaternion quat = Quaternion.Lerp(rotKeyframes[currentTime].q, rotKeyframes[nextTime].q, interval);

        if(isChild) {
            transform.localRotation = quat;
        } else {
            transform.localRotation = baseRotation * quat;
        }
    }

    private void UpdatePosition() {
        int tick = (int) (Time.realtimeSinceStartup * 1000) % lastPosKeyframe;

        int current = 0;
        for(int i = 0; i < posKeyframes.Length; i++) {
            var key = posKeyframes[i].frame;
            if(key > tick) {
                current = Mathf.Max(i - 1, 0);
                break;
            }
        }

        int next = current + 1;
        if(next >= posKeyframes.Length) {
            next = 0;
        }

        int currentTime = (int)posKeyframes[current].frame;
        int nextTime = (int)posKeyframes[next].frame;

        float interval = (tick - currentTime) / ((float) (nextTime - currentTime));

        Vector3 position = Vector3.Lerp(posKeyframes[currentTime].p, posKeyframes[nextTime].p, interval);

        transform.localPosition = position;
    }
}

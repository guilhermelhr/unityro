using UnityEngine;

public class NodeAnimation : MonoBehaviour {

    public int nodeId;

    [SerializeField]
    private AnimProperties Properties;

    private int lastRotKeyframe;
    private int lastPosKeyframe;

    public string parentName;
    public string mainName;

    public void Initialize(AnimProperties properties) {
        Properties = properties;

        if (Properties.rotKeyframes.Count > 0) {
            lastRotKeyframe = Properties.rotKeyframesKeys[Properties.rotKeyframes.Count - 1];
        }

        if (Properties.posKeyframes.Count > 0) {
            lastPosKeyframe = Properties.posKeyframesKeys[Properties.posKeyframes.Count - 1];
        }
    }

    private void Start() {
        if (Properties != null) {
            Initialize(Properties);
        }
    }

    //this was based on Borf's BroLib https://github.com/Borf/browedit/blob/master/brolib/BroLib/Rsm.cpp#L134
    void Update() {
        if (Properties.rotKeyframes != null && Properties.rotKeyframes.Count > 0) {
            UpdateRotation();
        }

        if (Properties.posKeyframes != null && Properties.posKeyframes.Count > 0) {
            UpdatePosition();
        }
    }

    private void UpdateRotation() {
        int tick = (int) (Time.realtimeSinceStartup * 1000) % lastRotKeyframe;

        int current = 0;
        for (int i = 0; i < Properties.rotKeyframes.Count; i++) {
            var key = Properties.rotKeyframesKeys[i];
            if (key > tick) {
                current = Mathf.Max(i - 1, 0);
                break;
            }
        }

        int next = current + 1;
        if (next >= Properties.rotKeyframes.Count) {
            next = 0;
        }

        int currentTime = Properties.rotKeyframesKeys[current];
        int nextTime = Properties.rotKeyframesKeys[next];

        float interval = (tick - currentTime) / ((float) (nextTime - currentTime));

        Quaternion quat = Quaternion.Lerp(Properties.rotKeyframes[current], Properties.rotKeyframes[next], interval);

        if (Properties.isChild) {
            transform.localRotation = quat;
        } else {
            transform.localRotation = Properties.baseRotation * quat;
        }
    }

    private void UpdatePosition() {
        int tick = (int) (Time.realtimeSinceStartup * 1000) % lastPosKeyframe;

        int current = 0;
        for (int i = 0; i < Properties.posKeyframes.Count; i++) {
            var key = Properties.posKeyframesKeys[i];
            if (key > tick) {
                current = Mathf.Max(i - 1, 0);
                break;
            }
        }

        int next = current + 1;
        if (next >= Properties.posKeyframes.Count) {
            next = 0;
        }

        int currentTime = Properties.posKeyframesKeys[current];
        int nextTime = Properties.posKeyframesKeys[next];

        float interval = (tick - currentTime) / ((float) (nextTime - currentTime));

        Vector3 position = Vector3.Lerp(Properties.posKeyframes[current], Properties.posKeyframes[next], interval);

        transform.localPosition = position;
    }
}

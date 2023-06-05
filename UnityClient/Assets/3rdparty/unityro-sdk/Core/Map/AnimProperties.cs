using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimProperties {
    [SerializeField]
    public List<Quaternion> rotKeyframes;

    [SerializeField]
    public List<int> rotKeyframesKeys;

    [SerializeField]
    public List<Vector3> posKeyframes;

    [SerializeField]
    public List<int> posKeyframesKeys;

    [SerializeField]
    public long animLen;

    [SerializeField]
    public Quaternion baseRotation;

    [SerializeField]
    public bool isChild;
}
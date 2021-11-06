#if UNITY_EDITOR
using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OfflineUtility))]
public class OfflineUtilityEditor : Editor {

    public override void OnInspectorGUI() {
        var component = (OfflineUtility) target;
        base.OnInspectorGUI();

        if (GUILayout.Button("Load map")) {
            component.LoadMap();
        }
    }
}
#endif
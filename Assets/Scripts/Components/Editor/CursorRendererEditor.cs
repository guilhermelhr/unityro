using System;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CursorRenderer))]
public class CursorRendererEditor : Editor {

    public override void OnInspectorGUI() {
        var component = (CursorRenderer)target;
        base.OnInspectorGUI();

        foreach (var action in Enum.GetValues(typeof(CursorAction))) {
            if (GUILayout.Button(((CursorAction) action).ToString())) {
                component.SetAction((CursorAction) action, false);
            }
        }
    }
}
#endif
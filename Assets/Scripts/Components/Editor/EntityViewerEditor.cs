using System;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EntityViewer))]
public class EntityViewerEditor : Editor {

    public override void OnInspectorGUI() {
        var component = (EntityViewer)target;
        base.OnInspectorGUI();

        foreach (var motion in Enum.GetValues(typeof(SpriteMotion))) {
            if (GUILayout.Button(((SpriteMotion)motion).ToString())) {
                component.ChangeMotion((SpriteMotion)motion);
            }
        }
    }
}
#endif
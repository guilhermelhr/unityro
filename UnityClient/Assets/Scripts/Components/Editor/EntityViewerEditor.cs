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
                component.ChangeMotion(
                    new EntityViewer.MotionRequest { Motion = (SpriteMotion)motion },
                    new EntityViewer.MotionRequest { Motion = SpriteMotion.Idle }
                );
            }
        }


        if (GUILayout.Button("Set attack speed")) {
            component.SetMotionSpeedMultipler(450);
        }
    }
}
#endif
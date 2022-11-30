using System;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpriteEntityViewer))]
public class EntityViewerEditor : Editor {

    public override void OnInspectorGUI() {
        var component = (SpriteEntityViewer)target;
        base.OnInspectorGUI();

        if (GUILayout.Button("Attack > Standby")) {
            component.ChangeMotion(
                    new MotionRequest { Motion = SpriteMotion.Attack },
                    new MotionRequest { Motion = SpriteMotion.Standby }
                );
        }

        if (GUILayout.Button("Hit > Standby")) {
            component.ChangeMotion(
                    new MotionRequest { Motion = SpriteMotion.Hit },
                    new MotionRequest { Motion = SpriteMotion.Standby }
                );
        }

        foreach (var motion in Enum.GetValues(typeof(SpriteMotion))) {
            if (GUILayout.Button(((SpriteMotion)motion).ToString())) {
                if ((SpriteMotion)motion == SpriteMotion.Attack) {
                    component.Entity.SetAttackSpeed(380);
                }
                component.ChangeMotion(
                    new MotionRequest { Motion = (SpriteMotion)motion },
                    new MotionRequest { Motion = SpriteMotion.Idle }
                );
            }
        }
    }
}
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomPanel))]
public class CustomPanelEditor : UnityEditor.UI.RawImageEditor {

    public override void OnInspectorGUI() {

        var component = (CustomPanel)target;
        base.OnInspectorGUI();

        component.backgroundImage = EditorGUILayout.TextField("Background Image", component.backgroundImage);
        component.hoverImage = EditorGUILayout.TextField("Hover Image", component.hoverImage);
        component.pressedImage = EditorGUILayout.TextField("Pressed Image", component.pressedImage);
        component.overrideSize = EditorGUILayout.Toggle("Override Size", component.overrideSize);
    }
}
#endif
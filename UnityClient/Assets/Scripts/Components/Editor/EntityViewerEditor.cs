using System;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EntityViewer))]
public class EntityViewerEditor : Editor {

    public override void OnInspectorGUI() {
        var component = (EntityViewer) target;
        base.OnInspectorGUI();

        foreach (var motion in Enum.GetValues(typeof(SpriteMotion))) {
            if (GUILayout.Button(((SpriteMotion) motion).ToString())) {
                component.ChangeMotion(
                    new EntityViewer.MotionRequest { Motion = (SpriteMotion) motion }
                );
            }
        }


        GUILayout.Space(8);

        if (GUILayout.Button("Set attack speed")) {
            component.SetMotionSpeedMultipler(450);
        }

        if (GUILayout.Button("Extract textures from motion")) {
            var frameIndex = 0;
            for (int cameraDirection = 0; cameraDirection < 8; cameraDirection++) {
                var entityDirection = 8;
                var currentActionIndex = (component.ActionId + (cameraDirection + entityDirection) % 8) % component.currentACT.actions.Length;
                var currentAction = component.currentACT.actions[currentActionIndex];

                foreach (var frame in currentAction.frames) {
                    foreach (var layer in frame.layers) {
                        var sprite = component.sprites[layer.index];

                        Texture2D texture = sprite.texture;
                        if (layer.isMirror) {
                            texture = texture.FlipTexture();
                        }

                        string assetPath = Path.Combine("Assets", "Resources", "Textures", "Extract");

                        Directory.CreateDirectory(assetPath);
                        texture.alphaIsTransparency = true;
                        var bytes = texture.EncodeToPNG();
                        var completePath = Path.Combine(assetPath, frameIndex + ".png");
                        File.WriteAllBytes(completePath, bytes);
                    }

                    frameIndex++;
                }
            }
        }
    }
}

public static class Texture2DHelper {
    public static Texture2D FlipTexture(this Texture2D original) {
        int textureWidth = original.width;
        int textureHeight = original.height;

        Color[] colorArray = original.GetPixels();

        for (int j = 0; j < textureHeight; j++) {
            int rowStart = 0;
            int rowEnd = textureWidth - 1;

            while (rowStart < rowEnd) {
                Color hold = colorArray[(j * textureWidth) + (rowStart)];
                colorArray[(j * textureWidth) + (rowStart)] = colorArray[(j * textureWidth) + (rowEnd)];
                colorArray[(j * textureWidth) + (rowEnd)] = hold;
                rowStart++;
                rowEnd--;
            }
        }

        Texture2D finalFlippedTexture = new Texture2D(original.width, original.height);
        finalFlippedTexture.SetPixels(colorArray);
        finalFlippedTexture.Apply();

        return finalFlippedTexture;
    }
}
#endif
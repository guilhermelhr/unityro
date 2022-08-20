
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

internal static class AddressablesExtensions {
    internal static void SetAddressableGroup(this UnityEngine.Object obj, string groupName, string labelName) {
        var settings = AddressableAssetSettingsDefaultObject.Settings;

        if (settings) {
            var group = settings.FindGroup(groupName);
            if (!group)
                group = settings.CreateGroup(groupName, false, false, true, null, typeof(ContentUpdateGroupSchema), typeof(BundledAssetGroupSchema));

            var assetpath = AssetDatabase.GetAssetPath(obj);
            var guid = AssetDatabase.AssetPathToGUID(assetpath);
            var length = "Assets/_Generated/Resources/".Length;

            var e = settings.CreateOrMoveEntry(guid, group, false, false);
            e.SetAddress(e.address[length..], false);
            if (labelName != null) {
                e.SetLabel(labelName, true, true, false);
            }
            var entriesAdded = new List<AddressableAssetEntry> { e };

            group.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, false, true);
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, true, false);
        }
    }

    internal static void SetAddressableGroup(this List<UnityEngine.Object> objs, string groupName, string labelName) {
        var settings = AddressableAssetSettingsDefaultObject.Settings;

        if (settings) {
            var group = settings.FindGroup(groupName);
            if (!group)
                group = settings.CreateGroup(groupName, false, false, true, null, typeof(ContentUpdateGroupSchema), typeof(BundledAssetGroupSchema));

            var entriesAdded = new List<AddressableAssetEntry>();
            var length = "Assets/_Generated/Resources/".Length;

            try {
                AssetDatabase.StartAssetEditing();

                for (int i = 0; i < objs.Count; i++) {
                    var progress = i * 1f / objs.Count;
                    if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Assigning to addressables group... {i} of {objs.Count}\t\t{progress * 100}%", progress)) {
                        break;
                    }


                    var obj = objs[i];
                    string assetpath = AssetDatabase.GetAssetPath(obj);
                    var guid = AssetDatabase.AssetPathToGUID(assetpath);

                    var e = settings.CreateOrMoveEntry(guid, group, false, false);
                    string address;
                    if (e.address != null) {
                        address = e.address[length..];
                    } else {
                        address = assetpath;
                    }
                    e.SetAddress(address, false);
                    if (labelName != null) {
                        e.SetLabel(labelName, true, true, false);
                    }
                    entriesAdded.Add(e);
                }
            } catch (Exception e) {
                Debug.LogException(e);
            } finally {
                AssetDatabase.StopAssetEditing();
                EditorUtility.ClearProgressBar();
            }

            group.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, false, true);
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, true, false);
        }
    }
}
#endif
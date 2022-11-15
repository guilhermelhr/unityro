
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

internal static class AddressablesExtensions {

    private static int GetPrefixLength(bool useResourcesPath = false) {
        var isAddressablesInitialized = Directory.Exists(DataUtility.GENERATED_ADDRESSABLES_PATH);
        return ((isAddressablesInitialized && !useResourcesPath) ? DataUtility.GENERATED_ADDRESSABLES_PATH : DataUtility.GENERATED_RESOURCES_PATH).Length + 1;
    }

    internal static void SetAddressableGroup(this UnityEngine.Object obj, string groupName, string labelName, bool useResourcesPath = false) {
        var settings = AddressableAssetSettingsDefaultObject.Settings;

        if (settings) {
            var group = settings.FindGroup(groupName);
            if (!group)
                group = settings.CreateGroup(groupName, false, false, true, null, typeof(ContentUpdateGroupSchema), typeof(BundledAssetGroupSchema));

            var assetpath = AssetDatabase.GetAssetPath(obj);
            var guid = AssetDatabase.AssetPathToGUID(assetpath);
            

            var e = settings.CreateOrMoveEntry(guid, group, false, false);
            e.SetAddress(e.address[GetPrefixLength(useResourcesPath)..], false);
            if (labelName != null) {
                e.SetLabel(labelName, true, true, false);
            }
            var entriesAdded = new List<AddressableAssetEntry> { e };

            group.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, false, true);
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, true, false);
        }
    }

    internal static void SetAddressableGroup<T>(this List<T> objs, string groupName, string labelName, bool useResourcesPath = false) where T : UnityEngine.Object {
        var settings = AddressableAssetSettingsDefaultObject.Settings;

        if (settings) {
            var group = settings.FindGroup(groupName);
            if (!group)
                group = settings.CreateGroup(groupName, false, false, true, null, typeof(ContentUpdateGroupSchema), typeof(BundledAssetGroupSchema));

            var entriesAdded = new List<AddressableAssetEntry>();

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
                    e.SetAddress(e.address[GetPrefixLength(useResourcesPath)..], false);
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
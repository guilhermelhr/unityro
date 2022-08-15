
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

internal static class AddressablesExtensions {
    internal static void SetAddressableGroup(this Object obj, string groupName, string labelName) {
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

            group.SetDirty(AddressableAssetSettings.ModificationEvent.EntryCreated, entriesAdded, true, true);
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryCreated, entriesAdded, true, false);
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
                e.SetAddress(e.address[length..], false);
                if (labelName != null) {
                    e.SetLabel(labelName, true, true, false);
                }
                entriesAdded.Add(e);
            }
            AssetDatabase.StopAssetEditing();
            EditorUtility.ClearProgressBar();

            group.SetDirty(AddressableAssetSettings.ModificationEvent.EntryCreated, entriesAdded, true, true);
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryCreated, entriesAdded, true, false);
        }
    }
}
#endif
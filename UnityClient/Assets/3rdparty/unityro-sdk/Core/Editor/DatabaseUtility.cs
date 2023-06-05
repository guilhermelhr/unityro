using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityRO.Core.Database;

namespace UnityRO.Core.Editor {
    public static class DatabaseUtility {
        [MenuItem("UnityRO/Utils/Database/Generate Job Database")]
        static void GenerateJobDatabase() {
            var bodies = DataUtility.GetFilesFromDir("Assets/3rdparty/unityro-resources/Resources/Sprites/Body/")
                .Where(it => Path.GetExtension(it) == ".asset")
                .Select(AssetDatabase.LoadAssetAtPath<SpriteData>)
                .Select(it => new { key = it.name.Replace("_m", "").Replace("_f", "").ToLowerInvariant(), value = it })
                .GroupBy(it => it.key)
                .ToList();

            AssetDatabase.StartAssetEditing();
            foreach (var body in bodies) {
                try {
                    var mfBody = body.ToList();
                    var f = mfBody.FirstOrDefault(it => it.value.name.EndsWith("_f"))?.value;
                    var m = mfBody.FirstOrDefault(it => it.value.name.EndsWith("_m"))?.value;

                    var job = ScriptableObject.CreateInstance<SpriteJob>();
                    job.JobId = f != null ? f.jobId : m.jobId;
                    job.Female = f;
                    job.Male = m;

                    var fullAssetPath = $"Assets/3rdparty/unityro-resources/Resources/Database/Job/{body.Key}.asset";
                    AssetDatabase.CreateAsset(job, fullAssetPath);
                } catch {
                    Debug.LogError($"Error saving job {body.Key}");
                }
            }

            AssetDatabase.StopAssetEditing();
        }

        [MenuItem("UnityRO/Utils/Database/Generate Head Database")]
        static void GenerateHeadDatabase() {
            var heads = DataUtility.GetFilesFromDir("Assets/3rdparty/unityro-resources/Resources/Sprites/Head/")
                .Where(it => Path.GetExtension(it) == ".asset")
                .Select(AssetDatabase.LoadAssetAtPath<SpriteData>)
                .Select(it => new { key = it.name.Replace("_m", "").Replace("_f", "").ToLowerInvariant(), value = it })
                .GroupBy(it => it.key)
                .ToList();

            AssetDatabase.StartAssetEditing();
            foreach (var head in heads) {
                try {
                    var mfHead = head.ToList();
                    var f = mfHead.FirstOrDefault(it => it.value.name.EndsWith("_f"))?.value;
                    var m = mfHead.FirstOrDefault(it => it.value.name.EndsWith("_m"))?.value;

                    var spriteHead = ScriptableObject.CreateInstance<SpriteHead>();
                    spriteHead.Id = int.Parse(head.Key);
                    spriteHead.Female = f;
                    spriteHead.Male = m;

                    var fullAssetPath = $"Assets/3rdparty/unityro-resources/Resources/Database/Head/{head.Key}.asset";
                    AssetDatabase.CreateAsset(spriteHead, fullAssetPath);
                } catch {
                    Debug.LogError($"Error saving job {head.Key}");
                }
            }

            AssetDatabase.StopAssetEditing();
        }

        [MenuItem("UnityRO/Utils/Database/Generate NPC Database")]
        static void GenerateNPCDatabase() {
            var npcs = DataUtility.GetFilesFromDir("Assets/3rdparty/unityro-resources/Resources/Sprites/Npc/")
                .Where(it => Path.GetExtension(it) == ".asset")
                .Select(AssetDatabase.LoadAssetAtPath<SpriteData>)
                .ToList();


            AssetDatabase.StartAssetEditing();
            foreach (var npc in npcs) {
                try {
                    var job = ScriptableObject.CreateInstance<SpriteJob>();
                    job.JobId = npc.jobId;
                    job.Male = npc;

                    var fullAssetPath = $"Assets/3rdparty/unityro-resources/Resources/Database/Npc/{npc.name}.asset";
                    AssetDatabase.CreateAsset(job, fullAssetPath);
                } catch {
                    Debug.LogError($"Error saving job {npc.name}");
                }
            }

            AssetDatabase.StopAssetEditing();
        }

        [MenuItem("UnityRO/Utils/Database/Assign Entries to DB")]
        static void AssignEntriesToDbFile() {
            var pcjobs = Resources.LoadAll<Job>("Database/Job").ToList();
            var npcJobs = Resources.LoadAll<Job>("Database/Npc").ToList();
            var spriteHeads = Resources.LoadAll<SpriteHead>("Database/Head").ToList();
            
            var jobDatabase = ScriptableObject.CreateInstance<JobDatabase>();
            jobDatabase.Values = new List<Job>();
            jobDatabase.Values.AddRange(pcjobs);
            jobDatabase.Values.AddRange(npcJobs);
            AssetDatabase.CreateAsset(jobDatabase, "Assets/3rdparty/unityro-resources/Resources/Database/JobDatabase.asset");
            
            var spriteHeadDatabase = ScriptableObject.CreateInstance<SpriteHeadDatabase>();
            spriteHeadDatabase.Values = new List<SpriteHead>();
            spriteHeadDatabase.Values.AddRange(spriteHeads);
            AssetDatabase.CreateAsset(spriteHeadDatabase, "Assets/3rdparty/unityro-resources/Resources/Database/SpriteHeadDatabase.asset");
        }
    }
}
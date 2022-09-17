using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Utils.Extensions {
    internal static class AssetReferenceExtensions {

        internal static async Task<T> LoadAsync<T>(this AssetReference aref) where T : UnityEngine.Object {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return (T) aref.editorAsset;
#endif
            return await aref.LoadAssetAsync<T>().Task;
        }

        internal static async Task<Texture2D> LoadAsync(this AssetReferenceTexture2D aref) {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return (Texture2D) aref.editorAsset;
#endif
            return await aref.LoadAssetAsync().Task;
        }
    }
}

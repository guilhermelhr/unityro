using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Core {
    internal static class UnityFileManager {

        internal static async Task<TObject> LoadAssetAsync<TObject>(string key) {
            return await Addressables.LoadAssetAsync<TObject>(key);
        }
    }
}

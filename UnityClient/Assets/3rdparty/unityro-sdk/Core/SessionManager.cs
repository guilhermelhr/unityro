using System.Threading.Tasks;
using Core.Path;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityRO.Core.GameEntity;

namespace UnityRO.Net {
    public class SessionManager : MonoBehaviour {
        public Session CurrentSession { get; private set; }
        private Scene CurrentScene;

        private NetworkClient NetworkClient;
        private PathFinder PathFinder;

        private void Awake() {
            NetworkClient = FindObjectOfType<NetworkClient>();
        }

        private void Start() {
            NetworkClient.HookPacket<ZC.NPCACK_MAPMOVE>(ZC.NPCACK_MAPMOVE.HEADER, OnEntityMoved);
        }

        private void OnEnable() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void StartSession(INetworkEntity networkEntity, int accountId) {
            CurrentSession = new Session(networkEntity, accountId);
        }

        public async Task SetCurrentMap(string mapName) {
            if (CurrentSession.CurrentMap == mapName) {
                return;
            }

            var mapScene = SceneManager.GetSceneByName(CurrentSession.CurrentMap ?? "");
            if (mapScene.isLoaded) {
                await UnloadScene(CurrentScene.name);
            }

            CurrentSession.SetCurrentMap(mapName);

            await LoadScene(CurrentSession.CurrentMap, LoadSceneMode.Additive);
        }

        private void OnSceneLoaded(Scene currentScene, LoadSceneMode arg1) {
            CurrentScene = currentScene;
        }

        private async void OnEntityMoved(ushort cmd, int size, ZC.NPCACK_MAPMOVE pkt) {
            if (CurrentSession.CurrentMap != pkt.MapName) {
                await SetCurrentMap(pkt.MapName);
            }

            if (PathFinder == null) {
                PathFinder = FindObjectOfType<PathFinder>();
            }

            var height = PathFinder?.GetCellHeight(pkt.PosX, pkt.PosY) ?? 0f;
            var position = new Vector3(pkt.PosX, height, pkt.PosY);
            if (CurrentSession.Entity is CoreGameEntity gameEntity) {
                gameEntity.transform.position = position;
                new CZ.NOTIFY_ACTORINIT().Send();
            }
        }

        private Task<bool> LoadScene(string sceneName, LoadSceneMode mode) {
            var t = new TaskCompletionSource<bool>();

            SceneManager.LoadSceneAsync(sceneName, mode).completed += delegate { t.TrySetResult(true); };

            return t.Task;
        }

        private Task<bool> UnloadScene(string sceneName) {
            var t = new TaskCompletionSource<bool>();

            SceneManager.UnloadSceneAsync(sceneName).completed += delegate { t.TrySetResult(true); };

            return t.Task;
        }
    }
}
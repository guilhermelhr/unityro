using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityRO.Net.Editor {
    [InitializeOnLoad]
    public class NetworkSnifferWindow : EditorWindow {
        private List<KeyValuePair<NetworkPacket, bool>> _networkPackets = new();
        private Vector2 _currentScrollPos;

        [MenuItem("Window/NetworkSniffer")]
        public static void ShowWindow() {
            EditorWindow.GetWindow(typeof(NetworkSnifferWindow), false, "Network Sniffer");
        }

        private void OnEnable() {
            NetworkClient.OnPacketEvent += OnPacketReceived;
        }

        private void OnDisable() {
            NetworkClient.OnPacketEvent -= OnPacketReceived;
        }

        private void OnGUI() {
            _currentScrollPos = EditorGUILayout.BeginScrollView(_currentScrollPos, GUILayout.Width(200));
            var defaultColor = GUI.contentColor;
            foreach (var keypair in _networkPackets) {
                GUI.contentColor = keypair.Value ? defaultColor : Color.yellow;
                if (keypair.Key is InPacket In) {
                    GUILayout.Button($"<< {In.Header}");
                } else if (keypair.Key is OutPacket Out) {
                    GUILayout.Button($">> {Out.Header}");
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private void OnInspectorUpdate() {
            Repaint();
        }

        private void OnPacketReceived(NetworkPacket packet, bool isHandled) {
            if (_networkPackets.Count > 50) {
                _networkPackets.RemoveAt(0);
            }
            _networkPackets.Add(new KeyValuePair<NetworkPacket, bool>(packet, isHandled));
        }
    }
}
using ROIO.Models.FileTypes;
using System;
using UnityEngine;

namespace Assets.Scripts.Renderer.Sprite {

    [Serializable]
    public class SpriteData : ScriptableObject {

        [SerializeField]
        public ACT act;
        [SerializeField]
        public UnityEngine.Sprite[] sprites;
    }
}

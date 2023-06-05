namespace ROIO.Models.FileTypes {
    public struct RoImage {
        public int width;
        public int height;
        public UnityEngine.TextureFormat format;
        public bool mipChain;
        public byte[] data;

        public UnityEngine.Texture2D ConvertToTexture2D() {
            var t = new UnityEngine.Texture2D(width, height, format, mipChain);
            t.LoadRawTextureData(data);
            t.Apply();
            return t;
        }
    }
}
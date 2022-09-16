using ROIO;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Effects {
    class MapWarpEffect : MonoBehaviour {
        public GameObject FollowTarget;
        public float Duration;

        private PrimitiveCylinderEffect prim;
        private PrimitiveCylinderEffect prim2;
        private PrimitiveCircleEffect circle;

        private Material Ring1Material;
        private Material Ring2Material;
        private Material CircleMaterial;

        public async void StartWarp(GameObject parent) {
            if (Ring1Material == null) {
                Ring1Material = new Material(ShaderCache.Instance.AdditiveShader);
                Ring1Material.mainTexture = await Addressables.LoadAssetAsync<Texture2D>("data/texture/effect/ring_blue.tga").Task;
                Ring1Material.color = new Color(170 / 255f, 170 / 255f, 1f, 1f);
            }

            if (Ring2Material == null) {
                Ring2Material = new Material(ShaderCache.Instance.AdditiveShader);
                Ring2Material.mainTexture = await Addressables.LoadAssetAsync<Texture2D>("data/texture/effect/ring_blue.tga").Task;
                Ring2Material.color = new Color(100 / 255f, 100 / 255f, 1f, 1f);
            }

            if (CircleMaterial == null) {
                CircleMaterial = new Material(ShaderCache.Instance.AlphaBlendParticleShader);
                CircleMaterial.mainTexture = await Addressables.LoadAssetAsync<Texture2D>("data/texture/effect/alpha_down.tga").Task;
                //CircleMaterial.color = new Color(1f, 1f, 1f, 1f);
            }

            FollowTarget = parent;

            Init();
        }

        public void Init() {
            var angle = 60;

            prim = PrimitiveCylinderEffect.LaunchEffect(new GameObject("Warp(A)"), Ring1Material, 4, float.MaxValue);
            prim.transform.SetParent(gameObject.transform, false);
            prim.transform.localScale = new Vector3(2f, 2f, 2f);
            //mode = 11

            prim.Updater = prim.Update3DCasting4;
            prim.Renderer = prim.Render3DCasting;

            prim.Parts[0] = new EffectPart() {
                Active = true,
                Step = 0,
                CoverAngle = 360,
                MaxHeight = 2.5f,
                Angle = 270,
                Alpha = 0,
                Distance = 2.5f, //4.5f,
                RiseAngle = angle - 7
            };

            prim.Parts[1] = new EffectPart() {
                Active = true,
                Step = 0,
                CoverAngle = 360,
                MaxHeight = 5f,
                Angle = 0,
                Alpha = 0,
                Distance = 5f, //4.5f,
                RiseAngle = angle
            };

            prim.Parts[2] = new EffectPart() {
                Active = true,
                Step = 0,
                CoverAngle = 360,
                MaxHeight = 7.5f,
                Angle = 90,
                Alpha = 0,
                Distance = 7.5f, //4.5f,
                RiseAngle = angle - 5
            };

            prim.Parts[3] = new EffectPart() {
                Active = true,
                Step = 0,
                CoverAngle = 360,
                MaxHeight = 10f,
                Angle = 180,
                Alpha = 0,
                Distance = 10f, //4.5f,
                RiseAngle = angle - 10
            };

            prim2 = PrimitiveCylinderEffect.LaunchEffect(new GameObject("Warp(B)"), Ring2Material, 4, float.MaxValue);
            prim2.transform.SetParent(gameObject.transform, false);
            prim2.DelayUpdate(1 / 60f);
            prim2.transform.localScale = new Vector3(2f, 2f, 2f);
            //mode = 4

            prim2.Updater = prim2.Update3DCasting4;
            prim2.Renderer = prim2.Render3DCasting;

            prim2.Parts[0] = new EffectPart() {
                Active = true,
                Step = 0,
                CoverAngle = 360,
                MaxHeight = 2.5f,
                Angle = 271,
                Alpha = 0,
                Distance = 2.7f,
                RiseAngle = angle - 8
            };

            prim2.Parts[1] = new EffectPart() {
                Active = true,
                Step = 0,
                CoverAngle = 360,
                MaxHeight = 5f,
                Angle = 1,
                Alpha = 0,
                Distance = 5.2f,
                RiseAngle = angle - 1
            };

            prim2.Parts[2] = new EffectPart() {
                Active = true,
                Step = 0,
                CoverAngle = 360,
                MaxHeight = 7.7f,
                Angle = 91,
                Alpha = 0,
                Distance = 7.7f,
                RiseAngle = angle - 6
            };

            prim2.Parts[3] = new EffectPart() {
                Active = true,
                Step = 0,
                CoverAngle = 360,
                MaxHeight = 10.2f,
                Angle = 181,
                Alpha = 0,
                Distance = 10.2f,
                RiseAngle = angle - 11
            };

            circle = PrimitiveCircleEffect.LaunchEffect(new GameObject("Circle"), CircleMaterial, 0, float.MaxValue);
            circle.transform.SetParent(gameObject.transform, false);
            circle.DelayUpdate(2 / 60f);
            circle.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            circle.transform.localPosition += new Vector3(0f, 0.1f, 0f);

            circle.Updater = circle.Update3DCircle;
            circle.Renderer = circle.Render3DCircle;

            circle.Duration = float.MaxValue;
            circle.Radius = 15;
            circle.Alpha = 0;
            circle.MaxAlpha = 96;
            circle.AlphaSpeed = circle.MaxAlpha / 20f;
            circle.FadeOutStart = float.MaxValue - 10f;
            circle.ArcAngle = 36f;

            var particles = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PortalParticles"));
            particles.transform.SetParent(gameObject.transform, false);
            particles.transform.localPosition = Vector3.zero;

            //circle.transform.localRotation = Quaternion.Euler(90, 0, 0);
        }

        public void OnDestroy() {
            if (prim != null)
                Destroy(prim.gameObject);
            if (prim2 != null)
                Destroy(prim2.gameObject);
        }

        public void Update() {
            if (FollowTarget == null) {
                Destroy(gameObject);
                return;
            }

            transform.position = FollowTarget.transform.position;
        }
    }
}


using ROIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Sky : MonoBehaviour {
    private Material material;

    private const int CLOUD_TEXTURE_COUNT = 7;
    private const int MAX_CLOUDS = 200; //from roBrowser
    private const int CLOUD_SIZE = 40;//from roBrowser
    private const float FADEIN_DURATION = 1f;
    private const float FADEOUT_DURATION = 0.1f;
    private const float CLOUDS_HEIGHT = -10;
    private const float BOUNDS_PADDING = 80;
    private const float CLOUD_SPEED = 15;
    private const int CLOUDS_PER_AREA = 50;

    public static GameObject cloudsParent;

    private bool draw = false;
    private WeatherEffect.SkyPrefs prefs;
    private Texture2D[] textures;
    private Cloud[] clouds;
    private int cloudsVisible = 0;

    //possible locations for spawning clouds (always within camera frustum)
    private float[] bounds;

    public bool HasClouds {
        get { return draw; }
    }

    private class Cloud {
        public Vector3 position;
        public Vector3 direction;
        public float seed;
        public int texture;
        public Color color;
        public GameObject obj;
        public MeshRenderer mr;

        public float spawnTime;
        public float fadeOutAt;
        public bool fadeOut;
    }

    private async void Start() {
        clouds = new Cloud[MAX_CLOUDS];
        material = (Material) Resources.Load("Materials/Sprites/SpriteMaterial", typeof(Material));

        if (draw) {
            cloudsParent = new GameObject("_clouds");
            cloudsParent.transform.parent = MapRenderer.mapParent.transform;

            Camera.main.backgroundColor = prefs.skyColor;

            await LoadCloudTextures();
            SetupClouds();
        }
    }

    internal void Initialize(string mapname) {
        draw = (prefs = WeatherEffect.GetPrefs(mapname)) != null;
    }

    private void SetupClouds() {
        for (int i = 0; i < MAX_CLOUDS; i++) {
            if (clouds[i] == null) {
                clouds[i] = new Cloud();
                clouds[i].position = new Vector3();
                clouds[i].direction = new Vector3();
            }
            Cloud cloud = clouds[i];

            cloud.color = prefs.cloudColor;

            //position
            cloud.position.y = CLOUDS_HEIGHT;

            //direction
            cloud.direction.x = UnityEngine.Random.Range(0.5f * CLOUD_SPEED, CLOUD_SPEED);
            cloud.direction.y = 0; //UnityEngine.Random.Range(-1, 1) / 10;
            cloud.direction.z = UnityEngine.Random.Range(-1, 1) / 10;

            //sprite
            cloud.texture = Mathf.FloorToInt(UnityEngine.Random.Range(0, textures.Length));

            cloud.seed = UnityEngine.Random.Range(0, 2 * Mathf.PI);
        }
    }

    private async Task LoadCloudTextures() {
        textures = new Texture2D[CLOUD_TEXTURE_COUNT];
        for (int i = 0; i < textures.Length; i++) {
            var filename = $"data/texture/effect/cloud{i + 1}.png";
            var texture = await Addressables.LoadAssetAsync<Texture2D>(filename).Task;
            textures[i] = texture;
        }
    }

    private GameObject CreateGameObject(int id) {
        GameObject obj = new GameObject("cloud[" + id + "]");
        obj.transform.parent = cloudsParent.transform;

        var mf = obj.AddComponent<MeshFilter>();
        var mr = obj.AddComponent<MeshRenderer>();

        var mesh = new Mesh();
        mf.mesh = mesh;

        //TODO: make static
        mesh.vertices = new Vector3[] {
            new Vector3(0, 0, 0),
            new Vector3(CLOUD_SIZE, 0, 0),
            new Vector3(0, 0, CLOUD_SIZE),
            new Vector3(CLOUD_SIZE, 0, CLOUD_SIZE)
        };

        mesh.triangles = new int[6] {
            0, 2, 1,
            2, 3, 1
        };

        mesh.normals = new Vector3[] {
            Vector3.up, Vector3.up, Vector3.up, Vector3.up
        };

        mesh.uv = new Vector2[] {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };

        mr.material = material;

        return obj;
    }

    void FixedUpdate() {
        bounds = GetBounds();
    }

    void Update() {
        if (bounds == null) {
            bounds = GetBounds();
        }

        float now = Time.realtimeSinceStartup;

        for (int i = 0; i < clouds.Length; i++) {
            Cloud cloud = clouds[i];
            if (cloud != null) {
                //cloud is visible
                if (cloud.obj == null) {
                    //create gameobject and set texture
                    cloud.obj = CreateGameObject(i);
                    cloud.mr = cloud.obj.GetComponent<MeshRenderer>();
                    cloud.mr.material.mainTexture = textures[cloud.texture];
                    cloud.position.x = UnityEngine.Random.Range(bounds[0] - BOUNDS_PADDING, bounds[1] + BOUNDS_PADDING);
                    cloud.position.z = UnityEngine.Random.Range(bounds[2] - BOUNDS_PADDING, bounds[3] + BOUNDS_PADDING);
                    cloud.spawnTime = 0;
                    cloudsVisible++;
                }

                //update cloud position
                float modifier = Mathf.Pow(Mathf.Sin(cloud.seed + now / 4f), 2) * 0.5f;
                cloud.position.x += cloud.direction.x * Time.deltaTime * (1 - modifier);
                cloud.obj.transform.position = cloud.position;


                float alpha;
                //fade out
                if (cloud.fadeOut) {
                    alpha = Mathf.Clamp01(Mathf.Abs(cloud.fadeOutAt - now) / FADEOUT_DURATION);

                    if (now > cloud.fadeOutAt) {
                        cloud.fadeOut = false;
                        cloudsVisible++;
                        cloud.spawnTime = now;
                        cloud.position.x = UnityEngine.Random.Range(bounds[0] - BOUNDS_PADDING, bounds[1] + BOUNDS_PADDING / 2);
                        cloud.position.z = UnityEngine.Random.Range(bounds[2] - BOUNDS_PADDING / 2, bounds[3] + BOUNDS_PADDING / 2);
                    }
                } else {
                    if (!IsInsideBounds(cloud.position)) {
                        cloud.fadeOut = true;
                        cloud.fadeOutAt = now;
                        cloudsVisible--;
                    }

                    //fade in
                    alpha = Mathf.Clamp01((now - cloud.spawnTime) / FADEIN_DURATION);
                }

                cloud.color.a = alpha;
                cloud.mr.material.SetColor("_TintColor", cloud.color);
            }
        }
    }

    private bool IsInsideBounds(Vector3 position) {
        return position.x >= bounds[0] - BOUNDS_PADDING
            && position.x <= bounds[1] + BOUNDS_PADDING
            && position.z >= bounds[2] - BOUNDS_PADDING
            && position.z <= bounds[3] + BOUNDS_PADDING;
    }

    //return the intersection between the camera frustum and a 3D plane at y=CLOUDS_HEIGHT
    private float[] GetBounds() {
        Ray rBottomLeft = Camera.main.ViewportPointToRay(new Vector3(0, 0, 0));
        Ray rTopLeft = Camera.main.ViewportPointToRay(new Vector3(0, 1, 0));
        Ray rTopRight = Camera.main.ViewportPointToRay(new Vector3(1, 1, 0));
        Ray rBottomRight = Camera.main.ViewportPointToRay(new Vector3(1, 0, 0));

        Vector3 bottomLeft = GetPointAtHeight(rBottomLeft, CLOUDS_HEIGHT * 3);
        Vector3 topLeft = GetPointAtHeight(rTopLeft, CLOUDS_HEIGHT * 3);
        Vector3 topRight = GetPointAtHeight(rTopRight, CLOUDS_HEIGHT * 3);
        Vector3 bottomRight = GetPointAtHeight(rBottomRight, CLOUDS_HEIGHT * 3);

        float minX, minZ, maxX, maxZ;
        minX = bottomLeft.x;
        minX = topLeft.x < minX ? topLeft.x : minX;
        minX = topRight.x < minX ? topRight.x : minX;
        minX = bottomRight.x < minX ? bottomRight.x : minX;

        maxX = bottomLeft.x;
        maxX = topLeft.x > maxX ? topLeft.x : maxX;
        maxX = topRight.x > maxX ? topRight.x : maxX;
        maxX = bottomRight.x > maxX ? bottomRight.x : maxX;

        minZ = bottomLeft.z;
        minZ = topLeft.z < minZ ? topLeft.z : minZ;
        minZ = topRight.z < minZ ? topRight.z : minZ;
        minZ = bottomRight.z < minZ ? bottomRight.z : minZ;

        maxZ = bottomLeft.z;
        maxZ = topLeft.z > maxZ ? topLeft.z : maxZ;
        maxZ = topRight.z > maxZ ? topRight.z : maxZ;
        maxZ = bottomRight.z > maxZ ? bottomRight.z : maxZ;

        return new float[] { minX, maxX, minZ, maxZ };
    }

    public static Vector3 GetPointAtHeight(Ray ray, float height) {
        return ray.origin + (((ray.origin.y - height) / -ray.direction.y) * ray.direction);
    }

    private void OnDestroy() {
        if (draw) {
            GameObject.Destroy(cloudsParent);
            textures = null;
            clouds = null;
        }
    }
}

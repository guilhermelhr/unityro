using Assets.Scripts.Renderer.Sprite;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MeshEntityViewer : GameEntityViewer {

    public ViewerType ViewerType;
    public Entity Entity;

    private GameObject Mesh3D;

    public void Start() {
        Init();
    }

    public override void ChangeMotion(MotionRequest motion, MotionRequest? nextMotion = null) {
        State = motion.Motion switch {
            SpriteMotion.Dead => SpriteState.Dead,
            SpriteMotion.Sit => SpriteState.Sit,
            SpriteMotion.Idle => SpriteState.Idle,
            SpriteMotion.Walk => SpriteState.Walking,
            _ => SpriteState.Alive,
        };
    }

    public override IEnumerator FadeOut() {
        throw new System.NotImplementedException();
    }

    public override void Init(SpriteData spriteData, Texture2D atlas) {
        throw new System.NotImplementedException();
    }

    public override void Init(bool reloadSprites = false) {
        var path = DBManager.GetBodyPath(Entity.Status.jobId, Entity.Status.sex);

        ViewerType = ViewerType.MESH;
        var meshPath = $"data/model/3dmodel/{Path.GetFileNameWithoutExtension(path)}.prefab";
        var model = Addressables.LoadAssetAsync<GameObject>(meshPath).WaitForCompletion();
        if (model != null) {
            Mesh3D = Instantiate(model, transform);
        }
    }
}

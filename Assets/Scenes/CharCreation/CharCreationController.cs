using UnityEngine;

public class CharCreationController : MonoBehaviour {

    public Entity StyleEntity;
    public Entity HumanSelectionEntity;
    public Entity DoramSelecionEntity;

    private bool IsDirty = false;

    // Start is called before the first frame update
    void Start() {
        StyleEntity.Init(new CharacterData() { Sex = 1, Job = 0, Name = "Player", GID = 20001, Weapon = 1, Speed = 150 }, LayerMask.NameToLayer("Characters"), true);
        StyleEntity.SortingGroup.sortingOrder = 3;
        StyleEntity.SetReady(true, true);

        HumanSelectionEntity.Init(new CharacterData() { Sex = 1, Job = 0, Name = "Player", GID = 20001, Weapon = 1, Speed = 150 }, LayerMask.NameToLayer("Characters"), true);
        HumanSelectionEntity.SortingGroup.sortingOrder = 3;
        HumanSelectionEntity.SetReady(true, true);

        DoramSelecionEntity.Init(new CharacterData() { Sex = 1, Job = 0, Name = "Player", GID = 20001, Weapon = 1, Speed = 150 }, LayerMask.NameToLayer("Characters"), true);
        DoramSelecionEntity.SortingGroup.sortingOrder = 3;
        DoramSelecionEntity.SetReady(true, true);
    }

    // Update is called once per frame
    void LateUpdate() {
        if (!IsDirty) {
            HumanSelectionEntity.ChangeMotion(new EntityViewer.MotionRequest { Motion = SpriteMotion.Walk });
            IsDirty = true;
        }
    }

    public void SelectDoramRace() {
        DoramSelecionEntity.ChangeMotion(new EntityViewer.MotionRequest { Motion = SpriteMotion.Walk });
        HumanSelectionEntity.ChangeMotion(new EntityViewer.MotionRequest { Motion = SpriteMotion.Idle });
    }

    public void SelectHumanRace() {
        DoramSelecionEntity.ChangeMotion(new EntityViewer.MotionRequest { Motion = SpriteMotion.Idle });
        HumanSelectionEntity.ChangeMotion(new EntityViewer.MotionRequest { Motion = SpriteMotion.Walk });
    }
}

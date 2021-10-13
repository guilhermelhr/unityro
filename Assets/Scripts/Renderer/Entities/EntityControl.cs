using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EntityControl : MonoBehaviour {

    private LayerMask GroundMask;
    private LayerMask EntityMask;
    private CursorRenderer CursorRenderer;
    private TextMeshPro EntityNameText;
    private GridRenderer GridRenderer;

    private PendingAction CurrentPendingAction = new PendingAction.None();

    public Entity Entity;

    void Start() {
        GroundMask = LayerMask.GetMask("Ground");
        EntityMask = LayerMask.GetMask("NPC", "Monsters", "Items");
        CursorRenderer = Core.CursorRenderer;
        GridRenderer = FindObjectOfType<GridRenderer>();

        MaybeInitEntityNameObject();
    }

    private void MaybeInitEntityNameObject() {
        if (EntityNameText != null)
            return;

        var textPrefab = (GameObject) Resources.Load("Prefabs/EntityName");
        EntityNameText = Instantiate(textPrefab).GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update() {
        if (GridRenderer == null) {
            GridRenderer = FindObjectOfType<GridRenderer>();
        }

        MaybeInitEntityNameObject();

        var ray = Core.MainCamera.ScreenPointToRay(Input.mousePosition);
        var didHitAnything = Physics.Raycast(ray, out var hit, 150, EntityMask | GroundMask);
        var didHitAnyEntity = Physics.Raycast(ray, out var entityHit, 150, EntityMask);
        var isActionRequested = Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject();

        if (isActionRequested && CurrentPendingAction is PendingAction.TargetSelection && !didHitAnyEntity) {
            CurrentPendingAction = new PendingAction.None();
        }

        if (!didHitAnything) {
            return;
        }

        hit.collider.gameObject.TryGetComponent<EntityViewer>(out var target);

        if (target != null) {
            if (CurrentPendingAction is PendingAction.None) {
                switch (target.Entity.Type) {
                    case EntityType.NPC:
                        CursorRenderer.SetAction(CursorAction.TALK, false);
                        break;
                    case EntityType.ITEM:
                        CursorRenderer.SetAction(CursorAction.PICK, true);
                        break;
                    case EntityType.MOB:
                        CursorRenderer.SetAction(CursorAction.ATTACK, false);
                        break;
                    case EntityType.WARP:
                        CursorRenderer.SetAction(CursorAction.WARP, false);
                        break;
                }
            }

            if (target.Entity.Type != EntityType.WARP) {
                RenderEntityName(hit, target);
            }
            if (isActionRequested) {
                ProcessEntityClick(target.Entity);
            }
        } else if (CurrentPendingAction is PendingAction.TargetSelection) {
            CursorRenderer.SetAction(CursorAction.TARGET, false);
        } else {
            EntityNameText.text = null;
            CursorRenderer.SetAction(CursorAction.DEFAULT, true);

            if (!GridRenderer.IsCurrentPositionValid) {
                CursorRenderer.SetAction(CursorAction.INVALID, false);
            }
            if (isActionRequested) {
                Entity.RequestMove(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.z), 0);
            }
        }
    }

    private void RenderEntityName(RaycastHit hit, EntityViewer target) {
        var nameTargetPosition = hit.collider.gameObject.transform.parent.position;
        EntityNameText.gameObject.transform.position = new Vector3(nameTargetPosition.x + 0.5f, nameTargetPosition.y, nameTargetPosition.z);
        EntityNameText.text = target.Entity.Status.name;
    }

    private void ProcessEntityClick(Entity target) {
        switch (target.Type) {
            case EntityType.NPC:
                new CZ.CONTACTNPC() {
                    NAID = target.AID,
                    Type = 1
                }.Send();
                break;
            case EntityType.ITEM:
                Core.CursorRenderer.SetAction(CursorAction.PICK, false, 2);

                OutPacket pickPacket = new CZ.ITEM_PICKUP2() { ID = (int) target.AID };
                if (Vector3.Distance(transform.position, target.transform.position) > 2) {
                    Entity.AfterMoveAction = delegate {
                        pickPacket.Send();
                    };

                    new CZ.REQUEST_MOVE2() {
                        x = (short) target.transform.position.x,
                        y = (short) target.transform.position.z,
                        dir = 0
                    }.Send();

                    break;
                }

                pickPacket.Send();
                Entity.LookTo(target.transform.position);
                break;
            case EntityType.MOB:
                // TODO render lock arrow

                List<PathNode> path;
                OutPacket actionPacket;

                if (CurrentPendingAction is PendingAction.TargetSelection TargetSelection) {
                    path = Core.PathFinding.GetPath(Entity.transform.position, target.transform.position, TargetSelection.SkillInfo.AttackRange + 1);
                    actionPacket = new CZ.USE_SKILL2() {
                        SkillId = TargetSelection.SkillInfo.SkillID,
                        SelectedLevel = TargetSelection.Level,
                        TargetId = (int) target.AID
                    };
                } else {
                    path = Core.PathFinding.GetPath(Entity.transform.position, target.transform.position, Entity.GetBaseStatus().attackRange + 1);
                    actionPacket = new CZ.REQUEST_ACT2() {
                        TargetID = target.AID,
                        action = EntityActionType.CONTINUOUS_ATTACK
                    };
                }

                Action actionDelegate = delegate {
                    actionPacket.Send();
                    CurrentPendingAction = new PendingAction.None();
                };

                if (path.Count == 0) {
                    return;
                } else if (path.Count <= 1) {
                    actionDelegate.Invoke();
                } else {
                    PathNode endNode = path[path.Count - 1];

                    Entity.AfterMoveAction = actionDelegate;

                    new CZ.REQUEST_MOVE2() {
                        x = (short) endNode.x,
                        y = (short) endNode.z,
                        dir = (byte) Entity.Direction
                    }.Send();
                }

                break;
            case EntityType.WARP:
                break;
        }

    }

    internal void UseSkill(SkillInfo skillInfo, short level) {
        if ((skillInfo.SkillType & (int) SkillTargetType.Self) > 0) {
            new CZ.USE_SKILL2() {
                SkillId = skillInfo.SkillID,
                SelectedLevel = level,
                TargetId = (int) Entity.GID
            }.Send();
        }

        if ((skillInfo.SkillType & (int) SkillTargetType.Target) > 0) {
            CurrentPendingAction = new PendingAction.TargetSelection(skillInfo, level);
        }
    }

    public partial class PendingAction {

        public class None : PendingAction { }

        public class TargetSelection : PendingAction {
            public SkillInfo SkillInfo;
            public short Level;

            public TargetSelection(SkillInfo skillInfo, short level) {
                SkillInfo = skillInfo;
                Level = level;
            }
        }

    }

}

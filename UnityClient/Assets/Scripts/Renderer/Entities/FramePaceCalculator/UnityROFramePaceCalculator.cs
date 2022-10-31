using ROIO.Models.FileTypes;
using System.Collections;
using UnityEngine;
using UnityRO.GameCamera;
using static EntityViewer;

internal class UnityROFramePaceCalculator : MonoBehaviour, IFramePaceCalculator {

    private const int AVERAGE_ATTACK_SPEED = 432;
    private const int AVERAGE_ATTACKED_SPEED = 288;
    private const int MAX_ATTACK_SPEED = AVERAGE_ATTACKED_SPEED * 2;

    [SerializeField] private Entity Entity;
    [SerializeField] private ViewerType ViewerType;
    [SerializeField] private int CurrentFrame = 0;
    [SerializeField] private long AnimationStart = GameManager.Tick;
    [SerializeField] private float CurrentDelay = 0f;
    [SerializeField] private MotionRequest CurrentMotion;
    [SerializeField] private MotionRequest? NextMotion;
    [SerializeField] private ACT CurrentACT;
    [SerializeField] private ACT.Action CurrentAction;
    [SerializeField] private int ActionId;

    private Coroutine MotionQueueCoroutine;

    public void Init(Entity entity, ViewerType viewerType, ACT currentACT) {
        Entity = entity;
        ViewerType = viewerType;
        CurrentACT = currentACT;
    }

    public int GetActionIndex() {
        var cameraDirection = (int) (CharacterCamera.ROCamera?.Direction ?? 0);
        var entityDirection = (int) Entity.Direction + 8;

        return (ActionId + (cameraDirection + entityDirection) % 8) % CurrentACT.actions.Length;
    }

    public int GetCurrentFrame() {
        CurrentAction = CurrentACT.actions[GetActionIndex()];

        var isIdle = (Entity.Type == EntityType.PC && (CurrentMotion.Motion == SpriteMotion.Idle || CurrentMotion.Motion == SpriteMotion.Sit));
        int frameCount = CurrentAction.frames.Length;
        long deltaSinceMotionStart = GameManager.Tick - AnimationStart;

        var maxFrame = frameCount - 1;

        if (isIdle) {
            CurrentFrame = Entity.HeadDir;
        }

        CurrentDelay = GetDelay();
        if (deltaSinceMotionStart >= CurrentDelay) {
            AnimationStart = GameManager.Tick;

            if (CurrentFrame < maxFrame && !isIdle) {
                CurrentFrame++;
            }
        }

        if (CurrentFrame >= maxFrame) {
            if (AnimationHelper.IsLoopingMotion(CurrentMotion.Motion)) {
                CurrentFrame = 0;
            } else if (NextMotion.HasValue && ViewerType == ViewerType.BODY) {
                // Since body is the main component, it's the only one "allowed" to ask for the next motion
                Entity.ChangeMotion(NextMotion.Value);
            } else {
                CurrentFrame = maxFrame;
            }
        }

        return CurrentFrame;
    }

    public float GetDelay() {
        if (ViewerType == ViewerType.BODY && CurrentMotion.Motion == SpriteMotion.Walk) {
            return CurrentAction.delay / 150 * Entity.Status.walkSpeed;
        }

        if (CurrentMotion.Motion == SpriteMotion.Attack ||
            CurrentMotion.Motion == SpriteMotion.Attack1 ||
            CurrentMotion.Motion == SpriteMotion.Attack2 ||
            CurrentMotion.Motion == SpriteMotion.Attack3) {
            return (float) Entity.Status.attackSpeed / CurrentAction.frames.Length;
        }
        return CurrentAction.delay;
    }

    private IEnumerator DelayCurrentMotion(MotionRequest currentMotion, MotionRequest? nextMotion, int actionId) {
        yield return new WaitUntil(() => GameManager.Tick > currentMotion.delay);
        OnMotionChanged(currentMotion, nextMotion, actionId);
    }

    public void OnMotionChanged(MotionRequest currentMotion, MotionRequest? nextMotion, int actionId) {
        if (MotionQueueCoroutine != null) {
            StopCoroutine(MotionQueueCoroutine);
            MotionQueueCoroutine = null;
        }

        if (currentMotion.delay > GameManager.Tick) {
            MotionQueueCoroutine = StartCoroutine(DelayCurrentMotion(currentMotion, nextMotion, actionId));
            return;
        }

        AnimationStart = GameManager.Tick;
        CurrentFrame = 0;
        CurrentMotion = currentMotion;
        NextMotion = nextMotion;
        ActionId = actionId;
    }
}

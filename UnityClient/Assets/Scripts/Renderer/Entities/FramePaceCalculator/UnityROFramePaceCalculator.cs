using log4net.Util;
using ROIO.Models.FileTypes;
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
    [SerializeField] private float MotionSpeedMultiplier = 1;
    [SerializeField] private long AnimationStart = GameManager.Tick;
    [SerializeField] private float CurrentDelay = 0f;
    [SerializeField] private MotionRequest CurrentMotion;
    [SerializeField] private MotionRequest? NextMotion;
    [SerializeField] private ACT CurrentACT;
    [SerializeField] private ACT.Action CurrentAction;
    [SerializeField] private int ActionId;

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
            } else if (NextMotion.HasValue) {
                Entity.ChangeMotion(NextMotion.Value);
            } else {
                CurrentFrame = maxFrame;
            }
        }

        Debug.Log($"Current frame {CurrentFrame} for {CurrentMotion.Motion}");

        return CurrentFrame;
    }

    public float GetDelay() {
        if (ViewerType == ViewerType.BODY && CurrentMotion.Motion == SpriteMotion.Walk) {
            return CurrentAction.delay / 150 * Entity.Status.walkSpeed;
        }

        return CurrentAction.delay * MotionSpeedMultiplier;
    }

    public void SetMotionSpeedMultiplier(ushort attackMT) {
        //if (weapon is bow)
        if (attackMT > MAX_ATTACK_SPEED) {
            attackMT = MAX_ATTACK_SPEED;
        }
        //endif

        MotionSpeedMultiplier = (float) attackMT / AVERAGE_ATTACK_SPEED;
    }

    public void OnMotionChanged(MotionRequest currentMotion, MotionRequest? nextMotion, int actionId) {
        AnimationStart = GameManager.Tick;
        MotionSpeedMultiplier = 1;
        CurrentFrame = 0;
        CurrentMotion = currentMotion;
        NextMotion = nextMotion;
        ActionId = actionId;
    }
}

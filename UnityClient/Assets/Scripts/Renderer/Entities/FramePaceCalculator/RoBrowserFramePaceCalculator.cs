using ROIO.Models.FileTypes;
using System;
using System.Collections;
using UnityEngine;
using UnityRO.GameCamera;
using static SpriteEntityViewer;

internal class RoBrowserFramePaceCalculator : MonoBehaviour, IFramePaceCalculator {

    private Entity Entity;
    private ViewerType ViewerType;

    [SerializeField] private long AnimationStart = GameManager.Tick;
    [SerializeField] private double PreviousFrame;
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
        double animCount = CurrentAction.frames.Length;
        long tm = GameManager.Tick - AnimationStart;

        long delay = (long) GetDelay();
        if (delay <= 0) {
            delay = (int) CurrentAction.delay;
        }
        var headDir = 0;
        double frame;

        if (ViewerType == ViewerType.BODY && Entity.Type == EntityType.PC && isIdle) {
            return Entity.HeadDir;
        }

        if ((ViewerType == ViewerType.HEAD ||
            ViewerType == ViewerType.HEAD_TOP ||
            ViewerType == ViewerType.HEAD_MID ||
            ViewerType == ViewerType.HEAD_BOTTOM) && isIdle) {
            animCount = Math.Floor(animCount / 3);
            headDir = Entity.HeadDir;
        }

        if (AnimationHelper.IsLoopingMotion(CurrentMotion.Motion)) {
            frame = Math.Floor((double) (tm / delay));
            frame %= animCount;
            frame += animCount * headDir;
            frame += PreviousFrame;
            frame %= animCount;

            return (int) frame;
        }

        frame = Math.Min(tm / delay | 0, animCount);
        frame += (animCount * headDir);
        frame += PreviousFrame;

        if (ViewerType == ViewerType.BODY && frame >= animCount - 1) {
            PreviousFrame = frame = animCount - 1;

            if (CurrentMotion.delay > 0 && GameManager.Tick < CurrentMotion.delay) {
                if (NextMotion.HasValue) {
                    Entity.StartCoroutine(ChangeMotionAfter(NextMotion.Value, (float) (CurrentMotion.delay - GameManager.Tick) / 1000f));
                }
            } else {
                if (NextMotion.HasValue) {
                    Entity.ChangeMotion(NextMotion.Value);
                }
            }
        }

        return (int) Math.Min(frame, animCount - 1);
    }

    private IEnumerator ChangeMotionAfter(MotionRequest motion, float time) {
        yield return new WaitForSeconds(time);

        Entity.ChangeMotion(motion);
    }

    public float GetDelay() {
        return CurrentAction.delay;
    }

    public void OnMotionChanged(MotionRequest currentMotion, MotionRequest? nextMotion, int actionId) {
        PreviousFrame = 0;
        AnimationStart = GameManager.Tick;
        CurrentMotion = currentMotion;
        NextMotion = nextMotion;
        ActionId = actionId;
    }
}

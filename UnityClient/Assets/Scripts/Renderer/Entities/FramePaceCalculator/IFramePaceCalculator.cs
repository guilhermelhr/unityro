using ROIO.Models.FileTypes;
using static SpriteEntityViewer;

internal interface IFramePaceCalculator {
    void Init(Entity entity, ViewerType viewerType, ACT currentACT);
    int GetCurrentFrame();
    int GetActionIndex();
    float GetDelay();
    void OnMotionChanged(MotionRequest currentMotion, MotionRequest? nextMotion, int actionId);
}
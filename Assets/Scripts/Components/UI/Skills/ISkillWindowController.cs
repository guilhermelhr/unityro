public interface ISkillWindowController {

    void CheckSkillRequirements(short skillID, bool isTraversing = false);
    void HighlightSkill(short skillID, int level);
    bool HasRequiredSkill(short skillID, short level);
    void ResetSkillRequirements();
    void AllocateSkillPoints(short skillID);
    void UseSkill(short skillID, short level, int type);
}
public interface ISkillWindowController {

    void CheckSkillRequirements(short skillID, bool isTraversing = false);
    void HighlightSkill(short skillID, int level);
    bool HasRequiredSkill(short skillID, short level);
    bool IsRequirementsMet(short skillID);
    void ResetSkillRequirements();
}
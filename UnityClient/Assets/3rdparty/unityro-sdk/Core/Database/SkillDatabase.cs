using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/Skill")]
public class SkillsDatabase : ScriptableObject {
    public List<Skill> Values;
}

using System.Collections.Generic;
using Core.Effects.EffectParts;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/Effect")]
public class EffectDatabase : ScriptableObject {
    public List<Effect> Values;
}

using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CharAssembleInfo", menuName = "Scriptable Objects/CharAssembleInfo")]
public class CharAssembleInfo : ScriptableObject
{
    public CharStats charStats;
    public Dictionary<Ability,List<InputOptions.Input>> activeAbilInput;
    public List<Ability> otherAbils;
}

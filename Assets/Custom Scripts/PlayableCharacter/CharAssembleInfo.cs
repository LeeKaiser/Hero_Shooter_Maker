using UnityEngine;
using System.Collections.Generic;
using InputOptions;

[CreateAssetMenu(fileName = "CharAssembleInfo", menuName = "Scriptable Objects/CharAssembleInfo")]
public class CharAssembleInfo : ScriptableObject
{
    public CharStats Stats;
    
    public Dictionary<InputUnit,GameObject> ActiveAbilityInput; // gameobject must have child of ability component
    public List<GameObject> OtherAbilities; // gameobject must have child of ability component


}

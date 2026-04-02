using UnityEngine;
using System.Collections.Generic;
using InputOptions;

[CreateAssetMenu(fileName = "CharAssembleInfo", menuName = "Scriptable Objects/CharAssembleInfo")]
public class CharAssembleInfo : ScriptableObject
{
    public CharStats Stats;
    
    public Dictionary<InputUnit,GameObject> ActiveAbilityInput = new Dictionary<InputUnit,GameObject>(); // gameobject must have child of ability component
    public List<GameObject> OtherAbilities = new List<GameObject>() ; // gameobject must have child of ability component


}

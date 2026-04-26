using UnityEngine;
using System.Collections.Generic;
using InputOptions;


//Groups together abilities and associated input
[CreateAssetMenu(fileName = "ActiveAbilityGroup", menuName = "Scriptable Objects/ActiveAbilityGroup")]
public class ActiveAbilityGroup : ScriptableObject
{
    public List<GameObject> AbilityList = new List<GameObject>();
    public List<InputUnit> InputList = new List<InputUnit>();


    public void AddAllAbilities(AbilityManager manager)
    {
        for (int i = 0; i < AbilityList.Count; i++)
        {
            GameObject AbilityObject = Instantiate(AbilityList[i], manager.transform);
            ActiveAbility activeAbility = AbilityObject.GetComponent<ActiveAbility>();
            activeAbility.AbilityID = new ActiveAbilityID();
            manager.SetupInput(activeAbility, activeAbility.AbilityID, InputList[i]);
        }
        
    }

    public void RemoveAllAbilities(AbilityManager manager)
    {
        for (int i = 0; i < AbilityList.Count; i++)
        {
            manager.RemoveAbility(AbilityList[i].GetComponent<Ability>());
        }
    }
}

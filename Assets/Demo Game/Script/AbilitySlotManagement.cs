using UnityEngine;
using System.Collections.Generic;

//manages group of active abilitiy groups and only allows one of them to be added to the character at a time
public class AbilitySlotManagement : MonoBehaviour
{
    
    public List<ActiveAbilityGroup> allActiveAbilities = new List<ActiveAbilityGroup>();

    public void AddToAssember(CharAssembleInfo assemble, int index)
    {
        //check if it has ability that belongs to its own slot, remove it if it finds one
        ActiveAbilityGroup toRemove = null;
        foreach (ActiveAbilityGroup active in assemble.ActiveAbilityList)
        {
            if (allActiveAbilities.Contains(active))
            {
                toRemove = active;
            }
        }
        if (toRemove != null)
        {
            assemble.ActiveAbilityList.Remove(toRemove);
        }
        
        //add ability at index
        assemble.ActiveAbilityList.Add(allActiveAbilities[index]);
    }

}

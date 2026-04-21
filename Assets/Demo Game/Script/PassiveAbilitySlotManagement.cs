using UnityEngine;
using System.Collections.Generic;

public class PassiveAbilitySlotManagement : MonoBehaviour
{
    public List<GameObject> allAbilities = new List<GameObject>();
    [SerializeField] List<GameObject> allAbilitiesCopy = new List<GameObject>();

    void Awake()
    {
        foreach (GameObject abil in allAbilities)
        {
            GameObject abilCopy = Instantiate(abil);
            DontDestroyOnLoad(abilCopy);
            abilCopy.SetActive(false);
            allAbilitiesCopy.Add(abilCopy);
        }
    }
    public void AddToAssember(CharAssembleInfo assemble, int index)
    {
        //check if it has ability that belongs to its own slot, remove it if it finds one
        GameObject toRemove = null;
        foreach (GameObject abil in assemble.OtherAbilities)
        {
            if (allAbilitiesCopy.Contains(abil))
            {
                toRemove = abil;
            }
        }
        if (toRemove != null)
        {
            assemble.OtherAbilities.Remove(toRemove);
        }
        
        //add ability at index
        assemble.OtherAbilities.Add(allAbilitiesCopy[index]);
    }
}

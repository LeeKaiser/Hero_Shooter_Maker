using UnityEngine;
using InputOptions;
using System.Collections.Generic;

public class CharAssembler : MonoBehaviour
{
    public CharAssembleInfo assembleInfo;

    public CharCore playerReference;
    public AbilityManager abilityManager;

    void Awake()
    {
        //put char stat in char core
        playerReference.Stats = assembleInfo.Stats;
    }

    void Start()
    {
        //tie actives to input
        foreach (ActiveAbilityGroup abil in assembleInfo.ActiveAbilityList)
        {
            abil.AddAllAbilities(abilityManager);
        }
        //add other abilities
        foreach (GameObject abil in assembleInfo.OtherAbilities)
        {
            GameObject AbilityObject = Instantiate(abil, this.transform);
        }
    }
    
}

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
        Init();
    }
    public void Init()
    {
        //put char stat in char core
        playerReference.Stats = assembleInfo.Stats;

        //tie actives to input
        foreach (KeyValuePair<InputUnit,GameObject> abil in assembleInfo.ActiveAbilityInput)
        {
            GameObject AbilityObject = Instantiate(abil.Value, this.transform);
            ActiveAbility activeAbility = AbilityObject.GetComponent<ActiveAbility>();
            activeAbility.AbilityID = new ActiveAbilityID();
            abilityManager.SetupInput(activeAbility, activeAbility.AbilityID, abil.Key);
        }
        //add other abilities
        foreach (GameObject abil in assembleInfo.OtherAbilities)
        {
            GameObject AbilityObject = Instantiate(abil, this.transform);
        }
    }
}

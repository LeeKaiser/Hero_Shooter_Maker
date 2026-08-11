using UnityEngine;
using InputOptions;
using System.Collections.Generic;
using HeroShooterMaker.Abilities;

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
        //add all abilities
        foreach (GameObject abil in assembleInfo.Abilities)
        {
            GameObject AbilityObject = Instantiate(abil, this.transform);
            AbilityObject.SetActive(true);
        }
    }
    
}

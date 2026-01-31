using UnityEngine;
using System.Collections.Generic;
using System;
using AbilityClassification;

public struct PlayerSummary
{
    //summarized variables
    public GameObject summarizedPlayer;

    //health information
    public int remainingHP;
    public int maxHP;
    public float percentHP;

    //position information
    public bool aboveSelf;
    public float distanceFromSelf;

    //ability information
    public Dictionary <AbilityClass, float> abilChargeRemainPercent; 
    //ability charges remaining in percentage

    //threat information
    public float threatValue;
    public float vulnValue;

    public void SetValues(PlayableCharCore playerChar, AbilityManager abilManager, Transform playerTransform, Transform selfTransform)
    {
        summarizedPlayer = playerChar.gameObject;
        //set health info
        remainingHP = playerChar.GetHitPointsCurrent();
        maxHP = playerChar.GethitPointsCurrentMax();
        percentHP = remainingHP / maxHP;

        //set position info
        distanceFromSelf = (playerTransform.position - selfTransform.position).magnitude;
        aboveSelf = playerTransform.position.y > selfTransform.position.y;

        //set ability info
        
    }

    void SetAbilSummary(AbilityManager abilManager)
    {
        foreach (AbilityClass i in Enum.GetValues(typeof(AbilityClass))){
            abilChargeRemainPercent[i] = 0f;
        }
        
        foreach (Ability currentAbility in abilManager.GetAbilList())
        {
            float percentAbilChargeRemain = currentAbility.GetCurrentCharge() / currentAbility.GetCurrentMaxCharge();
            
            //add percent ability charge remaining to each part of dictionary 
            int tempMask = (int)currentAbility.CurrentAbilClass;
            while (tempMask != 0)
            {
                int lowestBit = tempMask & -tempMask;
                int index = Mathf.RoundToInt(Mathf.Log(lowestBit,2));

                abilChargeRemainPercent[(AbilityClass)(1 << index)] += percentAbilChargeRemain;

                tempMask &= ~lowestBit;
            }
        }
    }

    public string toString()
    {
        return $"\n remaining hp: {remainingHP} \n max hp: {maxHP} \n % health remaining: {percentHP} " + 
            $"\n distance from self: {distanceFromSelf} \n above self: {aboveSelf}";
    }
}

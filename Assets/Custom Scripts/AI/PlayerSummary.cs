using UnityEngine;
using System.Collections.Generic;
using System;
using AbilityClassification;

public class PlayerSummary
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

    //expiration details
    public float timeUntilExpire;

    void Start()
    {
        foreach (AbilityClass a in Enum.GetValues(typeof(AbilityClass)))
        {
            if (a == AbilityClass.None) continue;
            abilChargeRemainPercent[a] = 0f;
        }
    }

    public void SetValues(PlayableCharCore playerChar, AbilityManager abilManager, Transform playerTransform, Transform selfTransform, float memoryExpirationTime)
    {
        timeUntilExpire = memoryExpirationTime;

        summarizedPlayer = playerChar.gameObject;
        //set health info
        remainingHP = playerChar.GetHitPointsCurrent();
        maxHP = playerChar.GethitPointsCurrentMax();
        percentHP = (float)remainingHP / (float)maxHP;
        //Debug.Log("set hp info" + $"\n remaining hp: {remainingHP} \n max hp: {maxHP} \n % health remaining: {percentHP} ");

        //set position info
        distanceFromSelf = (playerTransform.position - selfTransform.position).magnitude;
        aboveSelf = playerTransform.position.y > selfTransform.position.y;
        //Debug.Log("set pos info");

        //set ability info
        SetAbilSummary(abilManager);
        //Debug.Log("set abil info" + $"\n abil summary: {abilManager}");
    }

    void SetAbilSummary(AbilityManager abilManager)
    {
        abilChargeRemainPercent = new Dictionary<AbilityClass, float>();
        foreach (AbilityClass i in Enum.GetValues(typeof(AbilityClass))){
            abilChargeRemainPercent[i] = 0f;
        }

        if (abilManager == null)
        {
            Debug.Log("no ability list");
            return;
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

        foreach (AbilityClass a in Enum.GetValues(typeof(AbilityClass)))
        {
            if (abilChargeRemainPercent[a] > 0)
            {
                abilChargeRemainPercent[a] = abilChargeRemainPercent[a] / abilManager.GetAbilClassDict()[a];
            }
        }
    }

    public void SubtractTimeRemaining(float timeElapsed)
    {
        timeUntilExpire -= timeElapsed;
    }

    public string toString()
    {
        string abilChargeStr = "";
        foreach (AbilityClass i in Enum.GetValues(typeof(AbilityClass)))
        {
            abilChargeStr += i + ": " + abilChargeRemainPercent[i] + $"\n";
        }

        return $"\n remaining hp: {remainingHP} \n max hp: {maxHP} \n % health remaining: {percentHP} " + 
            $"\n distance from self: {distanceFromSelf} \n above self: {aboveSelf}" +
            $"\n abil summary: {abilChargeStr} time remaining in memory: {timeUntilExpire} \n";
    }
}

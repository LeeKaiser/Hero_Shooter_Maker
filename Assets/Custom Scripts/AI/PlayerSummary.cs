using UnityEngine;
using System.Collections.Generic;
using System;
using AbilityClassification;
using System.Linq;

public class PlayerSummary
{
    //summarized variables
    public GameObject summarizedPlayer;

    //health information
    public int remainingHP;
    public int maxHP;
    public float percentHP;

    [SerializeField] float hpThreatMult = 4f;
    [SerializeField] float hpVulnMult = 12f;

    //position information
    public bool aboveSelf;
    public float distanceFromSelf;
    [SerializeField] float highGndThreatMult = 1.5f;
    [SerializeField] float highGndVulnMult = 1.5f;

    //ability information
    public Dictionary <AbilityClass, float> abilChargeRemainPercent;
    //ability charges remaining in percentage
    public Dictionary <AbilityClass, int> hasAbilClass; //0f is false, 1f is true

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
            //hasAbilClass[a] = 0f;
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

        CalculateThreat();
        CalculateVuln();
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
            if (a == AbilityClass.None) continue;
            if (abilManager.GetAbilClassDict()[a] > 0)
            {
                abilChargeRemainPercent[a] = abilChargeRemainPercent[a] / abilManager.GetAbilClassDict()[a];
                
            }
            
        }

        hasAbilClass = abilManager.GetAbilClassDict().ToDictionary(
            pair => pair.Key,
            pair => pair.Value > 0 ? 1 : 0
            );
    }

    public void SubtractTimeRemaining(float timeElapsed)
    {
        timeUntilExpire -= timeElapsed;
    }

    //creates an arbitrary estimation of the likelyhood of this player to defeat other players based on known factors for simplification in decision making.
    public void CalculateThreat()
    {
        float totalPossibleThreat = 0;
        float currentThreat = 0;

        
        currentThreat += (percentHP * hpThreatMult) + (highGndThreatMult * Convert.ToSingle(aboveSelf)) + 
            abilChargeRemainPercent[AbilityClass.Active] + abilChargeRemainPercent[AbilityClass.Damage] +
            abilChargeRemainPercent[AbilityClass.SelfBoost] + abilChargeRemainPercent[AbilityClass.SelfSave] + 
            abilChargeRemainPercent[AbilityClass.MobilEng] + abilChargeRemainPercent[AbilityClass.Skirmish] + 
            abilChargeRemainPercent[AbilityClass.Shutdown] + abilChargeRemainPercent[AbilityClass.Parry];
        totalPossibleThreat += hpThreatMult + highGndThreatMult + hasAbilClass[AbilityClass.Active] + 
            hasAbilClass[AbilityClass.Damage] + hasAbilClass[AbilityClass.SelfBoost] +
            hasAbilClass[AbilityClass.SelfSave] + hasAbilClass[AbilityClass.MobilEng] +
            hasAbilClass[AbilityClass.Skirmish] + hasAbilClass[AbilityClass.Shutdown] +
            hasAbilClass[AbilityClass.Parry];

        if (totalPossibleThreat == 0) totalPossibleThreat = 1;

        threatValue = currentThreat / totalPossibleThreat;
    }

    //creates an arbitrary estimation of the likelyhood of this player be defeated by other players based on known factors for simplification in decision making.
    public void CalculateVuln()
    {
        float totalPossibleVuln = 0;
        float currentVuln = 0;

        
        currentVuln += (percentHP * hpVulnMult) + (highGndVulnMult * Convert.ToSingle(aboveSelf)) + 
            abilChargeRemainPercent[AbilityClass.Active] + abilChargeRemainPercent[AbilityClass.Damage] +
            abilChargeRemainPercent[AbilityClass.LongTermPet] + abilChargeRemainPercent[AbilityClass.SelfSave] + 
            abilChargeRemainPercent[AbilityClass.MobilDis] + abilChargeRemainPercent[AbilityClass.Zoning] + 
            abilChargeRemainPercent[AbilityClass.Shutdown] + abilChargeRemainPercent[AbilityClass.Parry];
        totalPossibleVuln += hpVulnMult + highGndVulnMult + hasAbilClass[AbilityClass.Active] + 
            hasAbilClass[AbilityClass.Damage] + hasAbilClass[AbilityClass.LongTermPet] + 
            hasAbilClass[AbilityClass.SelfSave] + hasAbilClass[AbilityClass.MobilDis] +
            hasAbilClass[AbilityClass.Zoning] + hasAbilClass[AbilityClass.Shutdown] +
            hasAbilClass[AbilityClass.Parry];

        if (totalPossibleVuln == 0) totalPossibleVuln = 1;

        vulnValue = (totalPossibleVuln - currentVuln) / totalPossibleVuln;
    }

    public string toString()
    {
        string abilChargeStr = "";
        foreach (AbilityClass i in Enum.GetValues(typeof(AbilityClass)))
        {
            abilChargeStr += i + ": " + abilChargeRemainPercent[i] + $"\n";
        }

        return $"\nremaining hp: {remainingHP} \nmax hp: {maxHP} \n% health remaining: {percentHP} " + 
            $"\ndistance from self: {distanceFromSelf} \nabove self: {aboveSelf}" +
            $"\nabil summary: {abilChargeStr}" + 
            $"Threat Value: {threatValue} \nVuln Value: {vulnValue}" +
            $"\ntime remaining in memory: {timeUntilExpire} \n";
    }
}

using UnityEngine;
using System.Collections.Generic;
using System;
using AbilityClassification;
using System.Linq;

/*
PlayerSummary
concise version of player's informations
*/
public class PlayerSummary
{
    //summarized variables
    public GameObject SummarizedPlayer;

    //health information
    public int RemainingHP;
    public int MaxHP;
    public float PercentHP;

    [SerializeField] float hpThreatMultiplier = 4f;
    [SerializeField] float hpVulnerabilityMultiplier = 12f;

    //position information
    public bool AboveSelf;
    public float DistanceFromSelf;
    [SerializeField] float highGroundThreatMultiplier = 1.5f;
    [SerializeField] float highGroundVulnerabilityMultiplier = 1.5f;

    //ability information
    public Dictionary <AbilityClass, float> AbililityChargeRemainPercent;
    //ability charges remaining in percentage
    public Dictionary <AbilityClass, int> HasAbilityClass; //0f is false, 1f is true

    //threat information
    public float ThreatValue;
    public float VulnerabilityValue;

    //expiration details
    public float TimeUntilExpire;

    void Start()
    {
        foreach (AbilityClass a in Enum.GetValues(typeof(AbilityClass)))
        {
            if (a == AbilityClass.None) continue;
            AbililityChargeRemainPercent[a] = 0f;
            //HasAbilityClass[a] = 0f;
        }
    }

    public void SetValues(CharCore character, AbilityManager abilManager, Transform playerTransform, Transform selfTransform, float memoryExpirationTime)
    {
        TimeUntilExpire = memoryExpirationTime;

        SummarizedPlayer = character.gameObject;
        //set health info
        RemainingHP = character.GetHitPointsCurrent();
        MaxHP = character.GetHitPointsBase();
        PercentHP = (float)RemainingHP / (float)MaxHP;
        //Debug.Log("set hp info" + $"\n remaining hp: {RemainingHP} \n max hp: {MaxHP} \n % health remaining: {PercentHP} ");

        //set position info
        DistanceFromSelf = (playerTransform.position - selfTransform.position).magnitude;
        AboveSelf = playerTransform.position.y > selfTransform.position.y;
        //Debug.Log("set pos info");

        //set ability info
        SetAbilSummary(abilManager);
        //Debug.Log("set abil info" + $"\n abil summary: {abilManager}");

        CalculateThreat();
        CalculateVuln();
    }

    void SetAbilSummary(AbilityManager abilManager)
    {
        AbililityChargeRemainPercent = new Dictionary<AbilityClass, float>();
        foreach (AbilityClass i in Enum.GetValues(typeof(AbilityClass))){
            AbililityChargeRemainPercent[i] = 0f;
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

                AbililityChargeRemainPercent[(AbilityClass)(1 << index)] += percentAbilChargeRemain;

                tempMask &= ~lowestBit;
            }
        }

        foreach (AbilityClass a in Enum.GetValues(typeof(AbilityClass)))
        {
            if (a == AbilityClass.None) continue;
            if (abilManager.GetAbilityClassDictionary()[a] > 0)
            {
                AbililityChargeRemainPercent[a] = AbililityChargeRemainPercent[a] / abilManager.GetAbilityClassDictionary()[a];
                
            }
            
        }

        HasAbilityClass = abilManager.GetAbilityClassDictionary().ToDictionary(
            pair => pair.Key,
            pair => pair.Value > 0 ? 1 : 0
            );
    }

    public void SubtractTimeRemaining(float timeElapsed)
    {
        TimeUntilExpire -= timeElapsed;
    }

    //creates an arbitrary estimation of the likelyhood of this player to defeat other players based on known factors for simplification in decision making.
    public void CalculateThreat()
    {
        float totalPossibleThreat = 0;
        float currentThreat = 0;

        
        currentThreat += (PercentHP * hpThreatMultiplier) + (highGroundThreatMultiplier * Convert.ToSingle(AboveSelf)) + 
            AbililityChargeRemainPercent[AbilityClass.Active] + AbililityChargeRemainPercent[AbilityClass.Damage] +
            AbililityChargeRemainPercent[AbilityClass.SelfBoost] + AbililityChargeRemainPercent[AbilityClass.SelfSave] + 
            AbililityChargeRemainPercent[AbilityClass.MobilEng] + AbililityChargeRemainPercent[AbilityClass.Skirmish] + 
            AbililityChargeRemainPercent[AbilityClass.Shutdown] + AbililityChargeRemainPercent[AbilityClass.Parry];
        totalPossibleThreat += hpThreatMultiplier + highGroundThreatMultiplier + HasAbilityClass[AbilityClass.Active] + 
            HasAbilityClass[AbilityClass.Damage] + HasAbilityClass[AbilityClass.SelfBoost] +
            HasAbilityClass[AbilityClass.SelfSave] + HasAbilityClass[AbilityClass.MobilEng] +
            HasAbilityClass[AbilityClass.Skirmish] + HasAbilityClass[AbilityClass.Shutdown] +
            HasAbilityClass[AbilityClass.Parry];

        if (totalPossibleThreat == 0) totalPossibleThreat = 1;

        ThreatValue = currentThreat / totalPossibleThreat;
    }

    //creates an arbitrary estimation of the likelyhood of this player be defeated by other players based on known factors for simplification in decision making.
    public void CalculateVuln()
    {
        float totalPossibleVuln = 0;
        float currentVuln = 0;

        
        currentVuln += (PercentHP * hpVulnerabilityMultiplier) + (highGroundVulnerabilityMultiplier * Convert.ToSingle(AboveSelf)) + 
            AbililityChargeRemainPercent[AbilityClass.Active] + AbililityChargeRemainPercent[AbilityClass.Damage] +
            AbililityChargeRemainPercent[AbilityClass.LongTermPet] + AbililityChargeRemainPercent[AbilityClass.SelfSave] + 
            AbililityChargeRemainPercent[AbilityClass.MobilDis] + AbililityChargeRemainPercent[AbilityClass.Zoning] + 
            AbililityChargeRemainPercent[AbilityClass.Shutdown] + AbililityChargeRemainPercent[AbilityClass.Parry];
        totalPossibleVuln += hpVulnerabilityMultiplier + highGroundVulnerabilityMultiplier + HasAbilityClass[AbilityClass.Active] + 
            HasAbilityClass[AbilityClass.Damage] + HasAbilityClass[AbilityClass.LongTermPet] + 
            HasAbilityClass[AbilityClass.SelfSave] + HasAbilityClass[AbilityClass.MobilDis] +
            HasAbilityClass[AbilityClass.Zoning] + HasAbilityClass[AbilityClass.Shutdown] +
            HasAbilityClass[AbilityClass.Parry];

        if (totalPossibleVuln == 0) totalPossibleVuln = 1;

        VulnerabilityValue = (totalPossibleVuln - currentVuln) / totalPossibleVuln;
    }

    public string toString()
    {
        string abilChargeStr = "";
        foreach (AbilityClass i in Enum.GetValues(typeof(AbilityClass)))
        {
            abilChargeStr += i + ": " + AbililityChargeRemainPercent[i] + $"\n";
        }

        return $"\nremaining hp: {RemainingHP} \nmax hp: {MaxHP} \n% health remaining: {PercentHP} " + 
            $"\ndistance from self: {DistanceFromSelf} \nabove self: {AboveSelf}" +
            $"\nabil summary: {abilChargeStr}" + 
            $"Threat Value: {ThreatValue} \nVuln Value: {VulnerabilityValue}" +
            $"\ntime remaining in memory: {TimeUntilExpire} \n";
    }
}

using UnityEngine;
using DecisionCondition;
using System.Collections.Generic;
using System.Collections;
using AbilityClassification;

/*
Condition Check
Static class containing a method to check if a condition is true or false. 
*/
public static class ConditionCheck
{
    public static bool CheckIfConditionTrue(decisionCondition currentCondition, float parameterFloat, KnownContext context)
    {
        switch (currentCondition)
        {
            case decisionCondition.EnemyPresent:
                return EnemyPresentCondition(context);
            case decisionCondition.EnemyClose:
                return EnemyCloseCondition(context,parameterFloat);
            case decisionCondition.TeammatePresent:
                return TeammatePresentConditiion(context);
            case decisionCondition.Random:
                return RandomCondition(parameterFloat);
            case decisionCondition.TeammateLowHp:
                return TeammateLowHpCondition(context, parameterFloat);
            case decisionCondition.SelfLowHp:
                return SelfLowHpCondition(context, parameterFloat);
            case decisionCondition.HasActiveAbilityOfClass:
                return HasActiveAbilityOfClassCondition(context, parameterFloat);
            case decisionCondition.TeamHasAdvantage:
                return TeamHasAdvantageCondition(context, parameterFloat);
            case decisionCondition.EnemyNearPointOfInterest:
                return EnemyNearPointOfInterestCondition(context, parameterFloat);
            default:
                return false;
        }
    }

    //EnemyPresent
    //if there is an enemy detected in the context's enemy list, return true
    private static bool EnemyPresentCondition(KnownContext context)
    {
        return context.KnownEnemyList.Count > 0;
    }

    //teammate present
    //if there is a teammate detected in the context's ally list, return true
    private static bool TeammatePresentConditiion(KnownContext context)
    {
        return context.KnownAllyList.Count > 0;
    }

    //enemy close
    //if there is an enemy that is within the parameter float's distance, return true
    private static bool EnemyCloseCondition(KnownContext context, float parameterFloat)
    {
        if (context.KnownEnemyList.Count > 0) 
        {
            foreach (KeyValuePair<CharCore, PlayerSummary> x in context.KnownEnemyList)
            {
                if (x.Value.DistanceFromSelf < parameterFloat)
                {
                    return true;
                }
            }
            return false;
        }
        else{return false;}
    }

    //random condition
    //if a randomly generated number is less than or equal to the parameter float, return true
    private static bool RandomCondition(float parameterFloat)
    {
        return parameterFloat <= Random.Range(0f,1f);
    }

    //teammate low hp
    //if there is a teammate with remaining hp percentage lower than or equal to the parameter float, return true
    private static bool TeammateLowHpCondition(KnownContext context, float parameterFloat)
    {
        if (context.KnownAllyList.Count > 0) 
        {
            foreach (KeyValuePair<CharCore, PlayerSummary> x in context.KnownAllyList)
            {
                if (x.Value.PercentHP < parameterFloat)
                {
                    return true;
                }
            }
            return false;
        }
        else{return false;}
    }

    //self low hp 
    //if self's remaining hp percentage is lower than or equal to parameter float, return true;
    private static bool SelfLowHpCondition(KnownContext context, float parameterFloat)
    {
        if (context.SelfSummary != null) 
        {
            
            if (context.SelfSummary.PercentHP < parameterFloat)
            {
                return true;
            }
            
            return false;
        }
        else{return false;}
    }

    //has active ability of class
    //return true if there are any ability of a class index (based on parameter float which is converted to enum)
    private static bool HasActiveAbilityOfClassCondition(KnownContext context, float parameterFloat)
    {
        if (context.SelfSummary != null) 
        {
            
            if (context.SelfSummary.AbililityChargeRemainPercent[(AbilityClass)(int)parameterFloat] > 0)
            {
                return true;
            }
            
            return false;
        }
        else{return false;}
    }

    //team has advantage
    //returns true if sum of all known ally's threat is greater than all enemy's vuln
    private static bool TeamHasAdvantageCondition(KnownContext context, float parameterFloat)
    {
        float teamAdvantage = 0;
        float enemyAdvantage = 0;
        teamAdvantage += context.SelfSummary.ThreatValue - context.SelfSummary.VulnerabilityValue;
        foreach (KeyValuePair<CharCore, PlayerSummary> x in context.KnownAllyList)
        {
            teamAdvantage += x.Value.ThreatValue - x.Value.VulnerabilityValue;
        }
        foreach (KeyValuePair<CharCore, PlayerSummary> x in context.KnownEnemyList)
        {
            enemyAdvantage += x.Value.ThreatValue - x.Value.VulnerabilityValue;
        }

        Debug.Log($"team adv: {teamAdvantage} vs enemy adv: {enemyAdvantage}");
        float totalAdvantage = teamAdvantage - enemyAdvantage;
        return totalAdvantage >= parameterFloat;
    }

    //enemy near point of interest
    //return true if the enemy is within parameter float of a poi it is focused on
    private static bool EnemyNearPointOfInterestCondition(KnownContext context, float parameterFloat)
    {
        if (context.KnownEnemyList.Count == 0 || context.FocusPOI == null)
        {
            //no enemies or point of interest detected
            return false;
        }

        //check if there is an enemy that is close to point of interest
        foreach (KeyValuePair<CharCore, PlayerSummary> x in context.KnownEnemyList)
        {
            Vector3 enemyPosition = x.Key.PlayerArmature.transform.position;
            float distanceFromPoint = (context.FocusPOI.transform.position - enemyPosition).magnitude;
            if (distanceFromPoint <= parameterFloat) return true;
        }
        return false;
    }
}

namespace DecisionCondition
{
    //add more condition as nessecary
    public enum decisionCondition
    {
        EnemyPresent,
        EnemyClose,
        TeammatePresent,
        TeammateLowHp,
        TeamHasAdvantage,
        SelfLowHp,
        HasActiveAbilityOfClass,
        EnemyNearPointOfInterest,
        Random,
    }
}
using UnityEngine;
using DecisionCondition;
using System.Collections.Generic;
using System.Collections;

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
            default:
                return false;
        }
    }

    private static bool EnemyPresentCondition(KnownContext context)
    {
        return context.KnownEnemyList.Count > 0;
    }

    private static bool TeammatePresentConditiion(KnownContext context)
    {
        return context.KnownAllyList.Count > 0;
    }

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

    private static bool RandomCondition(float parameterFloat)
    {
        return parameterFloat <= Random.Range(0f,1f);
    }
}

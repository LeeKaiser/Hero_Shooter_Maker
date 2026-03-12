using UnityEngine;
using DecisionCondition;
using System.Collections.Generic;
using System.Collections;

/*
ConditionCheck
static class to check if condition is true/false
*/
public static class ConditionCheck
{
    //Method
    //return true if the condition in param is true, false otherwise.
    //use parameter to control output if applicable
    public static bool CheckIfConditionTrue(decisionCondition currentCondition, float parameterFloat, KnownContext context)
    {
        switch (currentCondition)
        {
            case decisionCondition.EnemyPresent:
                if (context.knownEnemyList.Count > 0) {return true;}
                else{return false;}
            case decisionCondition.EnemyClose:
                if (context.knownEnemyList.Count > 0) 
                {
                    foreach (KeyValuePair<PlayableCharCore, PlayerSummary> x in context.knownEnemyList)
                    {
                        if (x.Value.distanceFromSelf < parameterFloat)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                else{return false;}
            case decisionCondition.TeammatePresent:
                if (context.knownAllyList.Count > 0) {return true;}
                else{return false;}
            case decisionCondition.Random:
                return false; //TODO: implement random. generate random number between 0 and 1 and if it is less than parameter, return true
            default:
                return false;
        }
    }
}

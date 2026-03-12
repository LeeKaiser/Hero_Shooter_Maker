using UnityEngine;
using DecisionCondition;
using System.Collections.Generic;
using System.Collections;

public static class ConditionCheck
{
    

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
            
            default:
                return false;
        }
    }
}

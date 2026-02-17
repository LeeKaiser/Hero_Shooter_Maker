using UnityEngine;
using DecisionCondition;

public static class ConditionCheck
{
    

    public static bool CheckIfConditionTrue(decisionCondition currentCondition, KnownContext context)
    {
        switch (currentCondition)
        {
            case decisionCondition.EnemyPresent:
                return false;
            case decisionCondition.EnemyClose:
                return false;
            case decisionCondition.TeammatePresent:
                return false;
            
            default:
                return false;
        }
    }
}

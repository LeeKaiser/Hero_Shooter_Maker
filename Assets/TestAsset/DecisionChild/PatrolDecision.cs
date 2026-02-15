using UnityEngine;

public class PatrolDecision : Decision
{
    public override float ScoreDecision(KnownContext currentContext)
    {
        float decisionValidity = 0f;
        if (currentContext.knownEnemyList.Count == 0)
        {
            decisionValidity = 1f;
        }

        return decisionValidity;
    }
}

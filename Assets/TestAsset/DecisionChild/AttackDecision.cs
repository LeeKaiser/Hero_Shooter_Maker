using UnityEngine;

public class AttackDecision : Decision
{
    public override float ScoreDecision(KnownContext currentContext)
    {
        float decisionValidity = 0f;
        float highestPossibleValidity = 0f;
        if (currentContext.knownEnemyList.Count > 0)
        {
            decisionValidity = 1f;
            highestPossibleValidity = 1f;
        }

        if (highestPossibleValidity == 0f) {highestPossibleValidity = 1f;}

        return decisionValidity / highestPossibleValidity;
    }
}

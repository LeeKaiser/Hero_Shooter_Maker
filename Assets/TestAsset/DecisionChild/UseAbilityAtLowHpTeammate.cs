using UnityEngine;
using AbilityClassification;
using System.Collections.Generic;
using InputOptions;
[CreateAssetMenu(menuName = "AIAction/AbilityAtAlly")]

public class UseAbilityAtLowHpTeammate : UseAbilityAtEnemy
{
    public override void DetermineMovement()
    {
        if (!(Detection.GetCurrentContext().KnownAllyList == null))
        {
            // identify weakest enemy
            if (targetPlayer == null)
            {
                float lowestHp = 1;
                foreach (KeyValuePair<CharCore, PlayerSummary> potentialTarget in Detection.GetCurrentContext().KnownAllyList)
                {
                    if (potentialTarget.Value.PercentHP <= lowestHp)
                    {
                        targetPlayer = potentialTarget.Key.PlayerArmature;
                        lowestHp = potentialTarget.Value.PercentHP;
                    }
                }
            }

            //set distance based on ability to use
            if (abilityToUse != null)
            {
                distanceFromEnemy = abilityToUse.MinimumRange + ((abilityToUse.MaximumRange - abilityToUse.MinimumRange) / 2);
                randomDistanceTweak = abilityToUse.MaximumRange - abilityToUse.MinimumRange;
            }

            Vector3 nextDestination = targetPlayer.transform.position;

            Vector3 allyToSelf =  playerArmature.transform.position - targetPlayer.transform.position;
            Quaternion randomRot = Quaternion.AngleAxis(Random.Range(-randomAngleTweak,randomAngleTweak),Vector3.up);
            nextDestination = nextDestination + (randomRot * allyToSelf.normalized * (distanceFromEnemy + Random.Range(-randomDistanceTweak,randomDistanceTweak)));
            //Debug.Log(nextDestination);
            MoveTarget.position = nextDestination;
            Movement.MoveToLocation();
        }
    }
}

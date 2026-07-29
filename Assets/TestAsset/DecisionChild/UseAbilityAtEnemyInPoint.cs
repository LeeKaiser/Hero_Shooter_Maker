using UnityEngine;
using System.Collections.Generic;
using System;
[CreateAssetMenu(menuName = "AIAction/AbilityAtEnemyInPoint")]
public class UseAbilityAtEnemyInPoint : UseAbilityAtEnemy
{
    public override void DetermineMovement()
    {
        if (!(Detection.GetCurrentContext().KnownEnemyList == null))
        {
            Vector3 pointLocation = playerArmature.transform.position;
            if (Detection.GetCurrentContext().focusPOIList != null)
            {
                int highestPriority = 0;
                foreach (PatrolLandmark x in Detection.GetCurrentContext().focusPOIList)
                {
                    if (x.PatrolPriority[0] > highestPriority)
                    {
                        pointLocation = x.transform.position;
                        highestPriority = x.PatrolPriority[0];
                    }
                }
                
            } 
            // identify weakest enemy
            float lowestDistance = Mathf.Infinity;
            foreach (KeyValuePair<CharCore, PlayerSummary> potentialTarget in Detection.GetCurrentContext().KnownEnemyList)
            {
                float dist = (potentialTarget.Key.PlayerArmature.transform.position - pointLocation).magnitude;
                if (dist <= lowestDistance)
                {
                    targetPlayer = potentialTarget.Key.PlayerArmature;
                    lowestDistance = dist;
                }
            }
            

            //set distance based on ability to use
            if (abilityToUse != null)
            {
                distanceFromEnemy = abilityToUse.MinimumRange + ((abilityToUse.MaximumRange - abilityToUse.MinimumRange) / 2);
                randomDistanceTweak = abilityToUse.MaximumRange - abilityToUse.MinimumRange;
            }

            try
            {
                Vector3 nextDestination = targetPlayer.transform.position;

                Vector3 enemyToSelf =  playerArmature.transform.position - targetPlayer.transform.position;
                Quaternion randomRot = Quaternion.AngleAxis(UnityEngine.Random.Range(-randomAngleTweak,randomAngleTweak),Vector3.up);
                nextDestination = nextDestination + (randomRot * enemyToSelf.normalized * (distanceFromEnemy + UnityEngine.Random.Range(-randomDistanceTweak,randomDistanceTweak)));
                MoveTarget.position = nextDestination;
                Movement.MoveToLocation();
            }
            //Debug.Log(nextDestination);
            catch (Exception e)
            {
                base.DetermineMovement();
                Debug.Log(e);
            }
            
        }
        else
        {
            base.DetermineMovement();
        }
    }
}

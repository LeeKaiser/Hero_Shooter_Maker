using UnityEngine;
using System;
using HeroShooterMaker.AI;

//DetermineMovementPrefRange
//Example of an overriden DetermineMovement for the demo.
//Choose to move to a distance away from character based on the ability's ideal range
namespace HeroShooterMakerDemo
{
    [CreateAssetMenu(fileName = "PrefRange", menuName = "AIAction/Movement/MovePrefRange")]
    public class DetermineMovementPrefRange : DetermineMovement
    {
        public float randomAngleTweak = 10f;
        public float randomDistanceTweak = 2f;
        public override void ExecuteDetermineMovement(AIAction action)
        {
            //set distance based on ability to use
            if (action.abilityToUse != null)
            {
                action.distanceFromEnemy = action.abilityToUse.MinimumRange + ((action.abilityToUse.MaximumRange - action.abilityToUse.MinimumRange) / 2);
                randomDistanceTweak = action.abilityToUse.MaximumRange - action.abilityToUse.MinimumRange;
            }

            try
            {
                Vector3 nextDestination = action.targetPlayer.transform.position;

                Vector3 enemyToSelf = action.playerArmature.transform.position - action.targetPlayer.transform.position;
                Quaternion randomRot = Quaternion.AngleAxis(UnityEngine.Random.Range(-randomAngleTweak, randomAngleTweak), Vector3.up);
                nextDestination = nextDestination + (randomRot * enemyToSelf.normalized * (action.distanceFromEnemy + UnityEngine.Random.Range(-randomDistanceTweak, randomDistanceTweak)));
                action.MoveTarget.position = nextDestination;
                action.Movement.MoveToLocation();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}
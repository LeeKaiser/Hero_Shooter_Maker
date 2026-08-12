using UnityEngine;
using System.Collections.Generic;

//DetermineMovementToTeammate
//Example of an overriden DetermineMovement for the demo.
//Choose to move close to teammate
namespace HeroShooterMaker.AI
{
    [CreateAssetMenu(fileName = "ToTeammate", menuName = "AIAction/Movement/MoveToTeammate")]
    public class DetermineMovementToTeammate : DetermineMovement
    {
        public float destinationSpread = 1;
        public override void ExecuteDetermineMovement(AIAction action)
        {
            GameObject playerArmatureRef = action.Detection.GetCurrentContext().PlayerReference.GetComponent<CharCore>().PlayerArmature;
            Vector3 nextDestination = playerArmatureRef.transform.position;


            if (action.Detection.GetCurrentContext().KnownAllyList.Count >= 1)
            {
                float highestVuln = 0;
                foreach (KeyValuePair<CharCore, PlayerSummary> potentialTarget in action.Detection.GetCurrentContext().KnownAllyList)
                {
                    //if (potentialTarget.Key.PlayerArmature == playerArmatureRef){ continue;}
                    if (potentialTarget.Value.VulnerabilityValue >= highestVuln)
                    {
                        nextDestination = potentialTarget.Key.PlayerArmature.transform.position;
                        highestVuln = potentialTarget.Value.VulnerabilityValue;
                    }
                }
            }

            //choose a random position and move to it
            nextDestination.x = nextDestination.x + Random.Range(destinationSpread, destinationSpread);
            nextDestination.z = nextDestination.z + Random.Range(destinationSpread, destinationSpread);
            action.MoveTarget.position = nextDestination;
            action.Movement.MoveToLocation();
        }
    }
}
using UnityEngine;
using System.Collections.Generic;
using System;
using HeroShooterMaker.Character;
using HeroShooterMaker.AI;

//DetermineAimAllyVuln
//Example of an overriden DetermineAim for the demo.
//causes the agent to aim at a teammate who is most vulnerable
namespace HeroShooterMakerDemo
{
    [CreateAssetMenu(fileName = "TargetAllyVuln", menuName = "AIAction/Aim/TargetAllyVuln")]
    public class DetermineAimAllyVuln : DetermineAim
    {
        public override void ExecuteDetermineAim(AIAction action)
        {
            if (!(action.Detection.GetCurrentContext().KnownAllyList == null))
            {
                // identify weakest teammate
                float highestVuln = 0;
                foreach (KeyValuePair<CharCore, PlayerSummary> potentialTarget in action.Detection.GetCurrentContext().KnownAllyList)
                {
                    if (potentialTarget.Value.VulnerabilityValue >= highestVuln)
                    {
                        action.targetPlayer = potentialTarget.Key.PlayerArmature;
                        highestVuln = potentialTarget.Value.VulnerabilityValue;
                    }
                }
            }

            try
            {
                Vector3 targetPosition = action.targetPlayer.transform.position;
                float heightAdjustment = action.targetPlayer.GetComponent<CharacterController>().height * 0.8f;
                targetPosition.y += heightAdjustment;
                action.AimTarget.position = targetPosition;
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}

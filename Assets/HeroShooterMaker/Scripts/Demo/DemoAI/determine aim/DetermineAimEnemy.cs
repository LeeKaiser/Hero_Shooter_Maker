using UnityEngine;
using System.Collections.Generic;
using System;
using HeroShooterMaker.Character;
using HeroShooterMaker.AI;

//DetermineAimEnemyVuln
//Example of an overriden DetermineAim for the demo.
//Causes the agent to aim at enemy that is most vulnerable
namespace HeroShooterMakerDemo
{
    [CreateAssetMenu(fileName = "EnemyVuln", menuName = "AIAction/Aim/TargetEnemyVuln")]
    public class DetermineAimEnemyVuln : DetermineAim
    {
        public override void ExecuteDetermineAim(AIAction action)
        {
            if (!(action.Detection.GetCurrentContext().KnownEnemyList == null))
            {
                // identify weakest enemy
                float highestVuln = 0;
                foreach (KeyValuePair<CharCore, PlayerSummary> potentialTarget in action.Detection.GetCurrentContext().KnownEnemyList)
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
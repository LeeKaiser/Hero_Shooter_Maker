using UnityEngine;
using System.Collections.Generic;
using System;
using HeroShooterMaker.Character;
using HeroShooterMaker.AI;

//DetermineAimEnemyPoint
//Example of an overriden DetermineAim for the demo.
//Causes the agent to aim at enemy that is closest to highest priority patrol point
namespace HeroShooterMakerDemo
{
    [CreateAssetMenu(fileName = "TargetEnemyPoint", menuName = "AIAction/Aim/TargetEnemyPoint")]
    public class DetermineAimEnemyPoint : DetermineAim
    {
        public override void ExecuteDetermineAim(AIAction action)
        {
            if (!(action.Detection.GetCurrentContext().KnownEnemyList == null))
            {
                //find highest priority point of interest
                Vector3 pointLocation = action.playerArmature.transform.position;
                if (action.Detection.GetCurrentContext().focusPOIList != null)
                {
                    int highestPriority = 0;
                    foreach (PatrolLandmark x in action.Detection.GetCurrentContext().focusPOIList)
                    {
                        if (x.PatrolPriority[0] > highestPriority)
                        {
                            pointLocation = x.transform.position;
                            highestPriority = x.PatrolPriority[0];
                        }
                    }
                    
                } 
                // identify enemy closest to it
                float lowestDistance = Mathf.Infinity;
                foreach (KeyValuePair<CharCore, PlayerSummary> potentialTarget in action.Detection.GetCurrentContext().KnownEnemyList)
                {
                    float dist = (potentialTarget.Key.PlayerArmature.transform.position - pointLocation).magnitude;
                    if (dist <= lowestDistance)
                    {
                        action.targetPlayer = potentialTarget.Key.PlayerArmature;
                        lowestDistance = dist;
                    }
                }
                try
                {
                    //set the aim target
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
            else
            {
                return;
            }
        }
    }
}
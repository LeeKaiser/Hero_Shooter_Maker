using UnityEngine;
using System.Collections.Generic;
using System;
using HeroShooterMaker.AI;

//DetermineAimDestination
//Example of an overriden DetermineAim for the demo.
//Causes the agent to aim at where it is moving to
namespace HeroShooterMakerDemo
{
    [CreateAssetMenu(fileName = "TargetDestination", menuName = "AIAction/Aim/TargetDestination")]
    public class DetermineAimDestination : DetermineAim
    {
        public override void ExecuteDetermineAim(AIAction action)
        {
            action.AimTarget.position = action.MoveTarget.position;
            return;
        }
    }
}
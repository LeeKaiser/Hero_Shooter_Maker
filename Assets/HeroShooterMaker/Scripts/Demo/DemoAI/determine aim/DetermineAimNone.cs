using UnityEngine;
using System.Collections.Generic;
using System;
using HeroShooterMaker.AI;

//DetermineAimNone
//Example of an overriden DetermineAim for the demo.
//Do not change aim
namespace HeroShooterMakerDemo
{
    [CreateAssetMenu(fileName = "TargetNone", menuName = "AIAction/Aim/TargetNone")]
    public class DetermineAimNone : DetermineAim
    {
        //do not change aim
        public override void ExecuteDetermineAim(AIAction action)
        {
            return;
        }
    }
}
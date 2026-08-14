using UnityEngine;
using System.Collections.Generic;
using HeroShooterMaker.AI;

//DetermineMovementNone
//Example of an overriden DetermineMovement for the demo.
//Do not choose to move to a new location
namespace HeroShooterMakerDemo
{
    [CreateAssetMenu(fileName = "MoveNone", menuName = "AIAction/Movement/MoveNone")]
    public class DetermineMovementNone : DetermineMovement
    {
        public override void ExecuteDetermineMovement(AIAction action)
        {
            return;
        }
    }
}
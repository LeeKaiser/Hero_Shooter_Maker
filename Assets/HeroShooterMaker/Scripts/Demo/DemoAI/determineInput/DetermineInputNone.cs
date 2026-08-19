using UnityEngine;
using System.Collections.Generic;
using HeroShooterMaker.Controls;
using System;
using HeroShooterMaker.AI;

//DetermineInputNone
//Example of an overriden DetermineInput for the demo.
//Do not choose to use any ability input
namespace HeroShooterMakerDemo
{
    [CreateAssetMenu(fileName = "InputNone", menuName = "AIAction/Input/InputNone")]
    public class DetermineInputNone : DetermineInput
    {
        //no additional input usage
        public override void ExecuteDetermineInput(AIAction action)
        {
            return;
        }
    }
}
using UnityEngine;
using System.Collections.Generic;
using InputOptions;
using System;
using HeroShooterMaker.Abilities;

//DetermineInput
//logic that affects the ability input the agent makes
namespace HeroShooterMaker.AI
{
    public abstract class DetermineInput : ScriptableObject
    {
        public AbilityClass PerferedAbilityClass;
        
        //set the input the character should use in action (action.abilityToUse and action.abilityInput variables)
        public abstract void ExecuteDetermineInput(AIAction action);
    }
}
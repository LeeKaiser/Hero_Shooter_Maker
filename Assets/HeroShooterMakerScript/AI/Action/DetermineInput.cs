using UnityEngine;
using System.Collections.Generic;
using InputOptions;
using System;
using HeroShooterMaker.Abilities;
public abstract class DetermineInput : ScriptableObject
{
    //return the input the character should use
    public AbilityClass PerferedAbilityClass;
    public abstract void ExecuteDetermineInput(AIAction action);
}

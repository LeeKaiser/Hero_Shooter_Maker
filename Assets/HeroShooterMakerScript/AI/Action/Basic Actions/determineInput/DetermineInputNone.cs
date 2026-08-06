using UnityEngine;
using AbilityClassification;
using System.Collections.Generic;
using InputOptions;
using System;

[CreateAssetMenu(fileName = "InputNone", menuName = "AIAction/Input/InputNone")]
public class DetermineInputNone : DetermineInput
{
    //no additional input usage
    public override void ExecuteDetermineInput(AIAction action)
    {
        return;
    }
}

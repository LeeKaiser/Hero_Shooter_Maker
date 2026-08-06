using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "TargetNone", menuName = "AIAction/Aim/TargetNone")]
public class DetermineAimNone : DetermineAim
{
    //do not change aim
    public override void ExecuteDetermineAim(AIAction action)
    {
        return;
    }
}

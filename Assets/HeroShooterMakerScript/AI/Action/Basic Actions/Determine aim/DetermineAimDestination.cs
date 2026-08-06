using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "TargetDestination", menuName = "AIAction/Aim/TargetDestination")]
public class DetermineAimDestination : DetermineAim
{
    //aim at where it is moving to
    public override void ExecuteDetermineAim(AIAction action)
    {
        action.AimTarget.position = action.MoveTarget.position;
        return;
    }
}

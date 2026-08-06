using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MoveNone", menuName = "AIAction/Movement/MoveNone")]
public class DetermineMovementNone : DetermineMovement
{
    public override void ExecuteDetermineMovement(AIAction action)
    {
        return;
    }
}

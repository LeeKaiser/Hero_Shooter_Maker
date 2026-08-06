using UnityEngine;

public abstract class DetermineMovement : ScriptableObject
{
    //set position of movement target
    public abstract void ExecuteDetermineMovement(AIAction action);
}

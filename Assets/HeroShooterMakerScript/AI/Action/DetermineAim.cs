using UnityEngine;


public abstract class DetermineAim : ScriptableObject
{
    //set target and set position of aim target
    public abstract void ExecuteDetermineAim(AIAction action);
}

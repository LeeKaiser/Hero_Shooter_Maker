using UnityEngine;

//DetermineMovement
//logic that affects the agent's movement destination
namespace HeroShooterMaker.AI
{
    public abstract class DetermineMovement : ScriptableObject
    {
        //set position of movement target
        public abstract void ExecuteDetermineMovement(AIAction action);
    }
}
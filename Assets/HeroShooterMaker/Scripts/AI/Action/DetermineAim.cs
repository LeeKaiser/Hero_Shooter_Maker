using UnityEngine;

//DetermineAim
//logic that affects the way the agent aims
namespace HeroShooterMaker.AI
{
    public abstract class DetermineAim : ScriptableObject
    {
        //ExecuteDetermineAim
        //set target (the character it wants to target) and set position of aim target in action
        public abstract void ExecuteDetermineAim(AIAction action);
    }
}
using UnityEngine;

/*
AI Action
abstract parent class for AI's logic for acting in the world
determines where the player looks at, moves to, and ability use
*/
public abstract class AIAction : ScriptableObject
{
    //Variable - Public
    //transform representing position character moves to
    public Transform movementDestination;
    //transform representing position character aims at
    public Transform aimTarget;
    //reference to agent's object detection
    public ObjectDetection objectDetection;
    //reference to agent's ability input event caller
    public InputEventCaller inputCall;

    //Methods
    //initializes variables
    public void Init(Transform movement, Transform aim, ObjectDetection od, InputEventCaller input)
    {
        movementDestination = movement;
        aimTarget = aim;
        objectDetection = od;
        inputCall = input;
    }

    //sets movement destination
    public abstract void DetermineMovement();
    //sets aim location
    public abstract void DetermineAim();
    //makes ability inputs
    public abstract void MakeInput();

}

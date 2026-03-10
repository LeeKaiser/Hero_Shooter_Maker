using UnityEngine;

public abstract class AIAction : ScriptableObject
{
    public Transform movementDestination;
    public Transform aimTarget;
    public ObjectDetection objectDetection;
    public InputEventCaller inputCall;

    public void Init(Transform movement, Transform aim, ObjectDetection od, InputEventCaller input)
    {
        movementDestination = movement;
        aimTarget = aim;
        objectDetection = od;
        inputCall = input;
    }

    public abstract void DetermineMovement();
    public abstract void DetermineAim();
    public abstract void MakeInput();

}

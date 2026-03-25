using UnityEngine;

/* 
AIAction
Abstract Parent class which determines how AI behaves. 
*/
public abstract class AIAction : ScriptableObject
{
    public Transform MoveTarget;
    public Transform AimTarget;
    public ObjectDetection Detection;
    public InputEventCaller InputCall;

    public void Init(Transform movement, Transform aim, ObjectDetection detection, InputEventCaller input)
    {
        MoveTarget = movement;
        AimTarget = aim;
        Detection = detection;
        InputCall = input;
    }

    public abstract void DetermineMovement();
    public abstract void DetermineAim();
    public abstract void MakeInput();

}

using UnityEngine;

public abstract class AIAction : MonoBehaviour
{
    public Transform movementDestination;
    public Transform aimTarget;
    public ObjectDetection objectDetection;

    public void Init(Transform movement, Transform aim, ObjectDetection od)
    {
        movementDestination = movement;
        aimTarget = aim;
        objectDetection = od;
    }

    public abstract void DetermineMovement();
    public abstract void DetermineAim();
    public abstract void MakeInput();

}

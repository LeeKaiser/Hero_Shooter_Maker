using UnityEngine;

public class PatrolAction : AIAction
{
    /*
    public Transform movementDestination;
    public Transform aimTarget;
    public ObjectDetection objectDetection;
    */
    public override void DetermineMovement()
    {
        if (!(objectDetection.GetCurrentContext().focusPOI == null))
        {
            Vector3 nextDestination = objectDetection.GetCurrentContext().focusPOI.transform.position;
            nextDestination.x = nextDestination.x + Random.Range(-10,10);
            nextDestination.z = nextDestination.z + Random.Range(-10,10);
            //Debug.Log(nextDestination);
            movementDestination.position = nextDestination;
        }
    }
    public override void DetermineAim()
    {
        //aimTarget.position = movementDestination.position;
        
    }
    public override void MakeInput()
    {
        
    }
}

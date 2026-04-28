using UnityEngine;
[CreateAssetMenu(menuName = "AIAction/Patrol")]
public class PatrolAction : AIAction
{
    /*
    public Transform MoveTarget;
    public Transform AimTarget;
    public ObjectDetection Detection;
    public InputEventCaller InputCall;
    */
    public override void DetermineMovement()
    {
        if (!(Detection.GetCurrentContext().FocusPOI == null))
        {
            Vector3 nextDestination = Detection.GetCurrentContext().FocusPOI.transform.position;
            nextDestination.x = nextDestination.x + Random.Range(-5,5);
            nextDestination.z = nextDestination.z + Random.Range(-5,5);
            //Debug.Log(nextDestination);
            MoveTarget.position = nextDestination;
            Movement.MoveToLocation();
        }
    }
    public override void DetermineAim()
    {
        AimTarget.position = MoveTarget.position;
        
    }
    public override void DetermineInput()
    {
        
    }
}

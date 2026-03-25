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
        if (!(Detection.GetCurrentContext().focusPOI == null))
        {
            Vector3 nextDestination = Detection.GetCurrentContext().focusPOI.transform.position;
            nextDestination.x = nextDestination.x + Random.Range(-10,10);
            nextDestination.z = nextDestination.z + Random.Range(-10,10);
            //Debug.Log(nextDestination);
            MoveTarget.position = nextDestination;
        }
    }
    public override void DetermineAim()
    {
        AimTarget.position = MoveTarget.position;
        
    }
    public override void MakeInput()
    {
        
    }
}

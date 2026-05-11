using UnityEngine;
using System.Collections.Generic;
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
        //determine place to go

        //when there is no relevant place to go, move to random position near itself
        GameObject playerArmatureRef = Detection.GetCurrentContext().PlayerReference.GetComponent<CharCore>().PlayerArmature;
        Vector3 nextDestination = playerArmatureRef.transform.position;
        
        //when there is an ally around, move somewhere around that ally
        if (Detection.GetCurrentContext().KnownAllyList.Count >= 1)
        {
            float highestVuln = 0;
            foreach (KeyValuePair<CharCore, PlayerSummary> potentialTarget in Detection.GetCurrentContext().KnownAllyList)
            {
                //if (potentialTarget.Key.PlayerArmature == playerArmatureRef){ continue;}
                if (potentialTarget.Value.VulnerabilityValue >= highestVuln)
                {
                    nextDestination = potentialTarget.Key.PlayerArmature.transform.position;
                    Debug.Log(nextDestination);
                    highestVuln = potentialTarget.Value.VulnerabilityValue;
                }
            }
        }

        //if there is a point of interest, move somewhere around the point of interest
        if (!(Detection.GetCurrentContext().FocusPOI == null))
        {
            nextDestination = Detection.GetCurrentContext().FocusPOI.transform.position;
        }

        //choose a random position and move to it
        nextDestination.x = nextDestination.x + Random.Range(-5,5);
        nextDestination.z = nextDestination.z + Random.Range(-5,5);
        //Debug.Log(nextDestination);
        MoveTarget.position = nextDestination;
        Movement.MoveToLocation();
    }
    public override void DetermineAim()
    {
        AimTarget.position = MoveTarget.position;
        
    }
    public override void DetermineInput()
    {
        
    }
}

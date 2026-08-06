using UnityEngine;

[CreateAssetMenu(fileName = "Patrol", menuName = "AIAction/Movement/MovePatrol")]
public class DetermineMovementPatrol : DetermineMovement
{
    public float destinationSpread = 1;
    public override void ExecuteDetermineMovement(AIAction action)
    {
        GameObject playerArmatureRef = action.Detection.GetCurrentContext().PlayerReference.GetComponent<CharCore>().PlayerArmature;
        Vector3 nextDestination = playerArmatureRef.transform.position;
        int priorityIndex = action.Detection.GetCurrentContext().PlayerReference.GetComponent<CharCore>().Stats.PatrolPriorityIndex;
        //if there is a point of interest, move somewhere around the point of interest
        if (!(action.Detection.GetCurrentContext().focusPOIList == null))
        {
            int highestPriority = 0;
            foreach (PatrolLandmark x in action.Detection.GetCurrentContext().focusPOIList)
            {
                //check if index is present or missing. default to 0 if missing
                int usedIndex = priorityIndex;
                if (x.PatrolPriority.Count <= usedIndex) usedIndex = 0;
                if (x.PatrolPriority[usedIndex] > highestPriority)
                {
                    highestPriority = x.PatrolPriority[0];
                    nextDestination = x.transform.position;
                }
            }
            
        }

        //choose a random position and move to it
        nextDestination.x = nextDestination.x + Random.Range(-destinationSpread,destinationSpread);
        nextDestination.z = nextDestination.z + Random.Range(-destinationSpread,destinationSpread);
        //Debug.Log(nextDestination);
        action.MoveTarget.position = nextDestination;
        action.Movement.MoveToLocation();
    }
}

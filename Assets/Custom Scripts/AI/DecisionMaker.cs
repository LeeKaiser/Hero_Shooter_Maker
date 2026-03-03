using UnityEngine;
using Unity.Behavior;
using System.Collections.Generic;
using System.Collections;

public class DecisionMaker : MonoBehaviour
{
    [SerializeField] private DecisionTree decisionTree;
    //[SerializeField] private BehaviorGraphAgent behaviorAgent;
    [SerializeField] private ObjectDetection objectDetection;
    //[SerializeField] private BehaviorGraph currentBehavior;
    [SerializeField] private AIAction currentAction;

    public Transform aimTarget;
    public Transform movementDestination;

    [Tooltip("time between each scan")]
    public float scanTimeInterval = 0.25f;

    void Start()
    {
        objectDetection = GetComponent<ObjectDetection>();
        //behaviorAgent = GetComponent<BehaviorGraphAgent>();
        StartCoroutine(WaitThenScan());
    }

    IEnumerator WaitThenScan()
    {
        yield return new WaitForSeconds(scanTimeInterval);
        objectDetection.RadiusScanAll();
        objectDetection.ElapseExpirationTime(scanTimeInterval);
        //BehaviorGraph newBehavior = decisionTree.MakeDecision(objectDetection.GetCurrentContext());
        //if (currentBehavior != newBehavior)
        //{
        //    behaviorAgent.Graph = newBehavior;
        //    currentBehavior = newBehavior;
        //}
        currentAction = decisionTree.MakeDecision(objectDetection.GetCurrentContext());
        currentAction.Init(movementDestination,aimTarget,objectDetection);
        currentAction.DetermineMovement();
        currentAction.DetermineAim();
        currentAction.MakeInput();
        StartCoroutine(WaitThenScan());
    }

}

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DecisionMaker : MonoBehaviour
{
    [SerializeField] private DecisionTree decisionTree;
    //[SerializeField] private BehaviorGraphAgent behaviorAgent;
    [SerializeField] private ObjectDetection objectDetection;
    //[SerializeField] private BehaviorGraph currentBehavior;
    [SerializeField] private AIAction currentAction;
    [SerializeField] private InputEventCaller inputCall;

    private DecisionTree decisionTreeRuntime;
    private AIAction actionRunTime;

    public Transform aimTarget;
    public Transform movementDestination;

    [Tooltip("time between each scan")]
    public float scanTimeInterval = 0.25f;

    void Start()
    {
        //step 0: start
        objectDetection = GetComponent<ObjectDetection>();
        StartCoroutine(WaitThenScan());
        decisionTreeRuntime = Instantiate(decisionTree);
    }

    IEnumerator WaitThenScan()
    {
        yield return new WaitForSeconds(scanTimeInterval);
        //step 1: run object detection
        objectDetection.RadiusScanAll();
        objectDetection.ElapseExpirationTime(scanTimeInterval);

        //step 2: make decision
        
        AIAction chosenAction = decisionTreeRuntime.MakeDecision(objectDetection.GetCurrentContext());
        if (currentAction != chosenAction)
        {
            currentAction = chosenAction;
            actionRunTime = Instantiate(currentAction);
        }
        actionRunTime.Init(movementDestination,aimTarget,objectDetection, inputCall);

        //step 3: act on decision
        actionRunTime.DetermineMovement();
        actionRunTime.DetermineAim();
        actionRunTime.MakeInput();
        StartCoroutine(WaitThenScan());
    }

}

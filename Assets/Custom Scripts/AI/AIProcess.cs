using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/*
AIProcess
Continuously activates Object Detection, Decision Making, and AI Action
*/
public class AIProcess : MonoBehaviour
{
    [SerializeField] private DecisionTree decisionTree;
    //[SerializeField] private BehaviorGraphAgent behaviorAgent;
    [SerializeField] private ObjectDetection objectDetection;
    //[SerializeField] private BehaviorGraph currentBehavior;
    [SerializeField] private AIAction currentAction;
    [SerializeField] private InputEventCaller inputCall;

    private DecisionTree decisionTreeRuntime;
    private AIAction actionRunTime;

    public Transform AimTarget;
    public Transform MoveTarget;

    [Tooltip("time between each scan")]
    public float ScanTimeInterval = 0.25f;

    void Start()
    {
        //step 0: start
        objectDetection = GetComponent<ObjectDetection>();
        
        decisionTreeRuntime = Instantiate(decisionTree);
        
    }

    void OnEnable()
    {
        StartCoroutine(WaitThenScan());
    }

    IEnumerator WaitThenScan()
    {
        yield return new WaitForSeconds(ScanTimeInterval);
        //step 1: run object detection
        objectDetection.RadiusScanAll();
        objectDetection.ElapseExpirationTime(ScanTimeInterval);

        //step 2: make decision
        AIAction chosenAction = decisionTreeRuntime.MakeDecision(objectDetection.GetCurrentContext());
        if (currentAction != chosenAction)
        {
            currentAction = chosenAction;
            actionRunTime = Instantiate(currentAction);
        }
        actionRunTime.Init(MoveTarget,AimTarget,objectDetection, inputCall);

        //step 3: act on decision
        actionRunTime.DetermineMovement();
        actionRunTime.DetermineAim();
        actionRunTime.MakeInput();

        //repeat
        StartCoroutine(WaitThenScan());
    }

}

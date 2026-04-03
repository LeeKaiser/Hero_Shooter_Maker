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
        StartCoroutine(RunObjectDetection());

        //step 2: make decision
        StartCoroutine(MakeDecision());

        //step 3: act on decision
        StartCoroutine(ActDecision());

        //repeat
        StartCoroutine(WaitThenScan());
    }

    IEnumerator RunObjectDetection()
    {
        objectDetection.RadiusScanAll();
        objectDetection.ElapseExpirationTime(ScanTimeInterval);
        yield return new WaitForSeconds(Random.Range(0.01f , 0.05f));
    }

    IEnumerator MakeDecision()
    {

        AIAction chosenAction = decisionTreeRuntime.MakeDecision(objectDetection.GetCurrentContext());
        if (currentAction != chosenAction)
        {
            currentAction = chosenAction;
            actionRunTime = Instantiate(currentAction);
        }
        actionRunTime.Init(MoveTarget,AimTarget,objectDetection, inputCall);
        yield return new WaitForSeconds(Random.Range(0.01f , 0.05f));
    }

    IEnumerator ActDecision()
    {

        actionRunTime.DetermineMovement();
        actionRunTime.DetermineAim();
        actionRunTime.MakeInput();
        yield return new WaitForSeconds(Random.Range(0.01f , 0.05f));
    }
}

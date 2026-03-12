using UnityEngine;
using Unity.Behavior;
using System.Collections.Generic;
using System.Collections;

/*
DecisionMaker
activates detection, decision making, and calls AI Action
*/
public class DecisionMaker : MonoBehaviour
{
    //variable - public
    [Tooltip("reference to decision tree")]
    [SerializeField] private DecisionTree decisionTree;

    [Tooltip("reference to object detection")]
    [SerializeField] private ObjectDetection objectDetection;

    [Tooltip("current AI Action in use")]
    [SerializeField] private AIAction currentAction;

    [Tooltip("reference to ability input event caller")]
    [SerializeField] private InputEventCaller inputCall;

    [Tooltip("transform representing where character aims at")]
    public Transform aimTarget;

    [Tooltip("transform representing where character moves to")]
    public Transform movementDestination;

    [Tooltip("time between each scan")]
    public float scanTimeInterval = 0.25f;


    //variable - private
    //copy of decision tree at run time
    private DecisionTree decisionTreeRuntime;
    //copy of AI Action at run time
    private AIAction actionRunTime;

    //method
    //called when agent is created
    void Start()
    {
        //step 0: start
        objectDetection = GetComponent<ObjectDetection>();
        StartCoroutine(WaitThenScan());
        decisionTreeRuntime = Instantiate(decisionTree);
    }

    //goes through detection - decision - action steps every scan interval
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

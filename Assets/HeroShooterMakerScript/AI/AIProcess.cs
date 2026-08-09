using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/*
AIProcess
Continuously activates Object Detection, Decision Making, and AI Action
*/

[RequireComponent(typeof(AIMovement))]
[RequireComponent(typeof(ObjectDetection))]
public class AIProcess : MonoBehaviour
{
    [SerializeField] private DecisionTree decisionTree;
    //[SerializeField] private BehaviorGraphAgent behaviorAgent;
    [SerializeField] private ObjectDetection objectDetection;
    //[SerializeField] private BehaviorGraph currentBehavior;
    [SerializeField] private AIAction currentAction;
    [SerializeField] private InputConverter inputConvert;
    [SerializeField] private AIMovement movement;

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
        movement = GetComponent<AIMovement>();
        inputConvert = GetComponent<InputConverter>();
        decisionTreeRuntime = Instantiate(decisionTree);
        
    }

    void Update()
    {
        if (actionRunTime != null)
        {
            actionRunTime.MakeInput();
        }
    }

    void OnEnable()
    {
        StartCoroutine(WaitThenScan());
    }

    IEnumerator WaitThenScan()
    {
        while (true)
        {
            //step 1: run object detection
            yield return new WaitForSeconds(Random.Range(0.0f , 0.1f));
            objectDetection.RadiusScanAll();
            
            yield return new WaitForSeconds(ScanTimeInterval);
            objectDetection.ElapseExpirationTime(ScanTimeInterval);

            //step 2: make decision
            AIAction chosenAction = decisionTreeRuntime.MakeDecision(objectDetection.GetCurrentContext());
            if (currentAction != chosenAction)
            {
                //finish using an action it was using
                if (actionRunTime != null)
                {
                    if (actionRunTime.HoldingInput && actionRunTime.abilityToUse != null)
                    {
                        actionRunTime.ReleaseInput();
                    }
                }
                
                currentAction = chosenAction;
                actionRunTime = Instantiate(currentAction);
            }
            actionRunTime.Init(MoveTarget,AimTarget,objectDetection, inputConvert, movement);

            //step 3: act on decision
            actionRunTime.CommitToAction();
        }
    }

}

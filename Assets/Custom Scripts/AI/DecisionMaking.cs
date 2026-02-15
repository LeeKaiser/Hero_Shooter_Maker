using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DecisionMaking : MonoBehaviour
{

    [SerializeField] private List<Decision> decisionList;
    [SerializeField] private ObjectDetection objectDetection;

    [SerializeField] private Decision currentDecision;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MakeDecision();
    }

    public void MakeDecision()
    {
        

        float bestDecisionValue = 0f;
        
        foreach(Decision x in decisionList)
        {
            float currentDecisionScore = x.ScoreDecision(objectDetection.getCurrentContext());
            if (currentDecisionScore > bestDecisionValue)
            {
                bestDecisionValue = currentDecisionScore;
                currentDecision = x;
            }
        }
        //set the agent's behavior to the behavior in the current decision

        
    }


}

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.Behavior;

[CreateAssetMenu(menuName = "DecisionTree")]
public class DecisionTree : ScriptableObject
{
    
    public DecisionTreeNode decisionTreeRoot = null;
    
    public List<DecisionTreeNode> allNodes = new List<DecisionTreeNode>();

    public AIAction MakeDecision(KnownContext context)
    {
        AIAction determinedAction = decisionTreeRoot.GetAction(context);
        return determinedAction;
    }

    


}

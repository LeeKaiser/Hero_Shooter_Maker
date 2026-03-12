using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.Behavior;

/*
Decision tree
used to make decisions based on if a condition is true/false in the current situation
*/
[CreateAssetMenu(menuName = "DecisionTree")]
public class DecisionTree : ScriptableObject
{
    //Variable - Public
    //root node of decision tree. starts decision making from this node
    public DecisionTreeNode decisionTreeRoot = null;
    //all nodes in the tree
    public List<DecisionTreeNode> allNodes = new List<DecisionTreeNode>();

    //Method
    //searches for an AIAction to use
    public AIAction MakeDecision(KnownContext context)
    {
        AIAction determinedAction = decisionTreeRoot.GetAction(context);
        return determinedAction;
    }

    


}

using UnityEngine;
using Unity.Behavior;
using System.Collections.Generic;
using System.Collections;
using DecisionCondition;

public class DecisionTreeNode : ScriptableObject
{
    public DecisionTreeNode childYesNode;
    public DecisionTreeNode childNoNode;
    public List<DecisionTreeNode> parentNodes = new List<DecisionTreeNode>();
    public decisionCondition nodeCondition;
    //public BehaviorGraph behavior;
    public AIAction action;
    public bool isBehavior = false; //if true, return the associated behavior, if false, return the child.

    public float parameterFloat = 0;
    public Vector2 editorPosition; // for visual placement

    //get the behavior that the AI should use.
    public AIAction GetAction(KnownContext currentContext)
    {
        if (isBehavior)
        {
            return action;
        }
        else
        {
            
            if (ConditionCheck.CheckIfConditionTrue(nodeCondition, parameterFloat, currentContext))
            {
                return childYesNode != null
                ? childYesNode.GetAction(currentContext)
                : null;
                
            }
            else
            {
                return childNoNode != null
                ? childNoNode.GetAction(currentContext)
                : null;
            }
        }
    }

    public void removeFromParent()
    {
        foreach(var x in parentNodes)
        {
            if (x.childYesNode == this)
            {
                x.childYesNode = null;
            }
            else if (x.childNoNode == this)
            {
                x.childNoNode = null;
            }
        }
    }

    
}

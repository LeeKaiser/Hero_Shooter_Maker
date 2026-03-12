using UnityEngine;
using Unity.Behavior;
using System.Collections.Generic;
using System.Collections;
using DecisionCondition;

/*
Decision tree node
node for decision tree
used to make decisions based on if a condition is true/false in the current situation
*/
public class DecisionTreeNode : ScriptableObject
{
    //Variables - Public
    //reference to child node when the condition is true
    public DecisionTreeNode childYesNode;

    //reference to child node when the condition is false
    public DecisionTreeNode childNoNode;

    //reference to all parents
    public List<DecisionTreeNode> parentNodes = new List<DecisionTreeNode>();

    //the condition that is checked on. only relevant if isBehavior is false
    public decisionCondition nodeCondition;

    //associated action. only relevant if isBehavior is true
    public AIAction action;

    //if true, return the associated behavior, if false, return the child.
    public bool isBehavior = false; 

    //parameter to check the condition on. only relevant if isBehavior is false
    public float parameterFloat = 0;

    // for visual placement
    public Vector2 editorPosition; 


    //Methods
    //get the AI action that the AI should use.
    public AIAction GetAction(KnownContext currentContext)
    {
        //return its associated behavior 
        if (isBehavior)
        {
            return action;
        }
        //otherwise, return the child's associated behavior
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

    //removes from parent nodes. used before deleting itself
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

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DecisionCondition;

public class DecisionTreeNode : ScriptableObject
{
    public DecisionTreeNode ChildYesNode;
    public DecisionTreeNode ChildNoNode;
    public List<DecisionTreeNode> ParentNodes = new List<DecisionTreeNode>();
    public decisionCondition NodeCondition;
    public AIAction Action;
    public bool IsAction = false; //if true, return the associated behavior, if false, return the child.

    public float ParameterFloat = 0;
    public Vector2 EditorPosition; // for visual placement

    //get the behavior that the AI should use.
    public AIAction GetAction(KnownContext currentContext)
    {
        if (IsAction)
        {
            return Action;
        }
        else
        {
            
            if (ConditionCheck.CheckIfConditionTrue(NodeCondition, ParameterFloat, currentContext))
            {
                return ChildYesNode != null
                ? ChildYesNode.GetAction(currentContext)
                : null;
                
            }
            else
            {
                return ChildNoNode != null
                ? ChildNoNode.GetAction(currentContext)
                : null;
            }
        }
    }

    public void removeFromParent()
    {
        foreach(var x in ParentNodes)
        {
            if (x.ChildYesNode == this)
            {
                x.ChildYesNode = null;
            }
            else if (x.ChildNoNode == this)
            {
                x.ChildNoNode = null;
            }
        }
    }

    
}

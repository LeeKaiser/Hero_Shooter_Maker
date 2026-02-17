using UnityEngine;
using Unity.Behavior;
using DecisionCondition;

public class DecisionTreeNode : ScriptableObject
{
    public DecisionTreeNode childYesNode;
    public DecisionTreeNode childNoNode;
    public decisionCondition nodeCondition;
    public BehaviorGraph behavior;
    public bool isBehavior = false; //if true, return the associated behavior, if false, return the child.

    
    public Vector2 editorPosition; // for visual placement

    //get the behavior that the AI should use.
    public BehaviorGraph GetBehavior(KnownContext currentContext)
    {
        if (isBehavior)
        {
            return behavior;
        }
        else
        {
            
            if (ConditionCheck.CheckIfConditionTrue(nodeCondition, currentContext))
            {
                return childYesNode != null
                ? childYesNode.GetBehavior(currentContext)
                : null;
                
            }
            else
            {
                return childNoNode != null
                ? childNoNode.GetBehavior(currentContext)
                : null;
            }
        }
    }
}

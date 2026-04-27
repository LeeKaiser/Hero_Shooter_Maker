using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/*
DecisionTree
Tree which represents the decision making process that AI goes through in order to pick a AI Action to use.

Edit Decision Tree by making a new decision tree. Then open the tree editor through Window->DecisionTreeEditor.  
Add new root node to begin

Nodes can be in either condition mode when Action is checked off, or be in action mode when Action is checked on. 
If in condition mode, it must have child nodes for the case where the condition is true or false. If on action mode, 
it must have an assigned AI Action. If there is a point in the tree that does not end with an AI Action, MakeDecision 
method will return null
*/
[CreateAssetMenu(menuName = "DecisionTree")]
public class DecisionTree : ScriptableObject
{
    
    public DecisionTreeNode DecisionTreeRoot = null;
    
    public List<DecisionTreeNode> AllNodes = new List<DecisionTreeNode>();

    public AIAction MakeDecision(KnownContext context)
    {
        AIAction determinedAction = DecisionTreeRoot.GetAction(context);
        return determinedAction;
    }

    


}

using UnityEngine;
using UnityEditor;
using DecisionCondition;

public class DecisionTreeEditorWindow : EditorWindow
{
    //editor code made with assistance from Chat GPT
    public DecisionTree currentDecisionTree;
    private DecisionTreeNode currentDecisionTreeNode;
    private Vector2 panOffset;
    private float zoom = 1f;
    private const float zoomMin = 0.5f;
    private const float zoomMax = 2f;
    private DecisionTreeNode draggingNode;
    //private Rect canvasRect = new Rect(-5000, -5000, 10000, 10000);
    private DecisionTreeNode nodeToDelete;

    private bool choosingRootMode = false;
    private bool choosingYesMode = false;
    private bool choosingNoMode = false;
    private DecisionTreeNode choosingParentNode;

    private DecisionTreeNode newNode;
    private bool isAddYes = false;

    [MenuItem("Window/Decision Tree Editor")]
    public static void OpenWindow()
    {
        GetWindow<DecisionTreeEditorWindow>("Decision Tree Editor");
    }

    private void OnGUI()
    {
        
        HandleEvents();
        currentDecisionTree = (DecisionTree)EditorGUILayout.ObjectField(
            "Decision Tree",
            currentDecisionTree,
            typeof(DecisionTree),
            false
        );
        if (currentDecisionTree == null)
        {
            EditorGUILayout.HelpBox("Create or assign a DecisionTree asset to start editing.", MessageType.Info);
            //GUI.EndGroup();
            return;
        }
        if (currentDecisionTree.DecisionTreeRoot == null)
        {
            EditorGUILayout.HelpBox("Tree has no root node.", MessageType.Info);

            if (GUILayout.Button("Create Root"))
            {
                DecisionTreeNode node = CreateNode();
                currentDecisionTree.AllNodes.Add(node);
                currentDecisionTree.DecisionTreeRoot = node;
            }

            
            
        }
        if (GUILayout.Button("Set Existing Node As Root"))
        {
            if (choosingRootMode)
            {
                choosingRootMode = false;
            }
            else
            {
                choosingRootMode = true;
            }
            
        }

        //tree view
        Matrix4x4 oldMatrix = GUI.matrix;

        GUI.matrix = Matrix4x4.TRS(
            panOffset,
            Quaternion.identity,
            Vector3.one * zoom
        );

        Rect viewRect = new Rect(0, 0, position.width, position.height);
        GUI.BeginGroup(viewRect);

        Handles.BeginGUI();
        foreach (var node in currentDecisionTree.AllNodes)
        {
            DrawConnections(node);
        }
            
        Handles.EndGUI();   
        foreach (var node in currentDecisionTree.AllNodes)
        {
            DrawNode(node);
        }
        
        if (!(nodeToDelete == null))
        {
            DeleteNode(nodeToDelete);
            nodeToDelete = null;
        }

        if (!(newNode == null))
        {
            currentDecisionTree.AllNodes.Add(newNode);
            newNode.ParentNodes.Add(currentDecisionTreeNode);
            
            if (isAddYes)
            {
                
                newNode.EditorPosition = currentDecisionTreeNode.EditorPosition + new Vector2(-200, 150);
                currentDecisionTreeNode.ChildYesNode = newNode;
                isAddYes = false;
            }
            else
            {
                newNode.EditorPosition = currentDecisionTreeNode.EditorPosition + new Vector2(200, 150);
                currentDecisionTreeNode.ChildNoNode = newNode;
            }
            
            newNode = null;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(currentDecisionTree);
            //AssetDatabase.SaveAssets();
        }

        GUI.matrix = oldMatrix;
        GUI.EndGroup();
    }

    void DrawNode(DecisionTreeNode node)
    {
        if (node == null) return;
        Rect rect = new Rect(node.EditorPosition.x, node.EditorPosition.y, 250, 170);
        EditorGUIUtility.AddCursorRect(rect, MouseCursor.MoveArrow);

        GUILayout.BeginArea(rect, GUI.skin.window);
        if (node == currentDecisionTree.DecisionTreeRoot)
        {
            EditorGUILayout.LabelField("Root Node", EditorStyles.boldLabel);
        }
        if (node == choosingParentNode)
        {
            EditorGUILayout.LabelField("Choosing Child", EditorStyles.boldLabel);
        }

        if (choosingRootMode)
        {
            if (GUILayout.Button("Set As Root"))
            {
                currentDecisionTree.DecisionTreeRoot = node;
                choosingRootMode = false;
            }
        }

        if ((choosingYesMode || choosingNoMode) && choosingParentNode != node)
        {
            if (GUILayout.Button("Set As child"))
            {
                
                node.ParentNodes.Add(choosingParentNode);
                if (choosingYesMode)
                {
                    choosingParentNode.ChildYesNode = node;
                }
                else if (choosingNoMode)
                {
                    choosingParentNode.ChildNoNode = node;
                }

                choosingNoMode = false;
                choosingYesMode = false;
                choosingParentNode = null;
            }
        }

        if (GUILayout.Button("Delete Node"))
        {
            nodeToDelete = node;
            GUILayout.EndArea();
            return;
        }
        Undo.RecordObject(currentDecisionTree, "Modify Node");
        //toggle for condition or end of node final decision
        node.IsAction =  EditorGUILayout.Toggle("Action", node.IsAction);

        if (!node.IsAction)
        {
            node.NodeCondition = (decisionCondition)EditorGUILayout.EnumPopup("Condition: ", node.NodeCondition);
            node.ParameterFloat = EditorGUILayout.FloatField("Parameter: ", node.ParameterFloat);

            GUILayout.BeginHorizontal();

            if (node.ChildYesNode == null)
            {
                if (GUILayout.Button("Add Yes Node"))
                {
                    newNode = CreateNode();
                    
                    isAddYes = true;
                }
            }
                

            if (node.ChildNoNode == null)
            {
                if (GUILayout.Button("Add No Node"))
                {
                    newNode = CreateNode();
                    
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            if (node.ChildYesNode == null)
            {
                if (GUILayout.Button("Set Yes Node"))
                {
                    if (choosingYesMode)
                    {
                        choosingYesMode = false;
                        choosingNoMode = false;
                        choosingParentNode = null;
                    }
                    else
                    {
                        choosingNoMode = false;
                        choosingYesMode = true;
                        choosingParentNode = node;
                    }
                    
                }
            }
                

            if (node.ChildNoNode == null)
            {
                if (GUILayout.Button("Set No Node"))
                {
                    if (choosingNoMode)
                    {
                        choosingYesMode = false;
                        choosingNoMode = false;
                        choosingParentNode = null;
                    }
                    else
                    {
                        choosingYesMode = false;
                        choosingNoMode = true;
                        choosingParentNode = node;
                    }
                }
            }
            GUILayout.EndHorizontal();
        }
        else
        {
            node.Action = (AIAction)EditorGUILayout.ObjectField(
                "AI Action",
                node.Action,
                typeof(AIAction),
                false
            );
        }
        

        GUILayout.EndArea();

        DragNode(node, rect);
        
    }

    void DragNode(DecisionTreeNode node, Rect rect)
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
        {
            draggingNode = node;
            currentDecisionTreeNode = node;
            e.Use();
        }

        if (e.type == EventType.MouseDrag && draggingNode == node)
        {
            node.EditorPosition += e.delta /*/ zoom*/;
            GUI.changed = true;
            e.Use();
        }

        if (e.type == EventType.MouseUp && draggingNode == node)
        {
            draggingNode = null;
            e.Use();
        }
    }
    void DeleteNode(DecisionTreeNode node)
    {
        
        Undo.RecordObject(currentDecisionTree, "Delete Node");
        //remove root's reference if deleted node is root
        if (currentDecisionTree.DecisionTreeRoot == node)
        {
            currentDecisionTree.DecisionTreeRoot = null;
        }
        //remove parent's reference to node
        node.removeFromParent();
        
        //remove child's reference to node
        if (!(node.ChildYesNode == null))
        {
            node.ChildYesNode.ParentNodes.Remove(node);
        }
        if (!(node.ChildNoNode == null))
        {
            node.ChildNoNode.ParentNodes.Remove(node);
        }
        //remove list's reference to the node
        currentDecisionTree.AllNodes.Remove(node);

        DestroyImmediate(node,true);
        

        EditorUtility.SetDirty(currentDecisionTree);
    }

    DecisionTreeNode CreateNode()
    {
        DecisionTreeNode node = ScriptableObject.CreateInstance<DecisionTreeNode>();

        Undo.RecordObject(currentDecisionTree, "Add Node");

        AssetDatabase.AddObjectToAsset(node, currentDecisionTree);
        AssetDatabase.SaveAssets();

        //currentDecisionTree.AllNodes.Add(node);

        //EditorUtility.SetDirty(currentDecisionTree);

        return node;
    }

    void DrawConnections(DecisionTreeNode node)
    {
        if (!node.IsAction)
        {
            Vector2 startPos = node.EditorPosition + new Vector2(125, 170);
            if (node.ChildYesNode != null)
            {
                Vector2 leftPos = node.ChildYesNode.EditorPosition + new Vector2(125, 0);
                Handles.DrawLine( startPos, leftPos );

                Vector2 mid = (startPos + leftPos) / 2;
                Handles.Label(mid, "Yes");

                //DrawConnections(node.ChildYesNode);
            }

            if (node.ChildNoNode != null)
            {
                Vector2 rightPos = node.ChildNoNode.EditorPosition + new Vector2(125, 0);
                Handles.DrawLine( startPos, rightPos );

                Vector2 mid = (startPos + rightPos) / 2;
                Handles.Label(mid, "No");

                //DrawConnections(node.ChildNoNode);
            }
        }
    }

    void HandleEvents()
    {
        Event e = Event.current;

        // PAN (Middle mouse or Alt+Left)
        if (e.type == EventType.MouseDrag &&
            (e.button == 2 || (e.button == 0 && e.alt)))
        {
            panOffset += e.delta;
            GUI.changed = true;
        }

        // ZOOM (Scroll wheel)
        if (e.type == EventType.ScrollWheel)
        {
            float oldZoom = zoom;
            float zoomDelta = -e.delta.y * 0.03f;
            zoom = Mathf.Clamp(zoom + zoomDelta, zoomMin, zoomMax);

            Vector2 mouse = e.mousePosition;

            panOffset = (panOffset - mouse) * (zoom / oldZoom) + mouse;

            e.Use();
        }
    }
}

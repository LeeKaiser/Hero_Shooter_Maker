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

    private bool choosingRootMode = false;
    private bool choosingYesMode = false;
    private bool choosingNoMode = false;
    private DecisionTreeNode choosingParentNode;

    [MenuItem("Window/Decision Tree Editor")]
    public static void OpenWindow()
    {
        GetWindow<DecisionTreeEditorWindow>("Decision Tree Editor");
    }

    private void OnGUI()
    {
        //Debug.Log(currentDecisionTree.decisionTreeRoot == null ? "Root is NULL" : "Root EXISTS");
        
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
        if (currentDecisionTree.decisionTreeRoot == null)
        {
            EditorGUILayout.HelpBox("Tree has no root node.", MessageType.Info);

            if (GUILayout.Button("Create Root"))
            {
                DecisionTreeNode node = CreateNode();
                currentDecisionTree.decisionTreeRoot = node;
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
        foreach (var node in currentDecisionTree.allNodes)
        {
            DrawConnections(node);
        }
            
        Handles.EndGUI();   
        foreach (var node in currentDecisionTree.allNodes)
        {
            DrawNode(node);
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
        Rect rect = new Rect(node.editorPosition.x, node.editorPosition.y, 250, 170);
        EditorGUIUtility.AddCursorRect(rect, MouseCursor.MoveArrow);

        GUILayout.BeginArea(rect, GUI.skin.window);
        if (node == currentDecisionTree.decisionTreeRoot)
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
                currentDecisionTree.decisionTreeRoot = node;
                choosingRootMode = false;
            }
        }

        if ((choosingYesMode || choosingNoMode) && choosingParentNode != node)
        {
            if (GUILayout.Button("Set As child"))
            {
                
                node.parentNodes.Add(choosingParentNode);
                if (choosingYesMode)
                {
                    choosingParentNode.childYesNode = node;
                }
                else if (choosingNoMode)
                {
                    choosingParentNode.childNoNode = node;
                }

                choosingNoMode = false;
                choosingYesMode = false;
                choosingParentNode = null;
            }
        }

        if (GUILayout.Button("Delete Node"))
        {
            DeleteNode(node);
            GUILayout.EndArea();
            return;
        }
        Undo.RecordObject(currentDecisionTree, "Modify Node");
        //toggle for condition or end of node final decision
        node.isBehavior =  EditorGUILayout.Toggle("Behavior", node.isBehavior);

        if (!node.isBehavior)
        {
            node.nodeCondition = (decisionCondition)EditorGUILayout.EnumPopup("Condition: ", node.nodeCondition);
            node.parameterFloat = EditorGUILayout.FloatField("Parameter: ", node.parameterFloat);

            GUILayout.BeginHorizontal();

            if (node.childYesNode == null)
            {
                if (GUILayout.Button("Add Yes Node"))
                {
                    DecisionTreeNode newNode = CreateNode();
                    
                    newNode.parentNodes.Add(node);
                    newNode.editorPosition = node.editorPosition + new Vector2(-200, 150);
                    node.childYesNode = newNode;
                }
            }
                

            if (node.childNoNode == null)
            {
                if (GUILayout.Button("Add No Node"))
                {
                    DecisionTreeNode newNode = CreateNode();
                    
                    newNode.parentNodes.Add(node);
                    newNode.editorPosition = node.editorPosition + new Vector2(200, 150);
                    node.childNoNode = newNode;
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            if (node.childYesNode == null)
            {
                if (GUILayout.Button("Set Yes Node"))
                {
                    if (choosingYesMode)
                    {
                        choosingYesMode = false;
                        choosingNoMode = false;
                    }
                    else
                    {
                        choosingNoMode = false;
                        choosingYesMode = true;
                        choosingParentNode = node;
                    }
                    
                }
            }
                

            if (node.childNoNode == null)
            {
                if (GUILayout.Button("Set No Node"))
                {
                    if (choosingNoMode)
                    {
                        choosingYesMode = false;
                        choosingNoMode = false;
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
            node.action = (AIAction)EditorGUILayout.ObjectField(
                "AI Action",
                node.action,
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
            node.editorPosition += e.delta /*/ zoom*/;
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
        if (currentDecisionTree.decisionTreeRoot == node)
        {
            currentDecisionTree.decisionTreeRoot = null;
        }
        //remove parent's reference to node
        node.removeFromParent();
        
        //remove child's reference to node
        if (!(node.childYesNode == null))
        {
            node.childYesNode.parentNodes.Remove(node);
        }
        if (!(node.childNoNode == null))
        {
            node.childNoNode.parentNodes.Remove(node);
        }
        //remove list's reference to the node
        currentDecisionTree.allNodes.Remove(node);

        DestroyImmediate(node,true);
        

        EditorUtility.SetDirty(currentDecisionTree);
    }

    DecisionTreeNode CreateNode()
    {
        DecisionTreeNode node = ScriptableObject.CreateInstance<DecisionTreeNode>();

        Undo.RecordObject(currentDecisionTree, "Add Node");

        AssetDatabase.AddObjectToAsset(node, currentDecisionTree);
        AssetDatabase.SaveAssets();

        currentDecisionTree.allNodes.Add(node);

        EditorUtility.SetDirty(currentDecisionTree);

        return node;
    }

    void DrawConnections(DecisionTreeNode node)
    {
        if (!node.isBehavior)
        {
            Vector2 startPos = node.editorPosition + new Vector2(125, 170);
            if (node.childYesNode != null)
            {
                Vector2 leftPos = node.childYesNode.editorPosition + new Vector2(125, 0);
                Handles.DrawLine( startPos, leftPos );

                Vector2 mid = (startPos + leftPos) / 2;
                Handles.Label(mid, "Yes");

                //DrawConnections(node.childYesNode);
            }

            if (node.childNoNode != null)
            {
                Vector2 rightPos = node.childNoNode.editorPosition + new Vector2(125, 0);
                Handles.DrawLine( startPos, rightPos );

                Vector2 mid = (startPos + rightPos) / 2;
                Handles.Label(mid, "No");

                //DrawConnections(node.childNoNode);
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

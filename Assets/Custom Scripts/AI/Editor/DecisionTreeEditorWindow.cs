using UnityEngine;
using UnityEditor;
using DecisionCondition;
using Unity.Behavior;

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

    [MenuItem("Window/Decision Tree Editor")]
    public static void OpenWindow()
    {
        GetWindow<DecisionTreeEditorWindow>("Decision Tree Editor");
    }

    private void OnGUI()
    {
        //Debug.Log(currentDecisionTree.decisionTreeRoot == null ? "Root is NULL" : "Root EXISTS");
        
        HandleEvents();
        Rect viewRect = new Rect(0, 0, position.width, position.height);
        GUI.BeginGroup(viewRect);
        currentDecisionTree = (DecisionTree)EditorGUILayout.ObjectField(
            "Decision Tree",
            currentDecisionTree,
            typeof(DecisionTree),
            false
        );
        Matrix4x4 oldMatrix = GUI.matrix;

        GUI.matrix = Matrix4x4.TRS(
            panOffset,
            Quaternion.identity,
            Vector3.one * zoom
        );

        

        

        if (currentDecisionTree == null)
        {
            EditorGUILayout.HelpBox("Create or assign a DecisionTree asset to start editing.", MessageType.Info);
            GUI.EndGroup();
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
            GUI.EndGroup();
            return; // stop drawing further
        }
        else
        {
            Handles.BeginGUI();
            DrawConnections(currentDecisionTree.decisionTreeRoot);
            Handles.EndGUI();   
            DrawNode(currentDecisionTree.decisionTreeRoot);
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
        Rect rect = new Rect(node.editorPosition.x, node.editorPosition.y, 300, 130);
        EditorGUIUtility.AddCursorRect(rect, MouseCursor.MoveArrow);

        GUILayout.BeginArea(rect, GUI.skin.window);

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

            GUILayout.BeginHorizontal();

            if (node.childYesNode == null)
            {
                if (GUILayout.Button("Add Yes Node"))
                {
                    DecisionTreeNode newNode = CreateNode();
                    node.childYesNode = newNode;
                    node.childYesNode.editorPosition = node.editorPosition + new Vector2(-200, 150);
                }
            }
                

            if (node.childNoNode == null)
            {
                if (GUILayout.Button("Add No Node"))
                {
                    DecisionTreeNode newNode = CreateNode();
                    node.childNoNode = newNode;
                    node.childNoNode.editorPosition = node.editorPosition + new Vector2(200, 150);
                }
            }
            GUILayout.EndHorizontal();
        }
        else
        {
            node.behavior = (BehaviorGraph)EditorGUILayout.ObjectField(
                "Behavior Graph",
                node.behavior,
                typeof(BehaviorGraph),
                false
            );
        }
        

        

        GUILayout.EndArea();

        DragNode(node, rect);

        if (!node.isBehavior)
        {
            if (node.childYesNode != null)
                DrawNode(node.childYesNode);

            if (node.childNoNode != null)
                DrawNode(node.childNoNode);
        }
        
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
        if (node == currentDecisionTree.decisionTreeRoot)
        {
            Undo.RecordObject(currentDecisionTree, "Delete Root");
            currentDecisionTree.decisionTreeRoot = null;
            EditorUtility.SetDirty(currentDecisionTree);
            AssetDatabase.SaveAssets();
            return;
        }

        Undo.RecordObject(currentDecisionTree, "Delete Node");
        DeleteNodeRecursive(currentDecisionTree.decisionTreeRoot, node);
        EditorUtility.SetDirty(currentDecisionTree);
    }

    bool DeleteNodeRecursive(DecisionTreeNode current, DecisionTreeNode target)
    {
        if (current == null)
            return false;

        if (current.childYesNode == target)
        {
            current.childYesNode = null;
            return true;
        }

        if (current.childNoNode == target)
        {
            current.childNoNode = null;
            return true;
        }

        return DeleteNodeRecursive(current.childYesNode, target)
            || DeleteNodeRecursive(current.childNoNode, target);
    }

    DecisionTreeNode CreateNode()
    {
        DecisionTreeNode node = ScriptableObject.CreateInstance<DecisionTreeNode>();

        Undo.RecordObject(currentDecisionTree, "Add Node");

        AssetDatabase.AddObjectToAsset(node, currentDecisionTree);
        AssetDatabase.SaveAssets();

        //currentDecisionTree.allNodes.Add(node);

        EditorUtility.SetDirty(currentDecisionTree);

        return node;
    }

    void DrawConnections(DecisionTreeNode node)
    {
        if (!node.isBehavior)
        {
            Vector2 startPos = node.editorPosition + new Vector2(150, 120);
            if (node.childYesNode != null)
            {
                Vector2 leftPos = node.childYesNode.editorPosition + new Vector2(150, 0);
                Handles.DrawLine( startPos, leftPos );

                Vector2 mid = (startPos + leftPos) / 2;
                Handles.Label(mid, "Yes");

                DrawConnections(node.childYesNode);
            }

            if (node.childNoNode != null)
            {
                Vector2 rightPos = node.childNoNode.editorPosition + new Vector2(150, 0);
                Handles.DrawLine( startPos, rightPos );

                Vector2 mid = (startPos + rightPos) / 2;
                Handles.Label(mid, "No");

                DrawConnections(node.childNoNode);
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

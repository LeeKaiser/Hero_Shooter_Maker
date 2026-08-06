using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class DecisionTreeEditorWindow : EditorWindow
{
    private DecisionTreeGraphView _graphView;
    private ObjectField _treeField;
    private DecisionTree _currentTree;

    [MenuItem("Window/AI/Decision Tree Editor")]
    public static void Open()
    {
        var window = GetWindow<DecisionTreeEditorWindow>();
        window.titleContent = new GUIContent("Decision Tree Editor");
        window.minSize = new Vector2(800, 500);
    }

    // Lets you double-click a DecisionTree asset in the Project window to open it here directly.
    [OnOpenAsset]
    public static bool OnOpenAsset(EntityId entityId, int line)
    {
        var obj = EditorUtility.EntityIdToObject(entityId);
        if (obj is DecisionTree tree)
        {
            Open();
            GetWindow<DecisionTreeEditorWindow>().LoadTree(tree);
            return true;
        }
        return false;
    }

    private void CreateGUI()
    {
        var toolbar = new Toolbar();

        _treeField = new ObjectField("Decision Tree")
        {
            objectType = typeof(DecisionTree),
            allowSceneObjects = false
        };
        _treeField.style.width = 350;
        _treeField.RegisterValueChangedCallback(evt => LoadTree(evt.newValue as DecisionTree));
        toolbar.Add(_treeField);

        toolbar.Add(new ToolbarButton(() => _graphView?.CreateRootNode()) { text = "Add Root Node" });
        toolbar.Add(new ToolbarButton(() => _graphView?.AutoLayout()) { text = "Auto Layout" });
        toolbar.Add(new ToolbarButton(() =>
        {
            if (_currentTree == null) return;
            EditorUtility.SetDirty(_currentTree);
            AssetDatabase.SaveAssets();
        })
        { text = "Save" });

        rootVisualElement.Add(toolbar);

        _graphView = new DecisionTreeGraphView { name = "Decision Tree Graph" };
        _graphView.style.flexGrow = 1;
        rootVisualElement.Add(_graphView);

        if (Selection.activeObject is DecisionTree selectedTree)
        {
            LoadTree(selectedTree);
        }
    }

    private void OnSelectionChange()
    {
        if (Selection.activeObject is DecisionTree tree && tree != _currentTree)
        {
            LoadTree(tree);
        }
    }

    public void LoadTree(DecisionTree tree)
    {
        _currentTree = tree;
        _treeField?.SetValueWithoutNotify(tree);
        _graphView?.PopulateFromTree(tree);
    }
}

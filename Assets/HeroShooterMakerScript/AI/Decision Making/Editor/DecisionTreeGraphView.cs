using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace HeroShooterMaker.AI
{
    public class DecisionTreeGraphView : GraphView
    {
        private DecisionTree _tree;
        private readonly Dictionary<DecisionTreeNode, DecisionTreeNodeView> _nodeViews = new Dictionary<DecisionTreeNode, DecisionTreeNodeView>();

        public DecisionTreeGraphView()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ContentZoomer());

            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            graphViewChanged = OnGraphViewChanged;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter adapter)
        {
            return ports.ToList().Where(p =>
                p.direction != startPort.direction &&
                p.node != startPort.node &&
                p.portType == startPort.portType).ToList();
        }

        // ---------------- Loading ----------------

        public void PopulateFromTree(DecisionTree tree)
        {
            // IMPORTANT: use RemoveElement, not DeleteElements, to clear the canvas.
            // DeleteElements() routes through graphViewChanged (the same path used when the
            // user presses Delete), which would treat every visible node as "deleted by the
            // user" and destroy its underlying ScriptableObject asset. RemoveElement only
            // clears the visual representation and leaves the data model untouched.
            foreach (var element in graphElements.ToList())
            {
                RemoveElement(element);
            }
            _nodeViews.Clear();
            _tree = tree;

            if (_tree == null) return;

            // Defensive: drop any null entries left behind by externally deleted assets.
            _tree.AllNodes.RemoveAll(n => n == null);

            foreach (var node in _tree.AllNodes)
            {
                CreateNodeView(node);
            }

            foreach (var node in _tree.AllNodes)
            {
                if (node.ChildYesNode != null && _nodeViews.TryGetValue(node.ChildYesNode, out var yesChildView))
                    ConnectView(_nodeViews[node], yesChildView, true);

                if (node.ChildNoNode != null && _nodeViews.TryGetValue(node.ChildNoNode, out var noChildView))
                    ConnectView(_nodeViews[node], noChildView, false);
            }

            RefreshRootMarker();
        }

        private void ConnectView(DecisionTreeNodeView parent, DecisionTreeNodeView child, bool yesBranch)
        {
            var outputPort = yesBranch ? parent.YesPort : parent.NoPort;
            var edge = outputPort.ConnectTo(child.InputPort);
            AddElement(edge);
        }

        private DecisionTreeNodeView CreateNodeView(DecisionTreeNode node)
        {
            var view = new DecisionTreeNodeView(node)
            {
                OnSetAsRoot = SetRoot
            };
            AddElement(view);
            _nodeViews[node] = view;
            return view;
        }

        // ---------------- Node creation / deletion ----------------

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            var graphMousePos = contentViewContainer.WorldToLocal(evt.mousePosition);

            if (_tree != null)
            {
                evt.menu.AppendAction("Create Node", _ => CreateNode(graphMousePos));
            }
            base.BuildContextualMenu(evt);
        }

        public DecisionTreeNodeView CreateNode(Vector2 position)
        {
            if (_tree == null)
            {
                Debug.LogWarning("Assign a Decision Tree before adding nodes.");
                return null;
            }

            var node = ScriptableObject.CreateInstance<DecisionTreeNode>();
            node.name = "Node";
            node.EditorPosition = position;

            AssetDatabase.AddObjectToAsset(node, _tree);
            AssetDatabase.SaveAssets();

            _tree.AllNodes.Add(node);
            EditorUtility.SetDirty(_tree);

            var view = CreateNodeView(node);
            view.SetPosition(new Rect(position, new Vector2(200, 150)));
            RefreshRootMarker();
            return view;
        }

        public void CreateRootNode()
        {
            var view = CreateNode(new Vector2(100, 100));
            if (view != null) SetRoot(view.Node);
        }

        public void SetRoot(DecisionTreeNode node)
        {
            if (_tree == null) return;
            _tree.DecisionTreeRoot = node;
            EditorUtility.SetDirty(_tree);
            RefreshRootMarker();
        }

        private void RefreshRootMarker()
        {
            foreach (var kv in _nodeViews)
            {
                kv.Value.SetRootHighlight(_tree != null && _tree.DecisionTreeRoot == kv.Key);
            }
        }

        // ---------------- Change handling ----------------

        private GraphViewChange OnGraphViewChanged(GraphViewChange change)
        {
            if (change.edgesToCreate != null)
            {
                foreach (var edge in change.edgesToCreate)
                {
                    if (edge.output.node is DecisionTreeNodeView parentView &&
                        edge.input.node is DecisionTreeNodeView childView)
                    {
                        bool isYesBranch = edge.output == parentView.YesPort;
                        ApplyConnection(parentView.Node, childView.Node, isYesBranch);
                    }
                }
            }

            if (change.elementsToRemove != null)
            {
                foreach (var element in change.elementsToRemove)
                {
                    if (element is Edge edge)
                    {
                        if (edge.output?.node is DecisionTreeNodeView parentView &&
                            edge.input?.node is DecisionTreeNodeView childView)
                        {
                            bool isYesBranch = edge.output == parentView.YesPort;
                            RemoveConnection(parentView.Node, childView.Node, isYesBranch);
                        }
                    }
                    else if (element is DecisionTreeNodeView nodeView)
                    {
                        DeleteNodeAsset(nodeView.Node);
                        _nodeViews.Remove(nodeView.Node);
                    }
                }
                RefreshRootMarker();
            }

            return change;
        }

        private void ApplyConnection(DecisionTreeNode parent, DecisionTreeNode child, bool isYesBranch)
        {
            if (isYesBranch)
                parent.ChildYesNode = child;
            else
                parent.ChildNoNode = child;

            if (!child.ParentNodes.Contains(parent))
                child.ParentNodes.Add(parent);

            EditorUtility.SetDirty(parent);
            EditorUtility.SetDirty(child);
        }

        private void RemoveConnection(DecisionTreeNode parent, DecisionTreeNode child, bool isYesBranch)
        {
            if (isYesBranch && parent.ChildYesNode == child)
                parent.ChildYesNode = null;
            else if (!isYesBranch && parent.ChildNoNode == child)
                parent.ChildNoNode = null;

            child.ParentNodes.Remove(parent);

            EditorUtility.SetDirty(parent);
            EditorUtility.SetDirty(child);
        }

        private void DeleteNodeAsset(DecisionTreeNode node)
        {
            if (_tree == null || node == null) return;

            // Detach from parents.
            node.removeFromParent();

            // Detach children's back-references so they don't keep a dangling parent.
            if (node.ChildYesNode != null) node.ChildYesNode.ParentNodes.Remove(node);
            if (node.ChildNoNode != null) node.ChildNoNode.ParentNodes.Remove(node);

            _tree.AllNodes.Remove(node);
            if (_tree.DecisionTreeRoot == node) _tree.DecisionTreeRoot = null;

            EditorUtility.SetDirty(_tree);

            AssetDatabase.RemoveObjectFromAsset(node);
            Undo.DestroyObjectImmediate(node);
            AssetDatabase.SaveAssets();
        }

        // ---------------- Auto layout ----------------

        public void AutoLayout()
        {
            if (_tree == null || _tree.DecisionTreeRoot == null) return;

            const float xSpacing = 320f;
            const float ySpacing = 180f;
            float nextY = 0f;

            float Layout(DecisionTreeNode node, int depth, HashSet<DecisionTreeNode> visited)
            {
                if (node == null || visited.Contains(node)) return nextY;
                visited.Add(node);

                float y;
                bool isLeaf = node.ChildYesNode == null && node.ChildNoNode == null;

                if (isLeaf)
                {
                    y = nextY;
                    nextY += ySpacing;
                }
                else
                {
                    float yesX = node.ChildYesNode != null ? Layout(node.ChildYesNode, depth + 1, visited) : nextY;
                    float noX = node.ChildNoNode != null ? Layout(node.ChildNoNode, depth + 1, visited) : nextY;
                    y = (yesX + noX) / 2f;
                }

                node.EditorPosition = new Vector2(depth * xSpacing, y);
                if (_nodeViews.TryGetValue(node, out var view))
                    view.SetPosition(new Rect(node.EditorPosition, view.GetPosition().size));

                return y;
            }

            Layout(_tree.DecisionTreeRoot, 0, new HashSet<DecisionTreeNode>());
            EditorUtility.SetDirty(_tree);
        }
    }
}
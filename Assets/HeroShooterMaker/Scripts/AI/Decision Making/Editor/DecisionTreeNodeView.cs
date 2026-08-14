using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace HeroShooterMaker.AI
{
    public class DecisionTreeNodeView : Node
    {
        public DecisionTreeNode Node { get; }
        public Port InputPort { get; private set; }
        public Port YesPort { get; private set; }
        public Port NoPort { get; private set; }

        public Action<DecisionTreeNode> OnSetAsRoot;

        private readonly SerializedObject _serializedObject;
        private VisualElement _conditionContainer;
        private VisualElement _determineAimContainer;
        private VisualElement _determineInputContainer;
        private VisualElement _determineMovementContainer;

        public DecisionTreeNodeView(DecisionTreeNode node)
        {
            Node = node;
            _serializedObject = new SerializedObject(node);

            title = string.IsNullOrEmpty(node.name) ? "Node" : node.name;
            viewDataKey = node.GetEntityId().ToString();

            SetPosition(new Rect(node.EditorPosition, new Vector2(200, 150)));

            BuildPorts();
            BuildFields();

            RefreshExpandedState();
            RefreshPorts();
            RefreshMode();
        }

        private void BuildPorts()
        {
            InputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            InputPort.portName = "Parent";
            inputContainer.Add(InputPort);

            YesPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            YesPort.portName = "Yes";
            YesPort.portColor = new Color(0.35f, 0.85f, 0.35f);
            outputContainer.Add(YesPort);

            NoPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            NoPort.portName = "No";
            NoPort.portColor = new Color(0.9f, 0.35f, 0.35f);
            outputContainer.Add(NoPort);
        }

        private void BuildFields()
        {
            var nameField = new TextField("Name") { value = Node.name };
            nameField.RegisterValueChangedCallback(evt =>
            {
                Node.name = evt.newValue;
                title = string.IsNullOrEmpty(evt.newValue) ? "Node" : evt.newValue;
                EditorUtility.SetDirty(Node);
            });
            extensionContainer.Add(nameField);

            var isActionProp = _serializedObject.FindProperty("IsAction");
            var isActionField = new PropertyField(isActionProp, "Action Node?");
            isActionField.Bind(_serializedObject);
            isActionField.RegisterValueChangeCallback(_ => RefreshMode());
            extensionContainer.Add(isActionField);

            // ---- Condition mode fields ----
            _conditionContainer = new VisualElement();

            var conditionProp = _serializedObject.FindProperty("NodeCondition");
            var conditionField = new PropertyField(conditionProp, "Condition");
            conditionField.Bind(_serializedObject);
            _conditionContainer.Add(conditionField);

            var parameterProp = _serializedObject.FindProperty("ParameterFloat");
            var parameterField = new PropertyField(parameterProp, "Parameter");
            parameterField.Bind(_serializedObject);
            _conditionContainer.Add(parameterField);

            extensionContainer.Add(_conditionContainer);

            // ---- Action mode fields ----
            _determineAimContainer = new VisualElement();

            var aimProp = _serializedObject.FindProperty("determineAim");
            var aimField = new PropertyField(aimProp, "Aim");
            aimField.Bind(_serializedObject);
            _determineAimContainer.Add(aimField);

            extensionContainer.Add(_determineAimContainer);

            _determineInputContainer = new VisualElement();

            var inputProp = _serializedObject.FindProperty("determineInput");
            var inputField = new PropertyField(inputProp, "Input");
            inputField.Bind(_serializedObject);
            _determineInputContainer.Add(inputField);

            extensionContainer.Add(_determineInputContainer);

            _determineMovementContainer = new VisualElement();

            var moveProp = _serializedObject.FindProperty("determineMovement");
            var moveField = new PropertyField(moveProp, "Movement");
            moveField.Bind(_serializedObject);
            _determineMovementContainer.Add(moveField);

            extensionContainer.Add(_determineMovementContainer);

            var setRootButton = new Button(() => OnSetAsRoot?.Invoke(Node)) { text = "Set As Root" };
            extensionContainer.Add(setRootButton);
        }

        private void RefreshMode()
        {
            bool isAction = Node.IsAction;

            _conditionContainer.style.display = isAction ? DisplayStyle.None : DisplayStyle.Flex;
            _determineAimContainer.style.display = isAction ? DisplayStyle.Flex : DisplayStyle.None;
            _determineInputContainer.style.display = isAction ? DisplayStyle.Flex : DisplayStyle.None;
            _determineMovementContainer.style.display = isAction ? DisplayStyle.Flex : DisplayStyle.None;

            YesPort.style.display = isAction ? DisplayStyle.None : DisplayStyle.Flex;
            NoPort.style.display = isAction ? DisplayStyle.None : DisplayStyle.Flex;

            RefreshExpandedState();
            RefreshPorts();
        }

        public void SetRootHighlight(bool isRoot)
        {
            titleContainer.style.backgroundColor = isRoot
                ? new StyleColor(new Color(0.2f, 0.5f, 0.85f))
                : new StyleColor(StyleKeyword.Null);
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Node.EditorPosition = newPos.position;
            EditorUtility.SetDirty(Node);
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Set As Root", _ => OnSetAsRoot?.Invoke(Node));
            evt.menu.AppendSeparator();
            base.BuildContextualMenu(evt);
        }
    }
}
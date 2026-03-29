using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Ability),true)]
public class AbilityEdtior : Editor
{
    public override void OnInspectorGUI()
    {
        Ability ability = (Ability)target;
        Editor.CreateEditor(ability.Stats).OnInspectorGUI();
        base.OnInspectorGUI();
    }
}

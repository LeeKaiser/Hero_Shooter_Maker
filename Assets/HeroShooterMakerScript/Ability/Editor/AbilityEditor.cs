using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using HeroShooterMaker.Abilities;

//Custom Editor for Abilities
[CustomEditor(typeof(Ability),true)]
public class AbilityEdtior : Editor
{
    public override void OnInspectorGUI()
    {
        Ability ability = (Ability)target;

        //draws normal content of ability
        EditorGUILayout.LabelField("-- ABILITY ASSOCIATED OBJECTS --", EditorStyles.boldLabel);
        DrawDefaultInspector();

        //draw stats inspector page
        if (ability.Stats != null)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("-- ABILITY STATS --", EditorStyles.boldLabel);
            Editor.CreateEditor(ability.Stats).OnInspectorGUI();
        }
        
        //draw buttons that loads other game objects 
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("-- ABILITY OBJECT INSPECTORS --", EditorStyles.boldLabel);
        System.Type type = ability.GetType();
        while (type != null && type != typeof(MonoBehaviour))
        {
            FieldInfo[] fields = type.GetFields(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic
            );

            foreach (FieldInfo field in fields)
            {
                if (field.IsPrivate && field.GetCustomAttribute<SerializeField>() == null)
                    continue;

                DrawOpenButton(ability, field);
            }

            type = type.BaseType;
        }
    }

    //draws buttons for game objects
    private void DrawOpenButton(Ability ability, FieldInfo field)
    {
        object value = field.GetValue(ability);
        if (value == null) return;

        Object unityObj = null;

        if (field.FieldType == typeof(GameObject))
            unityObj = (GameObject)value;
        else if (typeof(Component).IsAssignableFrom(field.FieldType))
            unityObj = (Component)value;

        if (unityObj == null) return;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(field.Name, GUILayout.Width(150));
        if (GUILayout.Button($"Open Inspector", GUILayout.Width(120)))
        {
            EditorUtility.OpenPropertyEditor(unityObj);
        }
        EditorGUILayout.EndHorizontal();
    }

}

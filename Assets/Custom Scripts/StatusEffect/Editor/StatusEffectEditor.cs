using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(StatusEffect),true)]
public class StatusEffectEdtior : Editor
{
    //created with assistance of Claude AI
    public override void OnInspectorGUI()
    {
        StatusEffect status = (StatusEffect)target;

        //draws normal content of status
        EditorGUILayout.LabelField("-- STATUS ASSOCIATED OBJECTS --", EditorStyles.boldLabel);
        DrawDefaultInspector();

        //draw stats inspector page
        if (status.Stats != null)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("-- STATUS EFFECT STATS --", EditorStyles.boldLabel);
            Editor.CreateEditor(status.Stats).OnInspectorGUI();
        }
        
        //draw buttons that loads other game objects 
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("-- STATUS EFFECT OBJECT INSPECTORS --", EditorStyles.boldLabel);
        System.Type type = status.GetType();
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

                DrawOpenButton(status, field);
            }

            type = type.BaseType;
        }
    }

    //draws buttons for game objects
    private void DrawOpenButton(StatusEffect status, FieldInfo field)
    {
        object value = field.GetValue(status);
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

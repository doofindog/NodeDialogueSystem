using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DialogueSystem;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EventEntry))]
public class EventEntryInspector : Editor
{
    private int typeIndex = 0;
    private int methodIndex = 0;
    
    public override void OnInspectorGUI()
    {
        EventEntry eventEntry = target as EventEntry;
        if (eventEntry == null)
        {
            return;
        }

        #region Text
        
        GUIStyle headingStyle = new GUIStyle(GUI.skin.label)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 15
        };

        EditorGUILayout.LabelField("Dialogue Text", headingStyle);
        EditorGUILayout.Space(2);
        
        GUI.skin.textField.wordWrap = true;
        eventEntry.text = EditorGUILayout.TextArea(eventEntry.text);
        GUI.skin.textField.wordWrap = false;

        #endregion
        
        EditorGUILayout.Space(50);
        
        #region Static Events

        EditorGUILayout.LabelField("Static Event", headingStyle);
        
        System.Type[] eventTypes = CachedData.eventTypes;
        string[] eventNames = new string[eventTypes.Length];
        for (int i = 0; i < eventTypes.Length; i++)
        {
            eventNames[i] = eventTypes[i].Name;
            if (eventEntry.m_type.type != null)
            {
                if (eventNames[i].Equals(eventEntry.m_type.type.Name))
                {
                    typeIndex = i;
                }
            }
        }
        typeIndex = EditorGUILayout.Popup("Event Type", typeIndex, eventNames);

        
        MethodInfo[] methods = CachedData.GetMethods(eventTypes[typeIndex]);
        string[] methodNames = new string[methods.Length];
        for (int i = 0; i < methods.Length; i++)
        {
            methodNames[i] = methods[i].Name;
            if (eventEntry.m_method.methodInfo != null)
            {
                if (eventEntry.m_method.methodInfo.Name == methodNames[i])
                {
                    methodIndex = i;
                }
            }
        }

        methodIndex = EditorGUILayout.Popup("Method", methodIndex, methodNames);
        
        eventEntry.SetEvent(methods[methodIndex]);

        #endregion

        EditorGUILayout.Space(50);
        
        #region UnityEvents
        
        EditorGUILayout.LabelField("Unity Event", headingStyle);
        
        DrawDefaultInspector();
        
        #endregion
    }
}

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

        #region UnityEvents
        
        EditorGUILayout.LabelField("Unity Event", headingStyle);
        
        DrawDefaultInspector();
        
        #endregion
    }
}

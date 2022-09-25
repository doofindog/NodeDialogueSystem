using System;
using System.Collections.Generic;
using DialogueSystem;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

[CustomEditor(typeof(NpcDialogueTrigger))]
[CanEditMultipleObjects]
public class NpcDialogueInspector : Editor
{
    private int _index = -1;
    private SerializedObject _selectedObj = null;
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NpcDialogueTrigger trigger = (NpcDialogueTrigger) target;

        SerializedProperty selectedProperty = serializedObject.FindProperty("_selectedConversation");
        if (selectedProperty.objectReferenceValue != null)
        {
            _selectedObj = new SerializedObject(selectedProperty.objectReferenceValue);
        }
        
        SerializedProperty dialogueProperty = serializedObject.FindProperty("_dialogueList");
        SerializedObject dialogueObj = new SerializedObject(dialogueProperty.objectReferenceValue);

        SerializedProperty conversations = dialogueObj.FindProperty("_conversations");
        List<string> values = new List<string>();
        
        if (conversations.isArray)
        {
            int arrayLength = 0;
 
            conversations.Next(true); // skip generic field
            conversations.Next(true); // advance to array size field
            
            arrayLength = conversations.intValue;
 
            conversations.Next(true); // advance to first array index
            
            int lastIndex = arrayLength - 1;
            for (int i = 0; i < arrayLength; i++)
            {
                SerializedObject obj = new SerializedObject(conversations.objectReferenceValue);
                SerializedProperty property = obj.FindProperty("_name");
                values.Add(property.stringValue);
                if (_index == i)
                {
                    trigger._selectedConversation = conversations.objectReferenceValue as ConversationGraph;
                }

                if(i < lastIndex) conversations.Next(false);
            }
        }

        _index = EditorGUILayout.Popup(_index, values.ToArray());
        
        serializedObject.ApplyModifiedProperties();
        SaveManager.SaveData(trigger);
    }
}

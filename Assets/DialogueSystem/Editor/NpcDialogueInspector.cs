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
    private int index = -1;
    private string selectedConversationName = string.Empty;
    private SerializedObject selectedObj = null;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NpcDialogueTrigger trigger = (NpcDialogueTrigger) target;

        SerializedProperty selectedProperty = serializedObject.FindProperty("_selectedConversation");
        if (selectedProperty.objectReferenceValue != null)
        {
            selectedObj = new SerializedObject(selectedProperty.objectReferenceValue);
        }
        


        SerializedProperty dialogueProperty = serializedObject.FindProperty("_dialogueList");
        SerializedObject dialogueObj = new SerializedObject(dialogueProperty.objectReferenceValue);

        SerializedProperty conversations = dialogueObj.FindProperty("conversations");
        List<string> values = null;
        if (conversations.isArray)
        {
            int arrayLength = 0;
 
            conversations.Next(true); // skip generic field
            conversations.Next(true); // advance to array size field
            
            arrayLength = conversations.intValue;
 
            conversations.Next(true); // advance to first array index
            
            values = new List<string>(arrayLength);
            int lastIndex = arrayLength - 1;
            for (int i = 0; i < arrayLength; i++)
            {
                SerializedObject obj = new SerializedObject(conversations.objectReferenceValue);
                SerializedProperty property = obj.FindProperty("name");
                values.Add(property.stringValue);
                if (index == i)
                {
                    trigger._selectedConversation = conversations.objectReferenceValue as ConversationGraph;
                }

                if(i < lastIndex) conversations.Next(false);
            }
        }

        if (values != null)
        {
            index = EditorGUILayout.Popup(index, values.ToArray());
        }
        
        serializedObject.ApplyModifiedProperties();
        SaveManager.SaveData(trigger);
    }
}

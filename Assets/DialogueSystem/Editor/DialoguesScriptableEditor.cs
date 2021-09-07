using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(DialoguesScriptable))]
public class DialoguesScriptableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Open Dialogue Window"))
        {
            DialoguesScriptable dialogue = (DialoguesScriptable)target;
            DialogueEditorManager.OpenDialogueWindow(dialogue);
        }
    }
    
    
}

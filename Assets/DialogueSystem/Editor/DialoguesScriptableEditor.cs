using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
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

    [MenuItem("DialogueSystem/CreateDialogue")]
    public static void CreateAsset()
    {
        if (!Directory.Exists(Application.dataPath + "/DialogueSystem/Dialogues"))
        {
            Directory.CreateDirectory(Application.dataPath + "/DialogueSystem/Dialogues");
        }
        AssetDatabase.Refresh();
        
        DialoguesScriptable dialogueScriptable = ScriptableObject.CreateInstance<DialoguesScriptable>();
        
        AssetDatabase.CreateAsset(dialogueScriptable,"Assets/DialogueSystem/Dialogues/"+dialogueScriptable.id+".asset");
        AssetDatabase.SaveAssets();
        
        Selection.activeObject = dialogueScriptable;
        DialogueEditorManager.CreateData(dialogueScriptable);
        EditorUtility.SetDirty(dialogueScriptable);
    }
    
    
}

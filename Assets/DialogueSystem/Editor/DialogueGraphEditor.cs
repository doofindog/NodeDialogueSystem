using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [CustomEditor(typeof(DialogueGraph))]
    public class DialogueGraphEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Open Dialogue Window"))
            {
                DialogueGraph dialogueGraph = (DialogueGraph)target;
                EditorManager.OpenDialogueWindow(dialogueGraph);
            }
            if (GUILayout.Button("Print Connector Count"))
            {
                DialogueGraph dialogueGraph = (DialogueGraph)target;
                Debug.Log(dialogueGraph.connectors.Count);
            }
            if (GUILayout.Button("Clear Data"))
            {
                DialogueGraph dialogueGraph = (DialogueGraph)target;
                dialogueGraph.connectors = new List<DialogueSystem.Connection>();
                dialogueGraph.dialogues = new List<DialogueSystem.Dialogue>();
            }
        
        }
    
    }
}

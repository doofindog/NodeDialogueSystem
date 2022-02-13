using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [CustomEditor(typeof(DialogueGraph))]
    public class GraphEditor : UnityEditor.Editor
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
            }
            if (GUILayout.Button("Clear Data"))
            {
                DialogueGraph dialogueGraph = (DialogueGraph)target;
                dialogueGraph.nodes = new List<DialogueSystem.Node>();
            }
        
        }
    
    }
}

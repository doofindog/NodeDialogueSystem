using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [CustomEditor(typeof(Graph))]
    public class GraphEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Open Dialogue Window"))
            {
                Graph graph = (Graph)target;
                EditorManager.OpenDialogueWindow(graph);
            }
            if (GUILayout.Button("Print Connector Count"))
            {
                Graph graph = (Graph)target;
            }
            if (GUILayout.Button("Clear Data"))
            {
                Graph graph = (Graph)target;
                graph.nodes = new List<DialogueSystem.Node>();
            }
        
        }
    
    }
}

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [CustomEditor(typeof(DialogueDatabase))]
    public class DialogueDBInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (GUILayout.Button("Open Dialogue Window"))
            {
                DialogueDatabase database = (DialogueDatabase)target;
                DatabaseEditorManager.OpenDatabaseWindow(database);
            }
        }
    
    }
}

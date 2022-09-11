using System;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Editor.NodeComponents;
using DialogueSystem.Editor.NodeEditors;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [System.Serializable]
    public class DatabaseWindow : EditorWindow
    {
        public DialogueDatabase dialogueDB;
        

        public ConversationGraph selectedGraph;
        public List<Node> nodes;
        
        public int convIndex = 0;
        public int preIndex = 0;

        private PortEditor _SelectINPort;
        private PortEditor _SelectdOutPort;
        
        private Vector2 offset;
        private Vector2 drag;

        public void Initialise(DialogueDatabase dialogueDB)
        {
            this.dialogueDB = dialogueDB;
            name = dialogueDB.name;

            nodes = new List<Node>();
        }

        public void OnEnable()
        {
            this.dialogueDB = null;
        }

        private void OnGUI()
        {
            if (dialogueDB == null) { return; }
            
            GUIStyle headerStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold
            };
            EditorGUILayout.LabelField(dialogueDB.name,headerStyle);

            EditorGUILayout.BeginHorizontal();
            

            if (dialogueDB.GetAllConversations() != null)
            {
                List<string> dropDownContent = new List<string>();
                foreach (ConversationGraph conv in dialogueDB.GetAllConversations())
                {
                    dropDownContent.Add(conv.GetName());
                }
                
                EditorGUI.BeginChangeCheck();
                convIndex = EditorGUILayout.Popup("Conversations", convIndex, dropDownContent.ToArray());
                if (EditorGUI.EndChangeCheck())
                {
                    selectedGraph = dialogueDB.GetConversationGraphAtIndex(convIndex);
                    Selection.activeObject = selectedGraph;
                    LoadNodes();
                }
                
            }
            
            if (GUILayout.Button("+",GUILayout.Width(50)))
            {
                selectedGraph = dialogueDB.CreateNewConversation();
                Selection.activeObject = selectedGraph;
                LoadNodes();
                
                convIndex = dialogueDB.GetAllConversations().Length - 1;
            }
            
            if (GUILayout.Button("-",GUILayout.Width(50)))
            {
                ConversationGraph graph = dialogueDB.GetConversationGraphAtIndex(convIndex);
                dialogueDB.DeleteDialogue(graph);
                convIndex = 0;
            }
            
            EditorGUILayout.EndHorizontal();

            DrawGrid(20, 0.2f, Color.gray);
            DrawGrid(100, 0.4f, Color.gray);

            if (selectedGraph == null)
            {
                if (GUI.changed)
                {
                    Repaint();
                }  
                return;
            }
            
            
            DrawNode();
            
            ProcessNodeEvents(Event.current);
            ProcessEvents(Event.current);
            
            DrawSave();
            
            if (GUI.changed)
            {
                Repaint();
            }    
        }
    
        private void ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                {
                    if (e.button == 1)
                    {
                        OpenContextMenu(e.mousePosition);
                    }
                    break;
                }
                case EventType.MouseDrag:
                {
                    if (e.button == 0)
                    {
                        DragWindow(e.delta);
                        e.Use();
                    }
                    break;
                }
                case EventType.MouseUp:
                {
                    if (e.button == 0)
                    {
                        DatabaseEditorManager.SaveData();
                    }
                    break;
                }
            }
        }

        private void OpenContextMenu(Vector2 contextPosition)
        {
            List<Type> ignoreType = new List<Type>()
            {
                typeof(PointEntry)
            };
            Type[] types = ReflectionHandler.GetDerivedTypes(typeof(Entry));
            GenericMenu contextMenu = new GenericMenu();
            foreach (Type type in types)
            {
                if (!ignoreType.Contains(type))
                {
                    contextMenu.AddItem(new GUIContent(type.Name), false, () =>
                    {
                        selectedGraph.CreateEntry(type, contextPosition);
                        LoadNodes();
                    });
                }
            }
            contextMenu.ShowAsContext();
        }

        #region Nodes
        
        public void CreateNode(Entry entry)
        {
            Type nodeType = entry.GetType();
            Type editorType = DatabaseEditorManager.GetCustomEditor(nodeType);
            
            Node node = ScriptableObject.CreateInstance(editorType) as Node;
            if (node == null)
            {
                return;
            }

            node.Init(entry,this);
            
            nodes.Add(node);
        }
        
        public void LoadNodes()
        {
            nodes.Clear();
            if (selectedGraph == null) { return; }
            if (selectedGraph.GetEntries() == null) { return; } 

            Entry[] entries = selectedGraph.GetEntries();
            for (int i = 0; i < entries.Length; i++)
            {
                CreateNode(entries[i]);
            }
        }

        private void DrawNode()
        {
            if (nodes == null) { return; }
            
            for (int i = 0; i < nodes.Count; i++)
            {
                NodeComponentUtilt.focusedNode = nodes[i];
                nodes[i].Draw();
            }
        }
        
        public void RemoveNode(Node node)
        {
            selectedGraph.RemoveDialogue(node.entry);
            DatabaseEditorManager.SaveData();
            Repaint();
        }
        
        private void ProcessNodeEvents(Event e)
        {
            if (nodes != null)
            {
                foreach (Node node in nodes)
                {
                    node.ProcessEvent(e);
                }
            }
        }
        

        #endregion


        public void DrawSave()
        {
            Rect buttonRect;
            Vector2 buttonSize = new Vector2(200, 100);
            Vector2 buttonPosition = new Vector2(position.width - buttonSize.x ,position.height - buttonSize.y );
            buttonRect = new Rect(buttonPosition, buttonSize);
            if (GUI.Button(buttonRect,"Save"))
            {
                DatabaseEditorManager.SaveData();
            }
        }

        private void DragWindow(Vector2 newPosition)
        {
            if (nodes != null)
            {
                foreach (Node node in nodes)
                {
                    node.UpdatePosition(newPosition);
                }
            }
        }
        
        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);
 
            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);
 
            offset += drag * 0.5f;
            Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);
 
            for (int i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
            }
 
            for (int j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
            }
 
            Handles.color = Color.white;
            Handles.EndGUI();
        }
    }
}




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
        public Dictionary<Entry,Node> nodes;
        public Dictionary<Node, LinkEditor> linkers;
        public int convIndex = 0;

        private Node _sourceNode;
        private Node _destinationNode;


        private Vector2 _offset;
        private Vector2 _drag;
        
        public bool isDragged;

        public void Initialise(DialogueDatabase dialogueDB)
        {
            this.dialogueDB = dialogueDB;

            _sourceNode = null;
            _destinationNode = null;
            nodes = new Dictionary<Entry,Node>();
            linkers = new Dictionary<Node, LinkEditor>();
            
            if (dialogueDB.GetAllConversations().Count > 0)
            {
                selectedGraph = dialogueDB.GetConversationGraphAtIndex(0);
                LoadNodes();
            }
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
                
                convIndex = dialogueDB.GetAllConversations().Count - 1;
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
            DrawLinks();

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

                    if (e.button == 0)
                    {
                        if (selectedGraph != null)
                        {
                            Selection.activeObject = selectedGraph;
                        }
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
                typeof(StartEntry)
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
            nodes.Add(entry,node);
        }

        public Node GetNode(Entry entry)
        {
            if (!nodes.ContainsKey(entry))
            {
                return null;
            }

            return nodes[entry];
        }

        public void LoadNodes()
        {
            nodes.Clear();
            if (selectedGraph == null) { return; }
            if (selectedGraph.GetEntries() == null) { return; }
            

            Entry[] entries = selectedGraph.GetEntries();
            foreach (var entry in entries)
            {
                CreateNode(entry);
            }
        }

        private void DrawNode()
        {
            foreach (Entry entry in nodes.Keys)
            {
                Node node = GetNode(entry);
                if (node == null)
                {
                    continue;
                }

                NodeComponentUtilt.focusedNode = node;
                node.Draw();
            }
        }
        
        public void RemoveNode(Node node)
        {
            selectedGraph.RemoveEntry(node.entry);
            DatabaseEditorManager.SaveData();
            LoadNodes();
        }
        
        private void ProcessNodeEvents(Event e)
        {
            foreach (Entry entry in nodes.Keys)
            {
                Node node = GetNode(entry);
                if (node == null)
                {
                    continue;
                }

                node.ProcessEvent(e);
            }
        }
        
        #endregion

        #region Connections

        public void SelectSourceNode(Node node)
        {
            _sourceNode = node;
            TryCreateLink();
        }

        public void SelectDestinationNode(Node node)
        {
            _destinationNode = node;
            TryCreateLink();
        }

        public void TryCreateLink()
        {
            if (_sourceNode != null && _destinationNode != null)
            {
                Entry source = _sourceNode.entry;
                Entry destination = _destinationNode.entry;
                
                Link link = selectedGraph.CreateLink(source, destination);
                
                _sourceNode = null;
                _destinationNode = null;
            }
        }

        public void LoadLink()
        {
            foreach (Entry entry in selectedGraph.links.Keys)
            {
                
            }
        }

        public void DrawLinks()
        {
            if (selectedGraph.links == null)
            {
                return;
            }
            
            foreach (Entry entry in nodes.Keys)
            {
                if (selectedGraph.GetAdjLinks(entry) == null) {continue;}
                
                Node sourceNode = GetNode(entry);
                if (sourceNode == null)
                {
                    continue;
                }
                
                foreach (Entry adjEntry in selectedGraph.GetAdjLinks(entry))
                {
                    Node destinationNode = GetNode(adjEntry);
                    if (destinationNode == null)
                    {
                        Debug.Log("Draw Link : Destination Node does not exist");
                        continue;
                    }

                    Vector2 sourcePos = sourceNode.GetRect().center;
                    Vector2 destinationPos = destinationNode.GetRect().center;
                    Handles.DrawLine(sourcePos, destinationPos);
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
            foreach (Entry entry in nodes.Keys)
            {
                Node node = GetNode(entry);
                if (node == null)
                {
                    continue;
                }
                
                node.UpdatePosition(newPosition);
            }

            GUI.changed = true;
        }
        
        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);
 
            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);
 
            _offset += _drag * 0.5f;
            Vector3 newOffset = new Vector3(_offset.x % gridSpacing, _offset.y % gridSpacing, 0);
 
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
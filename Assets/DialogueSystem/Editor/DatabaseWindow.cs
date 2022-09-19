using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
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
        public int convIndex = 0;

        private Entry _source;
        private Entry _destination;
        private Option _option;
        
        private Vector2 _offset;
        private Vector2 _drag;
        
        

        public void Initialise(DialogueDatabase dialogueDB)
        {
            this.dialogueDB = dialogueDB;

            _source = null;
            _destination = null;
            nodes = new Dictionary<Entry,Node>();

            if (dialogueDB.ContainsConversations())
            {
                DialogueEditorEvents.removeLink += RemoveLink;
                ConversationGraph graph = dialogueDB.GetConversationGraphAtIndex(0);
                LoadConversation(graph);
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

            #region Conversation Selection GUI

            EditorGUILayout.BeginHorizontal();
            
            if (dialogueDB.ContainsConversations())
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
                    ConversationGraph graph = dialogueDB.GetConversationGraphAtIndex(convIndex);
                    LoadConversation(graph);
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

            #endregion

            #region Grid GUI

            DrawGrid(20, 0.2f, Color.gray);
            DrawGrid(100, 0.4f, Color.gray);

            #endregion

            #region Graph GUI
            
            if (selectedGraph == null)
            {
                if (GUI.changed) { Repaint(); }  
                return;
            }
            
            DrawNode();
            DrawLinks();
            
            ProcessNodeEvents(Event.current);
            ProcessEvents(Event.current);
            
            #endregion

            if (GUI.changed) { Repaint(); }    
        }

        #region Events Processes
        
        private void ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                {
                    OnMouseDown(e);
                    break;
                }
                case EventType.MouseDrag:
                {
                    OnMouseDrag(e);
                    break;
                }
                case EventType.MouseUp:
                {
                    OnMouseUp(e);
                    break;
                }
            }
        }

        private void OnMouseDown(Event e)
        {
            if (e.button == 1) { OpenContextMenu(e.mousePosition); }
            if (e.button == 0)
            {
                if (selectedGraph != null)
                {
                    Selection.activeObject = selectedGraph;
                }
            }
        }

        private void OnMouseUp(Event e)
        {
            if (e.button == 0) { DatabaseEditorManager.SaveData(); }
        }

        private void OnMouseDrag(Event e)
        {
            if (e.button == 2)
            {
                DragWindow(e.delta);
                e.Use();
            }
        }
        
        private void OpenContextMenu(Vector2 contextPosition)
        {
            List<Type> ignoreType = new List<Type>() {typeof(StartEntry)};
            Type[] types = ReflectionHandler.GetDerivedTypes(typeof(Entry));
            GenericMenu contextMenu = new GenericMenu();
            
            foreach (Type type in types)
            {
                if (ignoreType.Contains(type)) { continue; }
                contextMenu.AddItem(new GUIContent(type.Name), false, () =>
                {
                    selectedGraph.CreateEntry(type, contextPosition);
                    LoadNodes();
                });
            }
            
            contextMenu.ShowAsContext();
        }
        
        #endregion
        
        #region Conversation

        public void LoadConversation(ConversationGraph conv)
        {
            if(conv == null) { return; }

            Selection.activeObject = selectedGraph = conv;
            LoadNodes();
        }

        #endregion

        #region Nodes
        
        public void CreateNode(Entry entry)
        {
            Type nodeType = entry.GetType();
            Type editorType = DatabaseEditorManager.GetCustomEditor(nodeType);
            
            Node node = ScriptableObject.CreateInstance(editorType) as Node;
            
            if (node == null) { return; }

            node.Init(entry,this);
            nodes.Add(entry,node);
        }

        public Node GetNode(Entry entry)
        {
            return !nodes.ContainsKey(entry) ? null : nodes[entry];
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

        #region Links

        public void SelectSourceNode(Node node, Option option = null)
        {
            _source = node.entry;
            _option = option;
            TryCreateLink();
        }

        public void SelectDestinationNode(Node node)
        {
            _destination = node.entry;
            TryCreateLink();
        }

        public void TryCreateLink()
        {
            if (_source == null || _destination == null) {return;}
            
            Node sourceNode = GetNode(_source);
            Node  destinationNode = GetNode(_destination);
            
            Link link = selectedGraph.CreateLink(_source, _destination, _option);
                
            _source = null;
            _destination = null;
        }

        public void DrawLinks()
        {
            if (!selectedGraph.ContainsLinks())
            {
                return;
            }
            
            foreach (Entry entry in selectedGraph.links.Keys.ToList())
            {
                Link[] adjLinks = selectedGraph.GetAdjLinks(entry);
                if (adjLinks == null)
                {
                    Debug.Log("(Draw Link) : Adj Graph is empty");
                    continue;
                }
                
                foreach (Link link in adjLinks.ToList())
                {
                    LinkEditor linkEditor = new LinkEditor(link);
                    if (link.destinationEntry == null)
                    {
                        RemoveLink(linkEditor);
                        continue;
                    }

                    Port sourcePort = null;
                    if (entry.GetType() == typeof(DecisionEntry))
                    {
                        DecisionNode node = (DecisionNode) GetNode(entry);
                        sourcePort = node.GetOptionPort(link.option);
                    }
                    else
                    {
                        sourcePort = GetNode(link.sourceEntry).GetOutPort();
                    }

                    Port destinationPort = GetNode(link.destinationEntry).GetInPort();

                    Vector2 startPos = sourcePort.GetRect().center;
                    Vector2 endPos = destinationPort.GetRect().center;
                        
                    linkEditor.Draw(startPos,endPos);
                }
            }
        }

        public void RemoveLink(LinkEditor p_linkEditor)
        {
            selectedGraph.RemoveLink(p_linkEditor.GetLink());
        }
        #endregion
        
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
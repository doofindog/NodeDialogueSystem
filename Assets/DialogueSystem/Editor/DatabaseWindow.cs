using System;
using System.Linq;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

using DialogueSystem.Editor.NodeComponents;
using DialogueSystem.Editor.NodeEditors;

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
        
        public void OnEnable()
        {
            this.dialogueDB = null;
        }
        
        public void Initialise(DialogueDatabase p_dialogueDB)
        {
            dialogueDB = p_dialogueDB;
            _source = null;
            _destination = null;
            nodes = new Dictionary<Entry,Node>();

            convIndex = -1;

            if (p_dialogueDB.ContainsConversations())
            {
                DialogueEditorEvents.removeLink += RemoveLink;
                ConversationGraph graph = p_dialogueDB.GetConversationGraphAtIndex(0);
                
                LoadConversation(graph);
            }
        }
        
        private void OnGUI()
        {
            if (dialogueDB == null)
            {
                return;
            }
            
            GUIStyle headerStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold
            };
            EditorGUILayout.LabelField(dialogueDB.name,headerStyle);
            
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////

            #region ----> Conversation Selection GUI <----

            EditorGUILayout.BeginHorizontal(); 
            
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
            
            if (GUILayout.Button("+",GUILayout.Width(50))) { CreateNewConversation(); }
            if (GUILayout.Button("-",GUILayout.Width(50))) { LoadConversation(dialogueDB.GetConversationGraphAtIndex(convIndex)); }
            
            EditorGUILayout.EndHorizontal();

            if (convIndex == -1)
            {
                if (GUI.changed) { Repaint(); }   
                return;
            }

            #endregion
            
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            
            #region ----> Grid GUI <----

            DrawGrid(20, 0.2f, Color.gray);
            DrawGrid(100, 0.4f, Color.gray);

            #endregion
            
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////

            #region ----> Graph GUI <----
            
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
            
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////

            if (GUI.changed) { Repaint(); }    
        }

        #region ----> Events Processes () <-----
        
        private void ProcessEvents(Event p_event)
        {
            switch (p_event.type)
            {
                case EventType.MouseDown:
                {
                    OnMouseDown(p_event);
                    break;
                }
                case EventType.MouseDrag:
                {
                    OnMouseDrag(p_event);
                    break;
                }
                case EventType.MouseUp:
                {
                    OnMouseUp(p_event);
                    break;
                }
            }
        }

        private void OnMouseDown(Event p_event)
        {
            if (p_event.button == 1) { OpenContextMenu(p_event.mousePosition); }
            
            if (p_event.button == 0)
            {
                if (selectedGraph != null)
                {
                    Selection.activeObject = selectedGraph;
                }
            }
        }

        private void OnMouseUp(Event p_event)
        {
            if (p_event.button == 0)
            {
                DatabaseEditorManager.SaveData();
            }
        }

        private void OnMouseDrag(Event p_event)
        {
            if (p_event.button == 2)
            {
                DragWindow(p_event.delta);
                p_event.Use();
            }
        }
        
        private void OpenContextMenu(Vector2 p_contextPosition)
        {
            List<Type> ignoreType = new List<Type>() {typeof(StartEntry)};
            Type[] types = ReflectionHandler.GetDerivedTypes(typeof(Entry));
            GenericMenu contextMenu = new GenericMenu();
            
            foreach (Type type in types)
            {
                if (ignoreType.Contains(type)) { continue; }
                
                contextMenu.AddItem(new GUIContent(type.Name), false, () =>
                {
                    selectedGraph.CreateEntry(type, p_contextPosition);
                    LoadNodes();
                });
            }
            
            contextMenu.ShowAsContext();
        }
        
        #endregion
        
        #region ----> Conversation () <----

        private void CreateNewConversation()
        {
            selectedGraph = dialogueDB.CreateNewConversation();
            Selection.activeObject = selectedGraph;
            LoadNodes();
            convIndex = dialogueDB.GetAllConversations().Count - 1;
        }

        private void DeletedConversation(ConversationGraph p_graph)
        {
            dialogueDB.DeleteDialogue(p_graph);
            convIndex = 0;
        }

        private void LoadConversation(ConversationGraph p_conv)
        {
            if(p_conv == null) { return; }

            Selection.activeObject = selectedGraph = p_conv;
            LoadNodes();
        }

        #endregion

        #region ----> Nodes () <----
        
        public void CreateNode(Entry p_entry)
        {
            Type nodeType = p_entry.GetType();
            Type editorType = DatabaseEditorManager.GetCustomEditor(nodeType);
            
            Node node = ScriptableObject.CreateInstance(editorType) as Node;
            
            if (node == null) { return; }

            node.Init(p_entry,this);
            nodes.Add(p_entry,node);
        }

        public Node GetNode(Entry p_entry)
        {
            return !nodes.ContainsKey(p_entry) ? null : nodes[p_entry];
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
        
        public void RemoveNode(Node p_node)
        {
            selectedGraph.RemoveEntry(p_node.entry);
            DatabaseEditorManager.SaveData();
            LoadNodes();
        }
        
        private void ProcessNodeEvents(Event p_event)
        {
            foreach (Entry entry in nodes.Keys)
            {
                Node node = GetNode(entry);
                if (node == null)
                {
                    continue;
                }

                node.ProcessEvent(p_event);
            }
        }
        
        #endregion

        #region ----> Links () <----

        public void SelectSourceNode(Node p_node, Option p_option = null)
        {
            _source = p_node.entry;
            _option = p_option;
            TryCreateLink();
        }

        public void SelectDestinationNode(Node p_node)
        {
            _destination = p_node.entry;
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
                if (adjLinks == null) { continue; }
                
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
        
        private void DragWindow(Vector2 p_newPosition)
        {
            foreach (Entry entry in nodes.Keys)
            {
                Node node = GetNode(entry);
                if (node == null)
                {
                    continue;
                }
                
                node.UpdatePosition(p_newPosition);
            }

            GUI.changed = true;
        }
        
        private void DrawGrid(float p_gridSpacing, float p_gridOpacity, Color p_gridColor)
        {
            int widthDivs = Mathf.CeilToInt(position.width / p_gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / p_gridSpacing);
 
            Handles.BeginGUI();
            Handles.color = new Color(p_gridColor.r, p_gridColor.g, p_gridColor.b, p_gridOpacity);
 
            _offset += _drag * 0.5f;
            Vector3 newOffset = new Vector3(_offset.x % p_gridSpacing, _offset.y % p_gridSpacing, 0);
 
            for (int i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(p_gridSpacing * i, -p_gridSpacing, 0) + newOffset, new Vector3(p_gridSpacing * i, position.height, 0f) + newOffset);
            }
 
            for (int j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(new Vector3(-p_gridSpacing, p_gridSpacing * j, 0) + newOffset, new Vector3(position.width, p_gridSpacing * j, 0f) + newOffset);
            }
 
            Handles.color = Color.white;
            Handles.EndGUI();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [System.Serializable]
    public class GraphWindow : EditorWindow
    {
        public string id;
        public Graph graph;

        public Dictionary<Node,NodeEditor> nodesEditors;
        public Dictionary<PortEditor, PortEditor> connections;

        private PortEditor _SelectINPort;
        private PortEditor _SelectdOutPort;


        public void OnEnable()
        {
            this.graph = null;
            id = null;
            _SelectINPort = null;
            _SelectdOutPort = null; 
        }

        public void Initialise(Graph graph)
        {
            
            this.graph = graph;
            id = graph.id;
            
            nodesEditors = new Dictionary<Node, NodeEditor>();
            connections = new Dictionary<PortEditor, PortEditor>();
            DialogueSystem.Node[] nodes = graph.GetNodes();
            if (nodes != null)
            {
                foreach (Node node in nodes)
                {
                    CreateNode(node);
                }
            }
            
            _SelectINPort = null;
            _SelectdOutPort = null; 
            
            LoadConnection();
        }
        
        private void OnGUI()
        {
            if (graph != null)
            {
                GUILayout.Label("Dialogue Editor Window", EditorStyles.boldLabel);
                
                DrawNodeGUI();
                DrawConnectionsGUI();
                ProcessNodeEvents(Event.current);
                ProcessEvents(Event.current);
                
                DrawSave();
                
                if (GUI.changed)
                {
                    Repaint();
                }
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
                        DragNodes(e.delta);
                        e.Use();
                    }
                    break;
                }
                case EventType.MouseUp:
                {
                    if (e.button == 0)
                    {
                        EditorManager.SaveData();
                    }
                    break;
                }
            }
        }
    
        private void ProcessNodeEvents(Event e)
        {
            if (nodesEditors != null)
            {
                foreach (Node node in nodesEditors.Keys)
                {
                    nodesEditors[node].ProcessEvent(e);
                }
            }
        }
    
        private void DrawNodeGUI()
        {
            if (nodesEditors != null)
            {
                foreach (Node node in nodesEditors.Keys)
                {
                    nodesEditors[node].Draw();
                }
            }
        }

        private void DrawConnectionsGUI()
        {
            if (connections == null)
            {
                connections = new Dictionary<PortEditor, PortEditor>();
            }
            
            foreach (PortEditor portEditors in connections.Keys.ToList())
            {
                PortEditor outPortEditor = portEditors;
                PortEditor inPortEditor = connections[portEditors];
                //Handles.DrawBezier(outPortEditor.GetCenterPosition(), inPortEditor.GetCenterPosition(),outPortEditor.GetCenterPosition() + Vector2.up * 50f , inPortEditor.GetCenterPosition() + Vector2.down * 50f, Color.white,null, 2f);
                Handles.DrawLine(outPortEditor.GetCenterPosition(),inPortEditor.GetCenterPosition());
                if (Handles.Button((outPortEditor.GetCenterPosition() + inPortEditor.GetCenterPosition()) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
                {
                    RemoveConnection(outPortEditor);
                }
            }
        }

        public void DrawSave()
        {
            Rect buttonRect;
            Vector2 buttonSize = new Vector2(200, 100);
            Vector2 buttonPosition = new Vector2(position.width - buttonSize.x ,position.height - buttonSize.y );
            buttonRect = new Rect(buttonPosition, buttonSize);
            if (GUI.Button(buttonRect,"Save"))
            {
                EditorManager.SaveData();
            }
        }

        private void OpenContextMenu(Vector2 contextPosition)
        {
            List<Type> ignoreType = new List<Type>()
            {
                typeof(PointNode)
            };
            Type[] types = ReflectionHandler.GetDerivedTypes(typeof(Node));
            GenericMenu contextMenu = new GenericMenu();
            foreach (Type type in types)
            {
                if (!ignoreType.Contains(type))
                {
                    contextMenu.AddItem(new GUIContent(type.Name), false, () =>
                    {
                        Node node = graph.CreateNode(type, contextPosition);
                        CreateNode(node);
                    });
                }
            }
            contextMenu.ShowAsContext();
        }
    
        public void CreateNode(Node node)
        {
            Type nodeType = node.GetType();
            Type editorType = EditorManager.GetCustomEditor(nodeType);
            NodeEditor nodeEditor = Activator.CreateInstance(editorType) as NodeEditor;
            nodeEditor.Init(node,this);
            nodesEditors.Add(node,nodeEditor);
        }

        public void RemoveNode(NodeEditor nodeEditor)
        {
            nodesEditors.Remove(nodeEditor.node);
            graph.RemoveDialogue(nodeEditor.node);
            EditorManager.SaveData();
            Repaint();
        }
        
        private void DragNodes(Vector2 newPosition)
        {
            if (nodesEditors != null)
            {
                if (nodesEditors.Count > 0)
                {
                    foreach (Node node in nodesEditors.Keys)
                    {
                       nodesEditors[node].UpdatePosition(newPosition);
                    }
                }
            }
        }

        public void SelectInPort(PortEditor portEditor)
        {
            if (_SelectdOutPort != portEditor)
            {
                _SelectINPort = portEditor;
                CreateConnection();
            }
            else
            {
                ClearConnection();
            }
        }

        public void SelectOutPort(PortEditor portEditor)
        {
            if (_SelectINPort != portEditor)
            {
                _SelectdOutPort = portEditor;
                CreateConnection();
            }
            else
            {
                ClearConnection();
            }
        }

        private void CreateConnection()
        {
            if (_SelectINPort != null && _SelectdOutPort != null)
            {
                if (connections == null)
                {
                    connections = new Dictionary<PortEditor, PortEditor>();
                }

                graph.CreateConnection(_SelectdOutPort.m_port,_SelectINPort.m_port);
                connections.Add(_SelectdOutPort,_SelectINPort);
                EditorManager.SaveData();
                ClearConnection();
            }
        }

        public void LoadConnection()
        {
            connections = new Dictionary<PortEditor, PortEditor>();

            foreach (Port key in graph.connections.Keys)
            {
                Port outPort = key;
                Port inPort = graph.connections[key];
                NodeEditor outNodeEditor = nodesEditors[outPort.GetNode()];
                NodeEditor inNodeEditor = nodesEditors[inPort.GetNode()];
                _SelectdOutPort = outNodeEditor.GetPortEditor(outPort);
                _SelectINPort = inNodeEditor.GetPortEditor(inPort);
                connections.Add(_SelectdOutPort, _SelectINPort);
            }
            EditorManager.SaveData();
            ClearConnection();
        }
        
        
        

        public void RemoveConnection(PortEditor outPortEditor)
        {
            Port outPort = outPortEditor.m_port;
            foreach (PortEditor portEditor in connections.Keys)
            {
                if (portEditor.m_port.id == outPort.id)
                {
                    connections.Remove(portEditor);
                    graph.RemoveConnection(outPort);
                    return;
                }
            }
            

        }

        public Connection FindConnector(DialogueNodeEditor startNodeEditor, OptionEditor optionEditor = null)
        {
            return null;
        }

        private void ClearConnection()
        {
            _SelectINPort = null;
            _SelectdOutPort = null;
        }

        public void ClearData()
        {
            id = null;
            
            if (nodesEditors != null)
            {
                nodesEditors.Clear();
            }

            if (connections != null)
            {
                connections = new Dictionary<PortEditor, PortEditor>();
            }

            _SelectINPort = null;
            _SelectdOutPort = null; 
        }

        public NodeEditor GetNode(Node node)
        {
            if (nodesEditors.ContainsKey(node))
            {
                return nodesEditors[node];
            }

            return null;
        }
    }
}




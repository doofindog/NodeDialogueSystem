using System;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Editor.NodeComponents;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [System.Serializable]
    public class GraphWindow : EditorWindow
    {
        public string id;
        public DialogueGraph dialogueGraph;

        public Dictionary<Node,BaseNodeEditor> nodesEditors;
        public Dictionary<PortEditor, PortEditor> connections;

        private PortEditor _SelectINPort;
        private PortEditor _SelectdOutPort;
        
        private Vector2 offset;
        private Vector2 drag;


        public void OnEnable()
        {
            this.dialogueGraph = null;
            id = null;
            _SelectINPort = null;
            _SelectdOutPort = null; 
        }

        public void Initialise(DialogueGraph dialogueGraph)
        {
            
            this.dialogueGraph = dialogueGraph;
            id = dialogueGraph.id;
            
            nodesEditors = new Dictionary<Node, BaseNodeEditor>();
            connections = new Dictionary<PortEditor, PortEditor>();
            DialogueSystem.Node[] nodes = dialogueGraph.GetNodes();
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
            if (dialogueGraph != null)
            {
                GUILayout.Label("Dialogue Editor Window", EditorStyles.boldLabel);
                
                DrawGrid(20, 0.2f, Color.gray);
                DrawGrid(100, 0.4f, Color.gray);
                
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
                        Node node = dialogueGraph.CreateNode(type, contextPosition);
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
            BaseNodeEditor baseNodeEditor = Activator.CreateInstance(editorType) as BaseNodeEditor;
            baseNodeEditor.Init(node,this);
            nodesEditors.Add(node,baseNodeEditor);
        }

        public void RemoveNode(BaseNodeEditor baseNodeEditor)
        {
            nodesEditors.Remove(baseNodeEditor.node);
            dialogueGraph.RemoveDialogue(baseNodeEditor.node);
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

                dialogueGraph.CreateConnection(_SelectdOutPort.m_port,_SelectINPort.m_port);
                connections.Add(_SelectdOutPort,_SelectINPort);
                EditorManager.SaveData();
                ClearConnection();
            }
        }

        public void LoadConnection()
        {
            connections = new Dictionary<PortEditor, PortEditor>();

            foreach (Port key in dialogueGraph.connections.Keys)
            {
                Port outPort = key;
                Port inPort = dialogueGraph.connections[key];
                BaseNodeEditor outBaseNodeEditor = nodesEditors[outPort.GetNode()];
                BaseNodeEditor inBaseNodeEditor = nodesEditors[inPort.GetNode()];
                _SelectdOutPort = outBaseNodeEditor.GetPortEditor(outPort);
                _SelectINPort = inBaseNodeEditor.GetPortEditor(inPort);
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
                    dialogueGraph.RemoveConnection(outPort);
                    return;
                }
            }
            

        }

        public Connection FindConnector(DialogueNodeEditor startBaseNodeEditor, NodeOptionEditor nodeOptionEditor = null)
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

        public BaseNodeEditor GetNode(Node node)
        {
            if (nodesEditors.ContainsKey(node))
            {
                return nodesEditors[node];
            }

            return null;
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




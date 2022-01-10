using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [System.Serializable]
    public class NodeGraphWindow : EditorWindow
    {
        public string id;
        public DialogueGraph dialogueGraph;

        public List<Node> nodes;
        public List<Connection> connections;
        
        private ConnectionPort _inConnectionPort;
        private ConnectionPort _outConnectionPort;
        
        public void Initialise(DialogueGraph dialogueGraph)
        {
            ClearData();
            this.dialogueGraph = dialogueGraph;
            id = dialogueGraph.id;
            EditorManager.LoadData(dialogueGraph);
        }
        
        private void OnGUI()
        {
            if (dialogueGraph != null)
            {
                GUILayout.Label("Dialogue Editor Window", EditorStyles.boldLabel);

                DrawNodeGUI();
                DrawConnectionsGUI();
                ProcessNodeEvents(Event.current);
                ProcessEvents(Event.current);
                

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
            if (nodes != null)
            {
                foreach (TextNode node in nodes)
                {
                    node.ProcessEvent(e);
                }
            }
        }
    
        private void DrawNodeGUI()
        {
            if (nodes != null)
            {
                foreach (TextNode node in nodes)
                {
                    node.Draw();
                }
            }
        }

        private void DrawConnectionsGUI()
        {
            if (connections == null)
            {
                connections = new List<Connection>();
            }
            
            foreach (Connection connection in connections.ToList())
            {
                connection.Draw();
            }
        }

        private void OpenContextMenu(Vector2 contextPosition)
        {
            GenericMenu contextMenu = new GenericMenu();
            contextMenu.AddItem(new GUIContent("Add Text Node"), false,() => CreateNode(contextPosition, dialogueGraph.CreateDialogue(typeof(TextDialogue))));
            contextMenu.ShowAsContext();
        }
    
        public TextNode CreateNode(Vector2 nodePosition,DialogueSystem.Dialogue dialogue)
        {
            if(nodes == null)
            {
                nodes = new List<Node>();
            }

            switch (dialogue.type)
            {
                case Dialogue.TypeContrain.TEXT:
                {
                    TextNode node = new TextNode(nodePosition, RemoveNode, SelectInConnectionPoint, SelectOutConnectionPoint,(TextDialogue)dialogue);
                    nodes.Add(node);
                    EditorManager.SaveData();
                    return node;
                }
            }

            return null;
        }
        
        private void RemoveNode(Node node)
        {
            foreach (Connection connection in connections.ToList())
            {
                if (connection.endPort == node.inPort)
                {
                    RemoveConnection(connection);
                }
            }

            foreach (Connection connection in connections.ToList())
            {
                if (connection.startPort.node.id == node.outPoint[0].node.id)
                {
                    RemoveConnection(connection);
                }
            }

            dialogueGraph.RemoveDialogue(node.dialogue);
            nodes.Remove(node);
            EditorManager.SaveData();
            Repaint();
        }
        
        private void DragNodes(Vector2 newPosition)
        {
            if (nodes != null)
            {
                if (nodes.Count > 0)
                {
                    foreach (TextNode node in nodes)
                    {
                        node.UpdatePosition(newPosition);
                    }
                }
            }
        }

        private void SelectInConnectionPoint(ConnectionPort port)
        {
            if (_inConnectionPort != port)
            {
                _inConnectionPort = port;
                CreateConnection();
            }
            else
            {
                ClearConnection();
            }
        }

        private void SelectOutConnectionPoint(ConnectionPort port)
        {
            if (_inConnectionPort != port)
            {
                _outConnectionPort = port;
                CreateConnection();
            }
            else
            {
                ClearConnection();
            }
        }

        private void CreateConnection()
        {
            if (_inConnectionPort != null && _outConnectionPort != null)
            {
                Node endDialogueNode = _inConnectionPort.node;
                Node startDialogueNode = _outConnectionPort.node;
                Dialogue endTextDialogue = endDialogueNode.dialogue;
                Dialogue startTextDialogue = startDialogueNode.dialogue;
                DialogueSystem.Connection connection = null;
                
                
                connection = dialogueGraph.CreateConnection(startTextDialogue,endTextDialogue);
                connections.Add(new Connection(_outConnectionPort, _inConnectionPort,RemoveConnection, connection.ID));
                EditorManager.SaveData();
                ClearConnection();
            }
        }

        public void CreateConnection(ConnectionPort inConnectionPort, ConnectionPort outConnectionPort)
        {
            Node endTextNode = inConnectionPort.node;
            Node startTextNode = outConnectionPort.node;
            Dialogue endTextDialogue = endTextNode.dialogue;
            Dialogue startTextDialogue = startTextNode.dialogue;
            DialogueSystem.Connection connection = null;
            connection = dialogueGraph.CreateConnection(startTextDialogue,endTextDialogue);
            connections.Add(new Connection(outConnectionPort, inConnectionPort,RemoveConnection, connection.ID));
            EditorManager.SaveData();
            ClearConnection();
        }

        public void RemoveConnection(Connection connection)
        {
            ConnectionPort startPort = connection.startPort;
            ConnectionPort endPort = connection.endPort;
            Node startDialogueNode = startPort.node; 
            Node endDialogueNode = endPort.node;
            Dialogue startTextDialogue = startDialogueNode.dialogue;
            Dialogue endTextDialogue = endDialogueNode.dialogue;
            
            dialogueGraph.RemoveConnection(startTextDialogue, endTextDialogue);
            
            connections.Remove(connection);
            EditorManager.SaveData();
            Repaint();
        }

        public Connection FindConnector(TextNode startNode, OptionEditor optionEditor = null)
        {
            return null;
        }

        private void ClearConnection()
        {
            _inConnectionPort = null;
            _outConnectionPort = null;
        }

        public void ClearData()
        {
            id = null;
            
            if (nodes != null)
            {
                nodes.Clear();
            }

            if (connections != null)
            {
                connections.Clear();
            }

            _inConnectionPort = null;
            _outConnectionPort = null; 
        }

        public TextNode GetNode(Dialogue dialogue)
        {
            foreach (TextNode node in nodes)
            {
                if (node.textDialogue.id == dialogue.id)
                {
                    return node;
                }
            }

            return null;
        }
    }
}




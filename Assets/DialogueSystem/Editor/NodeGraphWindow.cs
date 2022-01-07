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
            contextMenu.AddItem(new GUIContent("Add Text Node"), false,() => CreateNode(contextPosition, dialogueGraph.CreateDialogue(Dialogue.TypeContrain.TEXT)));
            contextMenu.ShowAsContext();
        }
    
        public TextNode CreateNode(Vector2 nodePosition,Dialogue dialogue)
        {
            if(nodes == null)
            {
                nodes = new List<Node>();
            }
            
            TextNode node = new TextNode(nodePosition, RemoveNode, SelectInConnectionPoint, SelectOutConnectionPoint,dialogue);
            nodes.Add(node);
            EditorManager.SaveData();

            return node;
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
                if (node.outPoint.Count == 1)
                {
                    if (connection.startPort.dialogueNode.id == node.outPoint[0].dialogueNode.id)
                    {
                        RemoveConnection(connection);
                    }
                }
                else
                {
                    foreach (OptionEditor option in node.options)
                    {
                        ConnectionPort outPort;
                        node.outPoints.TryGetValue(option.option.id,out outPort);

                        if (connection.startPort.ID == outPort.ID)
                        {
                            RemoveConnection(connection);
                        }
                    }
                }
            }

            dialogueGraph.RemoveDialogue(node.textDialogue);
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
                Node endDialogueNode = _inConnectionPort.dialogueNode;
                Node startDialogueNode = _outConnectionPort.dialogueNode;
                TextDialogue endTextDialogue = endDialogueNode.textDialogue;
                TextDialogue startTextDialogue = startDialogueNode.textDialogue;
                global::Connection connection = null;
                
                if (startTextDialogue.options.Count > 0)
                {
                    foreach (Option option in startTextDialogue.options)
                    {
                        if (_outConnectionPort == startDialogueNode.outPoints[option.id])
                        {
                            connection = dialogueGraph.CreateConnection(startTextDialogue,endTextDialogue,option);
                        }
                    }
                }
                else
                {
                    connection = dialogueGraph.CreateConnection(startTextDialogue,endTextDialogue);
                }

                 
                
                connections.Add(new Connection(_outConnectionPort, _inConnectionPort,RemoveConnection, connection.ID));
                EditorManager.SaveData();
                ClearConnection();
            }
        }

        public void CreateConnection(ConnectionPort inConnectionPort, ConnectionPort outConnectionPort)
        {
            TextNode endTextNode = inConnectionPort.dialogueNode;
            TextNode startTextNode = outConnectionPort.dialogueNode;
            TextDialogue endTextDialogue = endTextNode.textDialogue;
            TextDialogue startTextDialogue = startTextNode.textDialogue;
            global::Connection connection = null;

            if (endTextDialogue.options.Count > 0)
            {
                foreach (Option option in startTextDialogue.options)
                {
                    if (outConnectionPort == startTextNode.outPoints[option.id])
                    {
                        connection = dialogueGraph.CreateConnection(startTextDialogue,endTextDialogue,option);
                    }
                }
            }
            else
            {
                connection = dialogueGraph.CreateConnection(startTextDialogue,endTextDialogue);
            }
                
            connections.Add(new Connection(outConnectionPort, inConnectionPort,RemoveConnection, connection.ID));
            EditorManager.SaveData();
            ClearConnection();
        }

        public void RemoveConnection(Connection connection)
        {
            ConnectionPort startPort = connection.startPort;
            ConnectionPort endPort = connection.endPort;
            Node startDialogueNode = startPort.dialogueNode; 
            Node endDialogueNode = endPort.dialogueNode;
            TextDialogue startTextDialogue = startDialogueNode.textDialogue;
            TextDialogue endTextDialogue = endDialogueNode.textDialogue;
            if (connection.optionEd == null)
            {
                dialogueGraph.RemoveConnection(startTextDialogue, endTextDialogue);
            }
            else
            {
                dialogueGraph.RemoveConnection(startTextDialogue,endTextDialogue,connection.optionEd.option);
            }


            connections.Remove(connection);
            EditorManager.SaveData();
            Repaint();
        }

        public Connection FindConnector(TextNode startNode, OptionEditor optionEditor = null)
        {
            if (optionEditor != null)
            {
                foreach (Connection connector in connections)
                {
                    TextNode toFindTextNode = connector.startPort.dialogueNode;
                    if (toFindTextNode.textDialogue.id == startNode.textDialogue.id)
                    {
                        foreach (OptionEditor toFindOptionEd in toFindTextNode.options)
                        {
                            if (toFindOptionEd.option.id == optionEditor.option.id)
                            {
                                return connector;
                            }
                        }
                    }
                }
            }

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




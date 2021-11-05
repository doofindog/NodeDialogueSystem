using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager.UI;


namespace  DialogueEditor
{
    [System.Serializable]
    public class DialogueWindow : EditorWindow
    {
        public string id;
        public DialoguesScriptable dialoguesScriptable;

        public List<NodeEditor> nodes;
        public List<ConnectionEditor> connections;
        
        private ConnectionPoint inConnectionPoint;
        private ConnectionPoint outConnectionPoint;




        public void Initialise(DialoguesScriptable dialoguesScriptable)
        {
            ClearData();
            this.dialoguesScriptable = dialoguesScriptable;
            id = dialoguesScriptable.id;
            DialogueEditorManager.LoadData(dialoguesScriptable);
        }
        
    
        private void OnGUI()
        {
            if (dialoguesScriptable != null)
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
                        DialogueEditorManager.SaveData();
                    }
                    break;
                }
            }
        }
    
        private void ProcessNodeEvents(Event e)
        {
            if (nodes != null)
            {
                foreach (NodeEditor node in nodes)
                {
                    node.ProcessEvent(e);
                }
            }
        }
    
        private void DrawNodeGUI()
        {
            if (nodes != null)
            {
                foreach (NodeEditor node in nodes)
                {
                    node.Draw();
                }
            }
        }

        private void DrawConnectionsGUI()
        {
            if (connections == null)
            {
                connections = new List<ConnectionEditor>();
            }
            
            foreach (ConnectionEditor connection in connections)
            {
                connection.Draw();
            }
        }

        private void OpenContextMenu(Vector2 contextPosition)
        {
            GenericMenu contextMenu = new GenericMenu();
            contextMenu.AddItem(new GUIContent("Add Node"), false,() => CreateNode(contextPosition, dialoguesScriptable.CreateDialogue()));
            contextMenu.ShowAsContext();
        }
    
        public NodeEditor CreateNode(Vector2 nodePosition,Dialogue dialogue)
        {
            if(nodes == null)
            {
                nodes = new List<NodeEditor>();
            }
            NodeEditor nodeEditor = new NodeEditor(nodePosition, RemoveNode, SelectInConnectionPoint, SelectOutConnectionPoint,dialogue);
            nodes.Add(nodeEditor);
            DialogueEditorManager.SaveData();

            return nodeEditor;
        }
        

        private void RemoveNode(NodeEditor nodeEditor)
        {
            dialoguesScriptable.RemoveDialogue(nodeEditor.dialogue);
            nodes.Remove(nodeEditor);
            DialogueEditorManager.SaveData();
            Repaint();
        }

        private void DragNodes(Vector2 newPosition)
        {
            if (nodes.Count > 0)
            {
                foreach (NodeEditor node in nodes)
                {
                    node.UpdatePositon(newPosition);
                }
            }
        }

        private void SelectInConnectionPoint(ConnectionPoint point)
        {
            if (inConnectionPoint != point)
            {
                inConnectionPoint = point;
                CreateConnection();
            }
            else
            {
                ClearConnection();
            }
        }

        private void SelectOutConnectionPoint(ConnectionPoint point)
        {
            if (inConnectionPoint != point)
            {
                outConnectionPoint = point;
                CreateConnection();
            }
            else
            {
                ClearConnection();
            }
        }

        private void CreateConnection()
        {
            if (inConnectionPoint != null && outConnectionPoint != null)
            {
                connections.Add(new ConnectionEditor(inConnectionPoint, outConnectionPoint));
                ClearConnection();
            }
        }

        private void ClearConnection()
        {
            inConnectionPoint = null;
            outConnectionPoint = null;
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

            inConnectionPoint = null;
            outConnectionPoint = null; 
        }

        public NodeEditor GetNode(Dialogue dialogue)
        {
            foreach (NodeEditor node in nodes)
            {
                if (node.dialogue == dialogue)
                {
                    return node;
                }
            }

            return null;
        }


    }
}



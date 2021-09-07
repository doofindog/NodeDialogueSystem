using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


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
            DialogueEditorManager.LoadData(dialoguesScriptable);
            this.dialoguesScriptable = dialoguesScriptable;
            id = dialoguesScriptable.id;
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

        private void OpenContextMenu(Vector2 position)
        {
            GenericMenu contextMenu = new GenericMenu();
            contextMenu.AddItem(new GUIContent("Add Node"), false,() => AddNode(position));
            contextMenu.ShowAsContext();
        }
    
        public void AddNode(Vector2 position)
        {
            if(nodes == null)
            {
                nodes = new List<NodeEditor>();
            }
            NodeEditor nodeEditor =
                new NodeEditor(position, RemoveNode, SelectInConnectionPoint, SelectOutConnectionPoint);
            nodes.Add(nodeEditor);
        }
    
        private void RemoveNode(NodeEditor nodeEditor)
        {
            nodes.Remove(nodeEditor);
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
                connections.Add(new ConnectionEditor(inConnectionPoint,outConnectionPoint));
                ClearConnection();
            }
        }

        private void ClearConnection()
        {
            inConnectionPoint = null;
            outConnectionPoint = null;
        }

        private void DrawSave()
        {
            
            Vector2 size = new Vector2(100, 50);
            Vector2 position = Vector2.zero + (this.position.size - size);
            Rect rect = new Rect(position, size);
            if (GUI.Button(rect, "Save"))
            {
                DialogueEditorManager.SaveData();
            }
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


    }
}



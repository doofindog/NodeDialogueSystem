using System;
using System.Collections;
using System.Collections.Generic;
using Codice.Client.Common.TreeGrouper;
using UnityEngine;
using UnityEditor;
using DialogueEditor;

namespace  DialogueEditor
{
    public class DialogueWindow : EditorWindow
    {
        private DialoguesScriptable dialoguesScriptable;
        private List<NodeEditor> nodes;

        
    
        public void Initialise(DialoguesScriptable dialoguesScriptable)
        {
            this.dialoguesScriptable = dialoguesScriptable;
            nodes = new List<NodeEditor>();
        }
        
    
        private void OnGUI()
        {
            GUILayout.Label("Dialogue Editor Window", EditorStyles.boldLabel); 
    
            DrawNodeGUI();
            ProcessNodeEvents(Event.current);
            ProcessEvents(Event.current);
            
    
    
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
                        DragNodes(e.delta);
                        e.Use();
                    }
                    break;
                }
            }
        }
    
        private void ProcessNodeEvents(Event e)
        {
            if (nodes.Count > 0)
            {
                foreach (NodeEditor node in nodes)
                {
                    node.ProcessEvent(e);
                }
            }
        }
    
        private void DrawNodeGUI()
        {
            if (nodes.Count > 0)
            {
                foreach (NodeEditor node in nodes)
                {
                    node.Draw();
                }
            }
        }
    
        private void OpenContextMenu(Vector2 position)
        {
            GenericMenu contextMenu = new GenericMenu();
            contextMenu.AddItem(new GUIContent("Add Node"), false,() => AddNode(position));
            contextMenu.ShowAsContext();
        }
    
        private void AddNode(Vector2 position)
        {
            NodeEditor nodeEditor = new NodeEditor(position, RemoveNode);
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
    }
}



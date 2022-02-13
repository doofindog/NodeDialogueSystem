using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [CustomNodeEditor(typeof(DialogueSystem.Node))]
    public abstract class NodeEditor : UnityEditor.Editor
    {
        public string id;
        public GraphWindow graphWindow;
        public DialogueSystem.Node node;

        protected Vector2 padding;
        public Rect rect;
        protected Vector2 size;
        protected Vector2 newSize;
        protected Vector2 spacing;

        protected bool _isSelected;
        protected bool _canDrag;

        public List<PortEditor> portEditors;
        public PortEditor inPortEditor;
        public PortEditor outPortEditor;
        
        public GUIStyle panelStyle;

        

        public virtual void Init(DialogueSystem.Node node,GraphWindow graphWindow)
        {
            id = node.id;
            this.graphWindow = graphWindow;
            this.node = node;
            portEditors = new List<PortEditor>();

            
            this.padding = new Vector2(20, 20);
            size = new Vector2(300, 100);
            rect = new Rect(node.position, size);
            inPortEditor = new PortEditor(node.inPort, this.graphWindow.SelectInPort);
            outPortEditor = new PortEditor(node.outPort, this.graphWindow.SelectOutPort);
            portEditors.Add(outPortEditor);
            portEditors.Add(inPortEditor);
        }


        public virtual void Draw()
        {
            spacing = Vector2.zero;
            rect.size = size;
                        
            panelStyle = new GUIStyle(GUI.skin.box)
            {
                alignment = TextAnchor.UpperCenter
            };
            
            GUI.Box(rect, node.GetType().Name,panelStyle);
        }

        protected virtual void OpenMenu()
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Remove Node"),false, DeleteNode);
            menu.ShowAsContext();
        }

        public virtual void ProcessEvent(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                {
                    if (e.button == 1 && rect.Contains(e.mousePosition))
                    {
                        OpenMenu();
                        e.Use();
                    }

                    if (e.button == 0 && rect.Contains(e.mousePosition))
                    {
                        _isSelected = true;
                        _canDrag = true;
                    }
                    break;
                }
                case EventType.MouseUp:
                {
                    if (e.button == 0 && _isSelected == true)
                    {
                        _isSelected = false;
                        _canDrag = false;
                    }
                    break;
                }
                case EventType.MouseDrag:
                {
                    if (e.button == 0 && _canDrag == true)
                    {
                        UpdatePosition(e.delta);
                        e.Use();
                    }
                    break;
                }
            }
        }

        public void UpdatePosition(Vector2 newPosition)
        {
            node.position += newPosition;
            rect.position = node.position;
        }

        protected virtual void DeleteNode()
        {
            graphWindow.RemoveNode(this);
        }
    
        protected virtual void DrawInPorts()
        {
            Vector2 size = new Vector2(20,20);
            Vector2 position = rect.position;
            position.x -= size.x * 0.5f;
            position.y += (rect.size.y * 0.5f) - (size.y * 0.5f);

            Rect inRect = new Rect(position, size);
            
            inPortEditor.Draw(inRect);
        }

        protected virtual void DrawOutPorts()
        {
            Vector2 size = new Vector2(20,20);
            Vector2 position = Vector2.zero;
            position = rect.position;
            position.x += (rect.size.x) - size.x * 0.5f;
            position.y += (rect.size.y * 0.5f) - (size.y * 0.5f);

            Rect buttonRect = new Rect(position, size);
            
            outPortEditor.Draw(buttonRect);
        }

        public PortEditor GetPortEditor(Port port)
        {
            foreach (PortEditor portEditor in portEditors)
            {
                if (port.id == portEditor.m_port.id)
                {
                    return portEditor;
                }
            }
            return null;
        }
    }
}

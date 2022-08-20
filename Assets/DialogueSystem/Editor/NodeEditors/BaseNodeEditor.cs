using System;
using System.Collections.Generic;
using DialogueSystem.Editor.NodeComponents;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [CustomNodeEditor(typeof(DialogueSystem.Node))]
    public abstract class BaseNodeEditor : UnityEditor.Editor
    {
        public string id;
        public GraphWindow graphWindow;
        public DialogueSystem.Node node;

        public Vector2 padding;
        public Rect rect;
        protected Vector2 size;
        protected Vector2 newSize;
        public Vector2 spacing;
        public Vector2 drawPosition;

        private bool m_isSelected;
        private bool m_canDrag;

        public List<PortEditor> portEditors;
        protected PortEditor inPortEditor;
        protected PortEditor outPortEditor;
        
        public GUIStyle panelStyle;

        protected GenericMenu menu;
        
        public virtual void Init(DialogueSystem.Node node,GraphWindow graphWindow)
        {
            id = node.id;
            this.graphWindow = graphWindow;
            this.node = node;
            portEditors = new List<PortEditor>();

            menu = new GenericMenu();
            ConfigMenu();
            ConfigPorts();
            
            
            this.padding = new Vector2(20, 20);
            size = new Vector2(300, 100);
            rect = new Rect(node.position, size);
        }

        public virtual void Draw()
        {
            spacing = Vector2.zero;
            rect.size = size;
            panelStyle = new GUIStyle(GUI.skin.box) { alignment = TextAnchor.UpperCenter };
            
            GUI.Box(rect, node.GetType().Name,panelStyle);
            DrawInPorts();
            DrawOutPorts();
        }

        protected virtual void ConfigMenu()
        {
            menu.AddItem(new GUIContent("Remove Node"),false, DeleteNode);
        }

        protected virtual void ConfigPorts()
        {
            inPortEditor = new PortEditor(node.inPort, this.graphWindow.SelectInPort, this, EPortType.IN);
            outPortEditor = new PortEditor(node.outPort, this.graphWindow.SelectOutPort, this, EPortType.OUT);
            
            portEditors.Add(outPortEditor);
            portEditors.Add(inPortEditor);
        }

        protected virtual void OpenMenu()
        {
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
                        HandleRightClick();
                        e.Use();
                    }

                    if (e.button == 0 && rect.Contains(e.mousePosition))
                    {
                        m_isSelected = true;
                        m_canDrag = true;
                    }
                    break;
                }
                case EventType.MouseUp:
                {
                    if (e.button == 0 && m_isSelected == true)
                    {
                        m_isSelected = false;
                        m_canDrag = false;
                    }
                    break;
                }
                case EventType.MouseDrag:
                {
                    if (e.button == 0 && m_canDrag == true)
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
            inPortEditor.Draw();
        }

        protected virtual void DrawOutPorts()
        {
            outPortEditor.Draw();
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

        protected virtual void HandleRightClick()
        {
            OpenMenu();
        }
    }
}

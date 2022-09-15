using System;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

using DialogueSystem.Editor.NodeComponents;

namespace DialogueSystem.Editor
{
    [CustomNodeEditor(typeof(DialogueSystem.Entry))]
    public abstract class Node : UnityEditor.Editor, IEquatable<Node>
    {
        //ID and References
        [HideInInspector] public DatabaseWindow databaseWindow;
        [HideInInspector] public DialogueSystem.Entry entry;
        [HideInInspector] public Vector2 padding;
        [HideInInspector] public Rect rect;
        [HideInInspector] public Vector2 componentDrawPos;
        [HideInInspector] public Vector2 spacing;
        
        protected Vector2 defaultSize;
        protected GenericMenu menu;
        
        private Vector2 _rescaleSize;
        private bool m_isSelected;
        private bool m_canDrag;
        private GUIStyle _panelStyle;

        protected Port inPort;
        protected Port outPort;
        
        public virtual void Init(DialogueSystem.Entry entry,DatabaseWindow databaseWindow)
        {
            this.databaseWindow = databaseWindow;
            this.entry = entry;

            menu = new GenericMenu();
            
            ConfigMenu();
            
            this.padding = new Vector2(20, 20);
            defaultSize = new Vector2(250, 50);
            _rescaleSize = Vector2.zero; 
            rect = new Rect(entry.GetPosition(), defaultSize);
        }

        protected virtual void ConfigMenu()
        {
            menu.AddItem(new GUIContent("Remove Node"),false, DeleteNode);
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
                    OnMouseDown(e);
                    break;
                }
                case EventType.MouseUp:
                {
                    OnMouseUp(e);
                    break; 
                }
                case EventType.MouseDrag:
                {
                    OnMouseDrag(e);
                    break;
                }
            }
        }

        private void OnMouseDown(Event e)
        {
            if (e.button == 1 && rect.Contains(e.mousePosition))
            {
                OpenMenu();
                e.Use();
            }

            if (e.button == 0 && rect.Contains(e.mousePosition))
            {
                Selection.activeObject = this;
                m_isSelected = true;
                m_canDrag = true;
                e.Use();
            }
        }

        private void OnMouseUp(Event e)
        {
            if (e.button == 0 && m_isSelected == true)
            {
                m_isSelected = false;
                m_canDrag = false;
            }
        }

        private void OnMouseDrag(Event e)
        {
            if (e.button == 0 && m_canDrag == true)
            {
                UpdatePosition(e.delta);
                e.Use();
            }
        }

        public void UpdatePosition(Vector2 deltaPosition)
        {
            rect.position += deltaPosition;
            entry.UpdatePosition(rect.position);
        }

        protected virtual void DeleteNode()
        {
            databaseWindow.RemoveNode(this);
        }
        
        public virtual void Draw()
        {
            rect.size = defaultSize;
            if (_rescaleSize.y + (padding.y*2) > defaultSize.y)
            {
                rect.size = new Vector2(defaultSize.x, _rescaleSize.y + (padding.y * 2));
            }
            
            _panelStyle = new GUIStyle(GUI.skin.box) { alignment = TextAnchor.UpperCenter };
            GUI.Box(rect, entry.GetType().Name);

            componentDrawPos = rect.position + padding;
            _rescaleSize = Vector2.zero;
            
            DrawComponents();
        }

        protected virtual void DrawComponents()
        {
        }

        public void AddComponent(NodeComponent component)
        {
            Vector2 size = component.GetRect().size;
            
            componentDrawPos += new Vector2(0, size.y);
            _rescaleSize += new Vector2(0,size.y);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        public Rect GetRect()
        {
            return rect;
        }

        protected void HandleInPortSelect()
        {
            DatabaseEditorManager.window.SelectDestinationNode(this);
        }

        protected void HandleOutPortSelect()
        {
            DatabaseEditorManager.window.SelectSourceNode(this);
        }

        public Port GetInPort()
        {
            return inPort;
        }

        public Port GetOutPort()
        {
            return outPort;
        }

        #region Equatable
        
        public bool Equals(Node other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Equals(entry, other.entry);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Node) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (entry != null ? entry.GetHashCode() : 0);
            }
        }
        #endregion

    }
}

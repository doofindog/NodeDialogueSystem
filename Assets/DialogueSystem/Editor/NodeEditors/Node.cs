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
        [HideInInspector] public DatabaseWindow databaseWindow;
        [HideInInspector] public DialogueSystem.Entry entry;
        [HideInInspector] public Vector2 padding;
        [HideInInspector] public Rect rect;
        [HideInInspector] public Vector2 componentDrawPos;
        [HideInInspector] public Vector2 spacing;

        private Vector2 _defaultSize;
        private Vector2 _rescaleSize;
        private bool _isSelected;
        private bool _canDrag;
        
        protected Port inPort;
        protected Port outPort;
        protected GenericMenu menu;
        
        public virtual void Init(DialogueSystem.Entry p_entry,DatabaseWindow p_databaseWindow)
        {
            menu = new GenericMenu();
            
            databaseWindow = p_databaseWindow;
            entry = p_entry;
            
            ConfigMenu();
            
            padding = new Vector2(20, 20);
            _defaultSize = new Vector2(250, 50);
            _rescaleSize = Vector2.zero; 
            
            rect = new Rect(p_entry.GetPosition(), _defaultSize);
        }

        protected virtual void ConfigMenu()
        {
            menu.AddItem(new GUIContent("Remove Node"),false, DeleteNode);
        }

        private void OpenMenu()
        {
            menu.ShowAsContext();
        }

        public virtual void ProcessEvent(Event p_event)
        {
            switch (p_event.type)
            {
                case EventType.MouseDown:
                {
                    OnMouseDown(p_event);
                    break;
                }
                case EventType.MouseUp:
                {
                    OnMouseUp(p_event);
                    break; 
                }
                case EventType.MouseDrag:
                {
                    OnMouseDrag(p_event);
                    break;
                }
            }
        }

        protected virtual void OnMouseDown(Event p_event)
        {
            if (p_event.button == 1 && rect.Contains(p_event.mousePosition))
            {
                OpenMenu();
                p_event.Use();
            }

            if (p_event.button == 0 && rect.Contains(p_event.mousePosition))
            {
                Selection.activeObject = entry;
                _isSelected = true;
                _canDrag = true;
                p_event.Use();
            }
        }

        protected virtual void OnMouseUp(Event p_event)
        {
            if (p_event.button == 0 && _isSelected == true)
            {
                _isSelected = false;
                _canDrag = false;
            }
        }

        protected virtual void OnMouseDrag(Event p_event)
        {
            if (p_event.button == 0 && _canDrag == true)
            {
                UpdatePosition(p_event.delta);
                p_event.Use();
            }
        }

        public void UpdatePosition(Vector2 p_deltaPosition)
        {
            rect.position += p_deltaPosition;
            entry.UpdatePosition(rect.position);
        }

        protected virtual void DeleteNode()
        {
            databaseWindow.RemoveNode(this);
        }
        
        public void Draw()
        {
            rect.size = _defaultSize;
            if (_rescaleSize.y + (padding.y*2) > _defaultSize.y)
            {
                rect.size = new Vector2(_defaultSize.x, _rescaleSize.y + (padding.y * 2));
            }
            
            GUI.Box(rect, entry.GetType().Name);

            componentDrawPos = rect.position + padding;
            _rescaleSize = Vector2.zero;
            
            DrawComponents();
        }

        protected virtual void DrawComponents()
        {
        }

        public void AddComponent(NodeComponent p_component)
        {
            Vector2 size = p_component.GetRect().size;
            
            componentDrawPos += new Vector2(0, size.y);
            _rescaleSize += new Vector2(0,size.y);
        }

        public Rect GetRect()
        {
            return rect;
        }

        protected void HandleInPortSelect()
        {
            DatabaseEditorManager.WINDOW.SelectDestinationNode(this);
        }

        protected void HandleOutPortSelect()
        {
            DatabaseEditorManager.WINDOW.SelectSourceNode(this);
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
        
        public bool Equals(Node p_other)
        {
            if (ReferenceEquals(null, p_other)) return false;
            if (ReferenceEquals(this, p_other)) return true;
            return base.Equals(p_other) && Equals(entry, p_other.entry);
        }
        public override bool Equals(object p_obj)
        {
            if (ReferenceEquals(null, p_obj)) return false;
            if (ReferenceEquals(this, p_obj)) return true;
            if (p_obj.GetType() != this.GetType()) return false;
            return Equals((Node) p_obj);
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

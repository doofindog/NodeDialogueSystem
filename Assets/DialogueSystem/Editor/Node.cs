using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [CustomEditor(typeof(DialogueSystem.Dialogue))]
    public class Node : UnityEditor.Editor
    {
        public readonly string id;
        public DialogueSystem.Dialogue dialogue;

        protected Vector2 padding;
        public Rect rect;
        protected Vector2 spacing;

        protected bool _isSelected;
        protected bool _canDrag;

        public ConnectionPort inPort;
        public List<ConnectionPort> outPoint;

        public Action<ConnectionPort> onInPointClick;
        public Action<ConnectionPort> onOutPointClick;

        
        public Node(Vector2 position, Action<ConnectionPort> onInClick,
            Action<ConnectionPort> onOutClick, DialogueSystem.Dialogue dialogue)
        {
            id = dialogue.id;
            this.dialogue = dialogue;

            this.padding = new Vector2(20, 20);
            Vector2 size = new Vector2(300, 100);
            rect = new Rect(position, size);

            onInPointClick = onInClick;
            onOutPointClick = onOutClick;
        
            inPort = new ConnectionPort(this,ConnectionPointType.IN,onInClick);
            outPoint = new List<ConnectionPort>()
            {
                new ConnectionPort(this,ConnectionPointType.OUT,onOutClick)
            };
        }
    
        public virtual void Draw()
        {
        
        }

        protected virtual void OpenMenu()
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Remove Node"),false, DeleteNode);
            menu.ShowAsContext();
        }
        
        public void UpdatePosition(Vector2 newPosition)
        {
            rect.position += newPosition;
        }

        protected virtual void DeleteNode()
        {
        
        }
    
        protected virtual void DrawInConnectionPoint()
        {
            Vector2 size = new Vector2(20,20);
            Vector2 position = rect.position;
            position.x -= size.x * 0.5f;
            position.y += (rect.size.y * 0.5f) - (size.y * 0.5f);

            Rect inRect = new Rect(position, size);
            
            
            inPort.Draw(inRect);
        }

        protected virtual void DrawOutConnectionPoint()
        {
            Vector2 size = new Vector2(20,20);
            Vector2 position = Vector2.zero;
            position = rect.position;
            position.x += (rect.size.x) - size.x * 0.5f;
            position.y += (rect.size.y * 0.5f) - (size.y * 0.5f);

            Rect buttonRect = new Rect(position, size);
                
                
            outPoint[0].Draw(buttonRect);
        }
    }
}

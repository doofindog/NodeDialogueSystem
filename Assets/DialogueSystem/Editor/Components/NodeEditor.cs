using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DialogueEditor;

namespace DialogueEditor
{
    public class NodeEditor
    {
        public int id;
        
        public Vector2 padding;
        public Rect rect;
        public Vector2 spacing;

        private Action<NodeEditor> OnRemoveNode;
        private bool isSelected;
        private bool canDrag;

        private TextEditor mainText;
        private List<OptionEditor> options;

        private ConnectionPoint inPoint;
        private ConnectionPoint outPoint;
        private List<ConnectionPoint> optionPoint;

        private Action<ConnectionPoint> OnInPointClick;
        private Action<ConnectionPoint> OnOutPointClick;
        
        public NodeEditor(Vector2 position,Action<NodeEditor> onRemoveNode, Action<ConnectionPoint> onInClick, Action<ConnectionPoint> onOutClick)
        {

            this.padding = new Vector2(20, 20);
            Vector2 size = new Vector2(300, 100);
            rect = new Rect(position, size);

            OnRemoveNode = onRemoveNode;

            mainText = new TextEditor();
            options = new List<OptionEditor>();
            OnInPointClick = onInClick;
            OnOutPointClick = onOutClick;
            inPoint = new ConnectionPoint(ConnectionPointType.IN,onInClick);
            outPoint = new ConnectionPoint(ConnectionPointType.OUT,onOutClick);
            optionPoint = new List<ConnectionPoint>();
            
            
        }
        
    
        public void Draw()
        {
            GUI.Box(rect, string.Empty);
            spacing = new Vector2(0, 0);
            
            DrawDialogueText(45);
            DrawOptionText(20);
            
            DrawInConnectors();
            DrawOutConnector();
            
            
        }
    
        public void ProcessEvent(Event e)
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
                        isSelected = true;
                        canDrag = true;
                    }
                    break;
                }
                case EventType.MouseUp:
                {
                    if (e.button == 0 && isSelected == true)
                    {
                        isSelected = false;
                        canDrag = false;
                    }
                    break;
                }
                case EventType.MouseDrag:
                {
                    if (e.button == 0 && canDrag == true)
                    {
                        UpdatePositon(e.delta);
                        e.Use();
                    }
                    break;
                }
            }
        }
    
        private void OpenMenu()
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Remove Node"),false, DeleteNode);
            menu.AddItem(new GUIContent("Add Option"),false, AddOption);
            menu.ShowAsContext();
        }
    
        private void DeleteNode()
        {
            if (OnRemoveNode != null)
            {
                OnRemoveNode(this);
            }
        }

        public void UpdatePositon(Vector2 newPosition)
        {
            rect.position += newPosition;
        }
        
        public void AddOption()
        {
            OptionEditor option = new OptionEditor();
            options.Add(option);
            rect.size += new Vector2(0, 20 + 5);

            ConnectionPoint outPoint = new ConnectionPoint(ConnectionPointType.OUT,OnOutPointClick);
            optionPoint.Add(outPoint);
        }

        public void DrawDialogueText(float length)
        {
            Vector2 position = rect.position + padding + spacing;
            Vector2 size = new Vector2(rect.size.x - 2 * padding.x, length);
            mainText.Draw(new Rect(position, size));
            spacing.y += length + 10;
        }

        public void DrawOptionText(float length)
        {
            foreach (OptionEditor option in options)
            {
                Vector2 position = rect.position + padding + spacing;
                Vector2 size = new Vector2(rect.size.x - 2 * padding.x, length);
                option.Draw(new Rect(position, size));
                spacing.y += 20 + 5;
            }
        }

        public void DrawInConnectors()
        {
            Vector2 size = new Vector2(20,20);
            Vector2 position = rect.position;
            position.x -= size.x * 0.5f;
            position.y += (rect.size.y * 0.5f) - (size.y * 0.5f);

            Rect inRect = new Rect(position, size);
            
            
            inPoint.Draw(inRect);

            
        }

        void DrawOutConnector()
        {
            Vector2 size = new Vector2(20,20);
            Vector2 position = Vector2.zero;
            if (options.Count == 0)
            {

                position = rect.position;
                position.x += (rect.size.x) - size.x * 0.5f;
                position.y += (rect.size.y * 0.5f) - (size.y * 0.5f);

                Rect buttonRect = new Rect(position, size);
                
                
                outPoint.Draw(buttonRect);
                
            }
            else
            {
                for (int i = 0; i < options.Count; i++)
                {
                    Rect optionRect = options[i].rect;
                    position.x = rect.position.x + rect.size.x - (size.x * 0.5f);
                    position.y = rect.position.y + (Mathf.Abs(rect.position.y - optionRect.position.y))- (optionRect.size.y * 0.5f - size.y * 0.5f);

                    Rect buttonRect = new Rect(position, size);

                    optionPoint[i].Draw(buttonRect);
                }
            }
        }

    }
}


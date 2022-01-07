using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public class TextNode : Node
    {

        public TextDialogue textDialogue;
        public Action<TextNode> onDeleteCallBack;

        public List<OptionEditor> options;
        private OptionEditor _optiontoDelete; 
        
        public readonly Dictionary<string,ConnectionPort> outPoints;
        
        public TextNode(Vector2 position,Action<TextNode> onDeleteCallBack, 
            Action<ConnectionPort> onInClick, Action<ConnectionPort> onOutClick, 
            TextDialogue textDialogue) : base(position,onInClick,onOutClick,textDialogue)
        {
            
            this.textDialogue = textDialogue;
            options = new List<OptionEditor>();
            outPoints = new Dictionary<string, ConnectionPort>();

            this.onDeleteCallBack = onDeleteCallBack;
        }
        
    
        public override void Draw()
        {
            GUI.Box(rect, string.Empty);
            spacing = new Vector2(0, 0);
            
            DrawDialogueText(45);
            DrawOptions(20);
            
            DrawInConnectionPoint();
            DrawOutConnectionPoint();
            
            RemoveOption();
            
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

        protected override void OpenMenu()
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Remove Node"),false, DeleteNode);
            menu.AddItem(new GUIContent("Add Option"),false, () =>
            {
                AddOption(textDialogue.CreateOption());
            });
            menu.ShowAsContext();
        }
        
        private void DrawDialogueText(float length)
        {
            Vector2 position = rect.position + padding + spacing;
            Vector2 size = new Vector2(rect.size.x - 2 * padding.x, length);
            textDialogue.text = GUI.TextArea(new Rect(position, size), textDialogue.text);
            spacing.y += length + 10;
        }
        
        public void AddOption(Option option)
        {
            OptionEditor optionEditor = new OptionEditor(option, AddOptionToDelete);
            options.Add(optionEditor);
            rect.size += new Vector2(0, 20 + 5);

            
            ConnectionPort outPort = new ConnectionPort(this,ConnectionPointType.OUT, onOutPointClick);
            outPoints.Add(option.id,outPort);
        }

        private void AddOptionToDelete(OptionEditor optionEditor)
        {
            textDialogue.RemoveOption(optionEditor.option);
            _optiontoDelete = optionEditor;
            rect.size -= new Vector2(0, 20 + 5);
        }

        private void RemoveOption()
        {
            if (_optiontoDelete != null)
            {
                Connection connectiontoRemove = EditorManager.window.FindConnector(this, _optiontoDelete);
                options.Remove(_optiontoDelete);
                EditorManager.window.RemoveConnection(connectiontoRemove);
                _optiontoDelete = null;
                EditorManager.SaveData();
            }
        }

        private void DrawOptions(float length)
        {
            foreach (OptionEditor option in options)
            {
                Vector2 position = rect.position + padding + spacing;
                Vector2 size = new Vector2(rect.size.x - 2 * padding.x, length);
                option.Draw(new Rect(position, size));
                spacing.y += 20 + 5;
            }
        }

        protected override void DeleteNode()
        {
            if (onDeleteCallBack != null)
            {
                onDeleteCallBack.Invoke(this);
            }
        }

        protected override void DrawOutConnectionPoint()
        {
            Vector2 size = new Vector2(20,20);
            Vector2 position = Vector2.zero;
            if (options.Count == 0)
            {

                position = rect.position;
                position.x += (rect.size.x) - size.x * 0.5f;
                position.y += (rect.size.y * 0.5f) - (size.y * 0.5f);

                Rect buttonRect = new Rect(position, size);
                
                
                outPoint[0].Draw(buttonRect);
                
            }
            else
            {
                for (int i = 0; i < options.Count; i++)
                {
                    Rect optionRect = options[i].rect;
                    position.x = rect.position.x + rect.size.x - (size.x * 0.5f);
                    position.y = rect.position.y + (Mathf.Abs(rect.position.y - optionRect.position.y))- (optionRect.size.y * 0.5f - size.y * 0.5f);

                    Rect buttonRect = new Rect(position, size);

                    ConnectionPort connectionPort = outPoints[options[i].option.id];
                    connectionPort.Draw(buttonRect);
                }
            }
        }




    }
}


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
        public DialogueEditorConfig.NodeConfig config;
        
        public NodeEditor(Vector2 position,Action<NodeEditor> onRemoveNode)
        {
            config = new DialogueEditorConfig.NodeConfig(position, new Vector2(300, 100), new Vector2(20, 20), onRemoveNode);
        }
        
    
        public void Draw()
        {
            GUI.Box(config.rect, config.title);
            config.spacing = new Vector2(0, 0);
            
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
                    if (e.button == 1 && config.rect.Contains(e.mousePosition))
                    {
                        OpenMenu();
                        e.Use();
                    }

                    if (e.button == 0 && config.rect.Contains(e.mousePosition))
                    {
                        config.isSelected = true;
                        config.canDrag = true;
                    }
                    break;
                }
                case EventType.MouseUp:
                {
                    if (e.button == 0 && config.isSelected == true)
                    {
                        config.isSelected = false;
                        config.canDrag = false;
                    }
                    break;
                }
                case EventType.MouseDrag:
                {
                    if (e.button == 0 && config.canDrag == true)
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
            if (config.OnRemoveNode != null)
            {
                config.OnRemoveNode(this);
            }
        }

        public void UpdatePositon(Vector2 newPosition)
        {
            config.rect.position += newPosition;
        }
        
        public void AddOption()
        {
            OptionEditor option = new OptionEditor();
            config.options.Add(option);
            config.rect.size += new Vector2(0, 20 + 5);
        }

        public void DrawDialogueText(float length)
        {
            Vector2 position = config.rect.position + config.padding + config.spacing;
            Vector2 size = new Vector2(config.rect.size.x - 2 * config.padding.x, length);
            config.TextEditor.Draw(new Rect(position, size));
            config.spacing.y += length + 10;
        }

        public void DrawOptionText(float length)
        {
            foreach (OptionEditor option in config.options)
            {
                Vector2 position = config.rect.position + config.padding + config.spacing;
                Vector2 size = new Vector2(config.rect.size.x - 2 * config.padding.x, length);
                option.Draw(new Rect(position, size));
                config.spacing.y += 20 + 5;
            }
        }

        public void DrawInConnectors()
        {
            Vector2 size = new Vector2(20,20);
            Vector2 position = config.rect.position;
            position.x -= size.x * 0.5f;
            position.y += (config.rect.size.y * 0.5f) - (size.y * 0.5f);

            Rect inRect = new Rect(position, size);
            if (GUI.Button(inRect, ""))
            {
                //TODO
            }

            
        }

        void DrawOutConnector()
        {
            Vector2 size = new Vector2(20,20);
            Vector2 position = Vector2.zero;
            if (config.options.Count == 0)
            {

                position = config.rect.position;
                position.x += (config.rect.size.x) - size.x * 0.5f;
                position.y += (config.rect.size.y * 0.5f) - (size.y * 0.5f);

                Rect rect = new Rect(position, size);
                if (GUI.Button(rect, string.Empty))
                {
                    //TODO
                }
            }
            else
            {
                for (int i = 0; i < config.options.Count; i++)
                {
                    Rect optionRect = config.options[i].config.rect;
                    position.x = config.rect.position.x + config.rect.size.x - (size.x * 0.5f);
                    position.y = config.rect.position.y + ( Mathf.Abs(config.rect.position.y - optionRect.position.y)) - (optionRect.size.y * 0.5f - size.y * 0.5f);

                    Rect rect = new Rect(position, size);
                    if (GUI.Button(rect, string.Empty))
                    {
                        //TODO
                    }
                }
            }
        }

    }
}


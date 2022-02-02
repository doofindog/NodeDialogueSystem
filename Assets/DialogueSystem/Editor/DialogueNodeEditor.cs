using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [CustomNodeEditor(typeof(DialogueSystem.DialogueNode))]
    public class DialogueNodeEditor : NodeEditor
    {
        public DialogueNode dialogueNode;
        public List<OptionEditor> options;
        private OptionEditor _optiontoDelete;
        public readonly Dictionary<string,PortEditor> outPoints;

       
        

        public override void Init(Node node, GraphWindow graphWindow )
        {
            base.Init(node, graphWindow);
            dialogueNode = (DialogueNode)node;
        }
        


        public override void Draw()
        {
            base.Draw();
            spacing = new Vector2(0, 0);
            
            DrawDialogueText(45);
            DrawInPorts();
            DrawOutPorts();
            //DrawOptions(20);
        }
    
        public override void ProcessEvent(Event e)
        {
            base.ProcessEvent(e);
        }

        protected override void OpenMenu()
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Remove Node"),false, DeleteNode);
            menu.ShowAsContext();
        }
        
        private void DrawDialogueText(float length)
        {
            Vector2 position = rect.position + padding + spacing;
            Vector2 size = new Vector2(rect.size.x - 2 * padding.x, length);
            dialogueNode.text = GUI.TextArea(new Rect(position, size), dialogueNode.text);
            spacing.y += length + 10;
        }
        
        public void AddOption(Option option)
        {
            OptionEditor optionEditor = new OptionEditor(option, AddOptionToDelete);
            options.Add(optionEditor);
            rect.size += new Vector2(0, 20 + 5);
            
            outPoints.Add(option.id,outPortEditor);
        }

        private void AddOptionToDelete(OptionEditor optionEditor)
        {
            //textDialogue.RemoveOption(optionEditor.option);
            _optiontoDelete = optionEditor;
            rect.size -= new Vector2(0, 20 + 5);
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
        
        
        /*protected override void DrawOutConnectionPoint()
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
        }*/

    }
}


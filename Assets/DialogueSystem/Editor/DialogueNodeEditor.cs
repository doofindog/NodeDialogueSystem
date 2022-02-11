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
        
    }
}


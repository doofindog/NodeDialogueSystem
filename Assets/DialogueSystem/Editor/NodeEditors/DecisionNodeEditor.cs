using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using DialogueSystem.Editor;
using DialogueSystem.Editor.NodeComponents;
using UnityEditor;
using UnityEngine;

namespace  DialogueSystem.Editor
{
    [CustomNodeEditor(typeof(DialogueSystem.DecisionNode))]
    public class DecisionNodeEditor : BaseNodeEditor
    {
        public DecisionNode decisionNode;
        public List<PortEditor> optionPorts;
        private NodeTextEditor m_textEditor; 

        public override void Init(Node node, GraphWindow graphWindow)
        {
            base.Init(node, graphWindow);

            decisionNode = (DecisionNode) node;
            m_textEditor = new NodeTextEditor(decisionNode.text, this, 40);
        }

        protected override void ConfigMenu()
        {
            base.ConfigMenu();
            menu.AddItem(new GUIContent("Add Option"), false, AddOption);
        }

        protected override void OpenMenu()
        {
            menu.ShowAsContext();
        }

        public override void Draw()
        {
            base.Draw();
            decisionNode.text = m_textEditor.Draw();
        }

        private void DrawOptions(float length)
        {
            Option[] options = decisionNode.GetOptions();
            for (int i = 0; i < options.Length; i++)
            {
                Vector2 optionPosition = rect.position + spacing + padding;
                Vector2 optionSize = new Vector2(rect.size.x - 2 * padding.x, length);
                Rect optionRect = new Rect(optionPosition, optionSize);

                options[i].text = GUI.TextArea(optionRect, options[i].text);
                

                if (options.Length > 2)
                {
                    Vector2 buttonSize = new Vector2(15, 15);
                    Vector2 buttonPosition = optionPosition;
                    buttonPosition.x += optionSize.x - (buttonSize.x * 0.5f);
                    buttonPosition.y -= buttonSize.y * 0.5f;

                    Rect buttonrect = new Rect(buttonPosition, buttonSize);

                    if (GUI.Button(buttonrect, "x"))
                    {
                        RemoveOption(options[i]);
                    }
                }

                Vector2 portSize = new Vector2(20, 20);
                Vector2 portPosition = optionPosition + new Vector2(rect.width - (portSize.x+ portSize.x *0.5f), 0);
                Rect portRect = new Rect(portPosition, portSize);

                Port port = decisionNode.GetOptionPort(options[i]);
                PortEditor portEditor = new PortEditor(port, this.graphWindow.SelectOutPort);
                
                portEditor.Draw();
                
                spacing.y += length + 5;
            }


        }

        private void AddOption()
        {
            decisionNode.AddOption();
        }

        private void RemoveOption(Option option)
        {
            decisionNode.RemoveOption(option);
        }

        private void UpdatePort()
        {
            
        }
        
        protected override void HandleRightClick()
        {
            base.HandleRightClick(); 
        }

    }
}


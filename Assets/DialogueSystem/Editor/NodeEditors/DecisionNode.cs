using System.Collections.Generic;
using DialogueSystem.Editor.NodeComponents;
using UnityEngine;

namespace  DialogueSystem.Editor.NodeEditors
{
    [CustomNodeEditor(typeof(DialogueSystem.DecisionEntry))]
    public class DecisionNode : Node
    {
        public DecisionEntry decisionEntry;
        private Dictionary<Option, Port> _optionOutPorts;

        public override void Init(Entry entry, DatabaseWindow databaseWindow)
        {
            base.Init(entry, databaseWindow);
            decisionEntry = (DecisionEntry) entry;
        }

        protected override void ConfigMenu()
        {
            base.ConfigMenu();
            menu.AddItem(new GUIContent("Add Option"), false, AddOption);
            menu.AddItem(new GUIContent("Remove Option"), false, RemoveOption);
        }

        protected override void DrawComponents()
        {
            inPort = NodeComponentUtilt.DrawPort(PortType.In, HandleInPortSelect);
            
            decisionEntry.text = NodeComponentUtilt.DrawText(decisionEntry.text, 50);
 
            NodeComponentUtilt.DrawSpace(10);
            
            _optionOutPorts = new Dictionary<Option, Port>();
            Option[] options = decisionEntry.GetOptions();
            for (int i = 0; i < options.Length; i++)
            {
                options[i].text = NodeComponentUtilt.DrawText(options[i].text, 25);
                
                Rect optionTextRect = NodeComponentUtilt.GetLastDrawnComponent().GetRect();
                float portYPos = optionTextRect.position.y;
                Port port = NodeComponentUtilt.DrawPort(PortType.Out, () =>
                {
                    DatabaseEditorManager.window.SelectSourceNode(this,options[i]);
                }, portYPos);
                _optionOutPorts.Add(options[i], port);
                
                if (i != options.Length - 1)
                {
                    NodeComponentUtilt.DrawSpace(2);
                }
            }
        }

        private void AddOption()
        {
            decisionEntry.AddOption();
        }

        private void RemoveOption()
        {
            int lastOptionIndex = decisionEntry.GetOptions().Length - 1;
            Option option =  decisionEntry.GetOptions()[lastOptionIndex];
            decisionEntry.RemoveOption(option);
        }

        public Port GetOptionPort(Option option)
        {
            if (_optionOutPorts.ContainsKey(option))
            {

                return _optionOutPorts[option];
            }

            return null;
        }
    }
}


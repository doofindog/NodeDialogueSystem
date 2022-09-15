using System.Collections;
using System.Collections.Generic;
using DialogueSystem.Editor.NodeComponents;
using UnityEditor;
using UnityEngine;


namespace DialogueSystem.Editor
{
    [CustomNodeEditor(typeof(DialogueSystem.StartEntry))]
    public class StartNode : Node
    {
        private StartEntry _startEntry;

        public override void Init(Entry entry, DatabaseWindow databaseWindow)
        {
            base.Init(entry, databaseWindow);
            _startEntry = (StartEntry) entry;
        }

        protected override void DrawComponents()
        {
            NodeComponentUtilt.DrawText(_startEntry.GetPointType().ToString(), 50);
            outPort = NodeComponentUtilt.DrawPort(PortType.Out, HandleOutPortSelect);
        }

        protected override void ConfigMenu()
        {
            //base.ConfigMenu();
        }
    }
}


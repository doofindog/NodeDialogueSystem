using System.Collections;
using System.Collections.Generic;
using DialogueSystem.Editor.NodeComponents;
using UnityEditor;
using UnityEngine;


namespace DialogueSystem.Editor
{
    [CustomNodeEditor(typeof(DialogueSystem.PointEntry))]
    public class PointNode : Node
    {
        private PointEntry _pointEntry;

        public override void Init(Entry entry, DatabaseWindow databaseWindow)
        {
            base.Init(entry, databaseWindow);
            _pointEntry = (PointEntry) entry;
        }

        protected override void DrawComponents()
        {
            NodeComponentUtilt.DrawPort(PortType.In);
            NodeComponentUtilt.DrawText(_pointEntry.GetPointType().ToString(), 50);
            NodeComponentUtilt.DrawPort(PortType.Out);
        }
    }
}


using DialogueSystem.Editor.NodeComponents;

namespace DialogueSystem.Editor
{
    [CustomNodeEditor(typeof(DialogueSystem.StartEntry))]
    public class StartNode : Node
    {
        private StartEntry _startEntry;

        public override void Init(Entry p_entry, DatabaseWindow p_databaseWindow)
        {
            base.Init(p_entry, p_databaseWindow);
            _startEntry = (StartEntry) p_entry;
        }

        protected override void DrawComponents()
        {
            NodeComponentUtilt.DrawText(_startEntry.GetPointType().ToString(), 50);
            outPort = NodeComponentUtilt.DrawPort(PortType.Out, HandleOutPortSelect);
        }
    }
}


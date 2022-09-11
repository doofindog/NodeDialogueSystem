using DialogueSystem.Editor.NodeComponents;

namespace DialogueSystem.Editor.NodeEditors
{
    [CustomNodeEditor(typeof(DialogueSystem.DialogueEntry))]
    public class DialogueNode : Node
    {
        public DialogueEntry dialogueEntry;

        public override void Init(Entry entry, DatabaseWindow databaseWindow )
        {
            base.Init(entry, databaseWindow);
            dialogueEntry = (DialogueEntry)entry;
        }

        protected override void DrawComponents()
        {
            NodeComponentUtilt.DrawPort(PortType.In);
            
            dialogueEntry.text = NodeComponentUtilt.DrawText(dialogueEntry.text, 50);
            
            NodeComponentUtilt.DrawPort(PortType.Out);
        }
    }
}


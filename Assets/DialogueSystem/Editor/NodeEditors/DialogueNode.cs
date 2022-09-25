using DialogueSystem.Editor.NodeComponents;

namespace DialogueSystem.Editor.NodeEditors
{
    [CustomNodeEditor(typeof(DialogueSystem.DialogueEntry))]
    public class DialogueNode : Node
    {
        public DialogueEntry dialogueEntry;

        public override void Init(Entry p_entry, DatabaseWindow p_databaseWindow )
        {
            base.Init(p_entry, p_databaseWindow);
            dialogueEntry = (DialogueEntry)p_entry;
        }

        protected override void DrawComponents()
        {
            inPort = NodeComponentUtilt.DrawPort(PortType.In, HandleInPortSelect);
            
            dialogueEntry.text = NodeComponentUtilt.DrawText(dialogueEntry.text, 50);

            outPort = NodeComponentUtilt.DrawPort(PortType.Out, HandleOutPortSelect);
        }
    }
}


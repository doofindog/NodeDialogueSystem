using DialogueSystem.Editor.NodeComponents;

namespace DialogueSystem.Editor.NodeEditors
{
    [CustomNodeEditor(typeof(DialogueSystem.DialogueNode))]
    public class DialogueNodeEditor : BaseNodeEditor
    {
        public DialogueNode dialogueNode;
        private NodeTextEditor m_nodeTextEditor;
        
        public override void Init(Node node, GraphWindow graphWindow )
        {
            base.Init(node, graphWindow);
            dialogueNode = (DialogueNode)node;
        }

        protected override void ConfigComponents()
        {
            m_nodeTextEditor = AddComponent(new NodeTextEditor(this));
        }

        public override void Draw()
        {
            base.Draw();
            dialogueNode.text = m_nodeTextEditor.Draw(dialogueNode.text);
        }
        
        
    }
}


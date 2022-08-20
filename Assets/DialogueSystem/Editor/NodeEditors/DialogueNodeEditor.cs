using System;
using System.Collections.Generic;
using DialogueSystem.Editor.NodeComponents;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [CustomNodeEditor(typeof(DialogueSystem.DialogueNode))]
    public class DialogueNodeEditor : BaseNodeEditor
    {
        public DialogueNode dialogueNode;
        public List<NodeOptionEditor> options;
        private NodeOptionEditor _optiontoDelete;
        public readonly Dictionary<string,PortEditor> outPoints;
        private NodeTextEditor m_nodeTextEditor;
        
        public override void Init(Node node, GraphWindow graphWindow )
        {
            base.Init(node, graphWindow);
            dialogueNode = (DialogueNode)node;

            m_nodeTextEditor = new NodeTextEditor(dialogueNode.text,this);
        }
        
        public override void Draw()
        {
            base.Draw();
            dialogueNode.text = m_nodeTextEditor.Draw(dialogueNode.text);
        }
        
    }
}


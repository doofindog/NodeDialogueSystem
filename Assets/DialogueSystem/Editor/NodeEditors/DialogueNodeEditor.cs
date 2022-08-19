using System;
using System.Collections.Generic;
using DialogueSystem.Editor.NodeComponents;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [CustomNodeEditor(typeof(DialogueSystem.TextNode))]
    public class DialogueNodeEditor : BaseNodeEditor
    {
        public TextNode textNode;
        public List<OptionEditor> options;
        private OptionEditor _optiontoDelete;
        public readonly Dictionary<string,PortEditor> outPoints;
        private NodeTextEditor m_nodeText;
        
        public override void Init(Node node, GraphWindow graphWindow )
        {
            base.Init(node, graphWindow);
            textNode = (TextNode)node;
            
            m_nodeText = 
        }
        
        public override void Draw()
        {
            base.Draw();
            DrawDialogueText(45);
        }
        
    }
}


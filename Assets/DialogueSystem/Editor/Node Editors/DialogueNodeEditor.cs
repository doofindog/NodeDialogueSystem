using System;
using System.Collections.Generic;
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
        
        public override void Init(Node node, GraphWindow graphWindow )
        {
            base.Init(node, graphWindow);
            textNode = (TextNode)node;
        }
        
        public override void Draw()
        {
            base.Draw();
            
            DrawDialogueText(45);
        }

        private void DrawDialogueText(float length)
        {
            Vector2 position = rect.position + padding + spacing;
            Vector2 size = new Vector2(rect.size.x - 2 * padding.x, length);
            textNode.text = GUI.TextArea(new Rect(position, size), textNode.text);
            spacing.y += length + 10;
        }
    }
}


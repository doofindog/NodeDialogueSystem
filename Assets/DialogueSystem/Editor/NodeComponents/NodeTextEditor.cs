using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Editor.NodeComponents
{
    public class NodeTextEditor : NodeComponent
    {
        private string m_text;
        private int m_length;

        public NodeTextEditor(BaseNodeEditor nodeEditor, int length = 0)
        {
            base.nodeEditor = nodeEditor;
            m_length = length;
        }
        public string Draw(string text = null)
        {
            if (string.IsNullOrEmpty(text))
            {
                text = m_text;
            }
            UpdatePosition();
            UpdateSize(m_length);
            
            m_text = GUI.TextArea(rect, text);
            return m_text;
        }
        protected sealed override void UpdatePosition()
        {
            position = nodeEditor.rect.position + nodeEditor.padding;
            rect.position = position;
        }
        protected sealed override void UpdateSize(int length)
        {
            Vector2 offset = Vector2.zero;
            offset.x = Mathf.Abs(position.x - nodeEditor.rect.position.x);
            offset.y = Mathf.Abs(position.y - nodeEditor.rect.position.y);
            size = new Vector2(nodeEditor.rect.size.x - nodeEditor.padding.x - offset.x, length == 0 ? nodeEditor.rect.size.y - nodeEditor.padding.y - offset.y : length);
            rect.size = size;
        }
    }
}


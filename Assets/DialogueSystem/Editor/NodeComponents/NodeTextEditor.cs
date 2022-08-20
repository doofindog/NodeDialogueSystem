using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Editor.NodeComponents
{
    public class NodeTextEditor
    {
        private BaseNodeEditor m_nodeEditor;
        private Vector2 m_size;
        private Vector2 m_position;
        private Rect m_rect;

        private string m_text;

        public NodeTextEditor(string text, BaseNodeEditor nodeEditor, int length = 0)
        {
            m_nodeEditor = nodeEditor;
            UpdatePosition();
            UpdateSize(length);
        }

        public string Draw(string text = null)
        {
            if (string.IsNullOrEmpty(text))
            {
                text = m_text;
            }
            m_text = GUI.TextArea(m_rect, text);
            UpdatePosition();
            return m_text;
        }

        private void UpdatePosition()
        {
            m_position = m_nodeEditor.rect.position + m_nodeEditor.padding;
            m_rect.position = m_position;
        }

        private void UpdateSize(int length)
        {
            Vector2 offset = Vector2.zero;
            offset.x = Mathf.Abs(m_position.x - m_nodeEditor.rect.position.x);
            offset.y = Mathf.Abs(m_position.y - m_nodeEditor.rect.position.y);
            m_size = new Vector2(m_nodeEditor.rect.size.x - m_nodeEditor.padding.x - offset.x, length == 0 ? m_nodeEditor.rect.size.y - m_nodeEditor.padding.y - offset.y : length);
            m_rect.size = m_size;
        }
    }
}


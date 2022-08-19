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

        private const float length;

        public NodeTextEditor(string text, BaseNodeEditor nodeEditor)
        {
            m_nodeEditor = nodeEditor;
        }

        public void Draw()
        {
            UpdatePosition();
            m_text = GUI.TextArea(m_rect, m_text);
        }

        public void UpdatePosition()
        {
            m_position = m_nodeEditor.rect.position + m_nodeEditor.padding;
            m_size = new Vector2(m_nodeEditor.rect.size.x - 2 * m_nodeEditor.padding.x, length);
            m_rect.position = m_position;
            m_rect.size = m_size;
        }
    }
}


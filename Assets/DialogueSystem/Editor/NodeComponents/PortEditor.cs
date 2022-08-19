using System;
using UnityEngine;

namespace DialogueSystem.Editor.NodeComponents
{
    public class PortEditor
    {
        public Port m_port;

        private BaseNodeEditor m_nodeEditor;
        private Vector2 m_size;
        private Vector2 m_position;
        private Rect m_rect;
        private Action<PortEditor> m_callBack;
        private EPortType m_portType;
        
        private const int maxSizeX = 20;
        private const int maxSizeY = 20;

        public PortEditor(Port port,Action<PortEditor> mCallBack = null, BaseNodeEditor nodeEditor = null, EPortType portType = EPortType.NONE)
        {
            m_port = port;
            m_callBack = mCallBack;
            m_portType = portType;
            m_nodeEditor = nodeEditor;
            
            m_size = new Vector2(maxSizeX, maxSizeY);
            m_position = Vector2.zero;
            m_rect = new Rect(m_position, m_size);
        }

        public void Draw()
        {
            UpdatePosition();

            if (GUI.Button(m_rect, string.Empty))
            {
                if (m_callBack != null)
                {
                    m_callBack(this);
                }
            }
        }

        void UpdatePosition()
        {
            switch (m_portType)
            {
                case EPortType.IN:
                    m_position = m_nodeEditor.rect.position;
                    m_position.x -= m_size.x * 0.5f;
                    m_position.y += (m_nodeEditor.rect.size.y * 0.5f) - (m_size.y * 0.5f);
                    m_rect.position = m_position;
                    break;
                case EPortType.OUT:
                    m_position = m_nodeEditor.rect.position;
                    m_position.x += (m_nodeEditor.rect.size.x) - m_size.x * 0.5f;
                    m_position.y += (m_nodeEditor.rect.size.y * 0.5f) - (m_size.y * 0.5f);
                    m_rect.position = m_position;
                    break;
            }
        }

        public Vector2 GetCenterPosition()
        {
            return m_rect.center;
        }

    }

    public enum EPortType
    {
        NONE,
        IN,
        OUT
    }
}
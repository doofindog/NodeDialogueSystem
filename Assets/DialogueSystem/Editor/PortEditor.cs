using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public class PortEditor
    {
        public Port m_port;

        public Vector2 size;
        public Vector2 position;
        public Rect m_rect;
        private Action<PortEditor> _callBack;
        private bool _isclick;
        
        private const int maxSizeX = 20;
        private const int maxSizeY = 20;

        public PortEditor(Port port,Action<PortEditor> callBack = null)
        {
            m_port = port;
            _callBack = callBack;
            size = new Vector2(maxSizeX, maxSizeY);
            position = Vector2.zero; 
        }

        public void Draw(Rect rect)
        {
            if (GUI.Button(rect, string.Empty))
            {
                _isclick = true;
                if (_callBack != null)
                {
                    _callBack(this);
                }
            }

            m_rect = rect;
        }

        public void SetPosition(Vector2 newPos)
        {
            position = newPos;
        }

        public Vector2 GetPoition()
        {
            return m_rect.position;
        }

        public Vector2 GetCenterPosition()
        {
            return m_rect.center;
        }

    }
}
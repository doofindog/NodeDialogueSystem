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
        
        public Rect m_rect;
        private Action<PortEditor> _callBack;
        private bool _isclick;

        public PortEditor(Port port,Action<PortEditor> callBack = null)
        {
            m_port = port;
            _callBack = callBack;
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
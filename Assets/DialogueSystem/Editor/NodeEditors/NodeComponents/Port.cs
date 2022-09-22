using System;
using UnityEngine;

namespace DialogueSystem.Editor.NodeComponents
{
    public class Port : NodeComponent
    {
        private PortType _portType;
        
        private const int MAX_SIZE_X = 20;
        private const int MAX_SIZE_Y = 20;

        private Action _callBack;

        public Port(Rect p_rect, PortType p_portType, Action p_callBack)
        {
            rect = p_rect;
            _portType = p_portType;
            _callBack = p_callBack;

        }

        public void Draw()
        {
            if (GUI.Button(rect, string.Empty))
            {
                _callBack?.Invoke();
            }
        }

        public Vector2 GetCenterPosition()
        {
            return rect.center;
        }
    }

    public enum PortType
    {
        None,
        In,
        Out
    }
}
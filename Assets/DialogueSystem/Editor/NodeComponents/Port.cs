using System;
using UnityEngine;

namespace DialogueSystem.Editor.NodeComponents
{
    public class Port : NodeComponent
    {
        private PortType _portType;
        
        private const int maxSizeX = 20;
        private const int maxSizeY = 20;

        private Action _callBack;

        public Port(Rect rect, PortType portType, Action callBack)
        {
            canvasRect = rect;
            _portType = portType;
            _callBack = callBack;

        }

        public void Draw()
        {
            if (GUI.Button(canvasRect, string.Empty))
            {
                _callBack?.Invoke();
            }
        }

        public Vector2 GetCenterPosition()
        {
            return canvasRect.center;
        }
    }

    public enum PortType
    {
        None,
        In,
        Out
    }
}
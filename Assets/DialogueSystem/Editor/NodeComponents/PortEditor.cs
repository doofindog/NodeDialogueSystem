using System;
using UnityEngine;

namespace DialogueSystem.Editor.NodeComponents
{
    public class PortEditor : NodeComponent
    {
        private PortType m_portType;
        
        private const int maxSizeX = 20;
        private const int maxSizeY = 20;

        public PortEditor(Rect rect, PortType portType)
        {
            canvasRect = rect;
        }

        public void Draw()
        {
            if (GUI.Button(canvasRect, string.Empty))
            {
                
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
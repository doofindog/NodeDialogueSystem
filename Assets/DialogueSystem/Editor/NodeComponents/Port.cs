using System;
using UnityEngine;

namespace DialogueSystem.Editor.NodeComponents
{
    public class Port : NodeComponent
    {
        private PortType m_portType;
        
        private const int maxSizeX = 20;
        private const int maxSizeY = 20;

        public Port(Rect rect, PortType portType)
        {
            canvasRect = rect;
        }

        public void Draw()
        {
            if (GUI.Button(canvasRect, string.Empty))
            {
                if (m_portType == PortType.Out)
                {
                    DatabaseEditorManager.window.SelectSourceNode(node, this);
                }

                if (m_portType == PortType.In)
                {
                    DatabaseEditorManager.window.SelectDestinationNode(node, this);
                }
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
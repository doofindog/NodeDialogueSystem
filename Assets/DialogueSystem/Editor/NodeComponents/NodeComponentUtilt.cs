using System;
using UnityEngine;

namespace DialogueSystem.Editor.NodeComponents
{
    public static class NodeComponentUtilt
    {
        public static Node focusedNode;
        public static NodeComponent lastDrawnComponent;
        
        public static string DrawText(string text, int length)
        {
            Vector2 position = focusedNode.componentDrawPos;
            Vector2 offset = new Vector2
            {
                x = Mathf.Abs(position.x - focusedNode.rect.position.x),
                y = Mathf.Abs(position.y - focusedNode.rect.position.y)
            };
            Vector2 size = new Vector2
            (
                focusedNode.rect.size.x - focusedNode.padding.x - offset.x, 
                length == 0 ? focusedNode.rect.size.y - focusedNode.padding.y - offset.y : length
            );
            Rect componentRect = new Rect(position, size);
            
            NodeTextArea textArea = new NodeTextArea(componentRect);
            text = textArea.Draw(text);

            lastDrawnComponent = textArea;
            
            focusedNode.AddComponent(textArea);
            
            return text;
        }

        public static void DrawSpace(int length)
        {
            Vector2 position = focusedNode.componentDrawPos;
            Vector2 size = new Vector2(0, length);
            Rect componentRect = new Rect(position, size);
            
            NodeSpace nodeSpace = new NodeSpace(componentRect);
            lastDrawnComponent = nodeSpace;
            
            focusedNode.AddComponent(nodeSpace);
        }

        public static void DrawPort(PortType type, float yPos = 0)
        {
            Vector2 size = new Vector2(20, 20);
            Vector2 position = Vector2.zero;
            switch (type)
            {
                case PortType.In:
                    position.x = (focusedNode.rect.position.x - size.x * 0.5f);
                    break;
                case PortType.Out:
                    position.x = (focusedNode.rect.position.x + focusedNode.rect.size.x - size.x * 0.5f);
                    break;
            }
            position.y = yPos!=0 ? yPos : focusedNode.rect.position.y + (focusedNode.rect.size.y * 0.5f) - (size.y * 0.5f);

            Rect componentRect = new Rect(position, size);

            PortEditor portEditor = new PortEditor(componentRect, type);
            lastDrawnComponent = portEditor;
            portEditor.Draw();
        }

        public static NodeComponent GetLastDrawnComponent()
        {
            return lastDrawnComponent ?? null;
        }
    }
}

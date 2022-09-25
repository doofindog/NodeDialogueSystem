using System;
using UnityEngine;

namespace DialogueSystem.Editor.NodeComponents
{
    public static class NodeComponentUtilt
    {
        public static Node focusedNode;
        private static NodeComponent _lastDrawnComponent;
        
        public static string DrawText(string p_text, int p_length)
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
                p_length == 0 ? focusedNode.rect.size.y - focusedNode.padding.y - offset.y : p_length
            );
            Rect componentRect = new Rect(position, size);
            
            NodeTextArea textArea = new NodeTextArea(componentRect);
            p_text = textArea.Draw(p_text);

            _lastDrawnComponent = textArea;
            
            focusedNode.AddComponent(textArea);
            
            return p_text;
        }

        public static void DrawSpace(int p_length)
        {
            Vector2 position = focusedNode.componentDrawPos;
            Vector2 size = new Vector2(0, p_length);
            Rect componentRect = new Rect(position, size);
            
            NodeSpace nodeSpace = new NodeSpace(componentRect);
            _lastDrawnComponent = nodeSpace;
            
            focusedNode.AddComponent(nodeSpace);
        }

        public static Port DrawPort(PortType p_type, Action p_callBack ,float p_yPos = 0)
        {
            Vector2 size = new Vector2(20, 20);
            Vector2 position = Vector2.zero;
            switch (p_type)
            {
                case PortType.In:
                    position.x = (focusedNode.rect.position.x - size.x * 0.5f);
                    break;
                case PortType.Out:
                    position.x = (focusedNode.rect.position.x + focusedNode.rect.size.x - size.x * 0.5f);
                    break;
            }
            position.y = p_yPos!=0 ? p_yPos : focusedNode.rect.position.y + (focusedNode.rect.size.y * 0.5f) - (size.y * 0.5f);

            Rect componentRect = new Rect(position, size);

            Port port = new Port(componentRect, p_type, p_callBack);
            _lastDrawnComponent = port;
            port.Draw();

            return port;
        }

        public static int DrawPopUp(int p_index, string[] p_content, int p_length)
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
                p_length == 0 ? focusedNode.rect.size.y - focusedNode.padding.y - offset.y : p_length
            );
            Rect componentRect = new Rect(position, size);

            NodePopUp popUp = new NodePopUp(componentRect, p_index, p_content);
            int selectedIndex = popUp.Draw();

            _lastDrawnComponent = popUp;
            
            focusedNode.AddComponent(popUp);
            
            return selectedIndex;
        }

        public static void DrawLine()
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
                2
            );
            Rect componentRect = new Rect(position, size);

            NodeLine line = new NodeLine(componentRect);
            line.Draw();
            
            focusedNode.AddComponent(line);
        }

        public static string DrawTextField(string p_label, string p_text)
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
                20
            );
            Rect componentRect = new Rect(position, size);
            
            NodeTextField textField = new NodeTextField(componentRect);
            p_text = textField.Draw(p_label, p_text);

            _lastDrawnComponent = textField;
            
            focusedNode.AddComponent(textField);
            
            return p_text;
        }

        public static bool DrawToggle(string p_lable,bool p_isTrue)
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
                20
            );
            Rect componentRect = new Rect(position, size);
            
            NodeBoolean boolean = new NodeBoolean(componentRect);
            p_isTrue = boolean.Draw(p_lable,p_isTrue);

            _lastDrawnComponent = boolean;
            
            focusedNode.AddComponent(boolean);
            
            return p_isTrue;
        }

        public static NodeComponent GetLastDrawnComponent()
        {
            return _lastDrawnComponent ?? null;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [CustomNodeEditor(typeof(DialogueSystem.EventNode))]
    public class EventNodeEditor : NodeEditor
    {
        private EventNode eventNode;
        private int typeIndex;
        private int cachedTypeIndex;

        private int methodIndex;
        private int CachedMethodIndex;
        
        private List<string> optiontext;

        public override void Init(Node node, GraphWindow graphWindow)
        {
            base.Init(node, graphWindow);
            eventNode = (EventNode) node;
            optiontext = new List<string>();
            Type[] types = CachedData.GetEventTypes();

            for(int i =0 ;i< types.Length;i++)
            {
                optiontext.Add(types[i].Name);
            }
            
        }

        public override void Draw()
        {
            base.Draw();
            
            Vector2 textPosition = rect.position + padding + spacing;
            Vector2 textSize = new Vector2(rect.size.x - 2 * padding.x,45);
            eventNode.text = GUI.TextArea(new Rect(textPosition, textSize), eventNode.text);
            
            spacing.y += 45 + 10;
            size.y = 100 + spacing.y;
            
            Vector2 holderPosition = rect.position + padding + spacing;
            Vector2 holderSize = new Vector2(rect.size.x - 2 * padding.x,25);
            typeIndex = EditorGUI.Popup(new Rect(holderPosition, holderSize),typeIndex,optiontext.ToArray());
            if (cachedTypeIndex != typeIndex)
            {
                cachedTypeIndex = typeIndex;
                eventNode.SetEventObj(CachedData.GetEvent(typeIndex));
            }

        }
    }
}

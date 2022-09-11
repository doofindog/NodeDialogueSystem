using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [CustomNodeEditor(typeof(DialogueSystem.EventEntry))]
    public class EventNode : Node
    {
        private EventEntry _eventEntry;
        private int typeIndex;
        private int methodIndex;
        


        public override void Init(Entry entry, DatabaseWindow databaseWindow)
        {
            base.Init(entry, databaseWindow);
            _eventEntry = (EventEntry) entry;

        }

        public override void Draw()
        {
            base.Draw();
            
            Vector2 textPosition = rect.position + padding + spacing;
            Vector2 textSize = new Vector2(rect.size.x - 2 * padding.x,45);
            _eventEntry.text = GUI.TextArea(new Rect(textPosition, textSize), _eventEntry.text);
            
            spacing.y += 45 + 10;
            defaultSize.y = 100 + spacing.y;
            
            Vector2 typePosition = rect.position + padding + spacing;
            Vector2 typeSize = new Vector2(rect.size.x - 2 * padding.x,25);

            
            System.Type[] eventTypes = CachedData.eventTypes;
            string[] eventNames = new string[eventTypes.Length];
            for (int i = 0; i < eventTypes.Length; i++)
            {
                eventNames[i] = eventTypes[i].Name;
                if (_eventEntry.m_type.type != null)
                {
                    if (_eventEntry.m_type.type.Name == eventNames[i])
                    {
                        typeIndex = i;
                    }
                }
            }

            typeIndex = EditorGUI.Popup(new Rect(typePosition, typeSize),typeIndex,eventNames);
            MethodInfo[] methods = CachedData.GetMethods(eventTypes[typeIndex]);

            spacing.y += 25 + 10;
            defaultSize.y = 100 + spacing.y;
            
            Vector2 methodPosition = rect.position + padding + spacing;
            Vector2 methodSize = new Vector2(rect.size.x - 2 * padding.x,25);
            
            
            string[] methodNames = new string[methods.Length];
            for (int i = 0; i < methods.Length; i++)
            {
                methodNames[i] = methods[i].Name;
                if (_eventEntry.m_method.methodInfo != null)
                {
                    if (_eventEntry.m_method.methodInfo.Name == methodNames[i])
                    {
                        methodIndex = i;
                    }
                }
            }
            methodIndex = EditorGUI.Popup(new Rect(methodPosition, methodSize),methodIndex,methodNames);
            _eventEntry.SetEvent(methods[methodIndex]);

        }
    }
}
